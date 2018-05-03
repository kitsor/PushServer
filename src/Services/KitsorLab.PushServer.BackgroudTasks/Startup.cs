namespace KitsorLab.PushServer.BackgroudTasks
{
	using System.IO;
	using System.Reflection;
	using KitsorLab.BuildingBlocks.EventBus;
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.BuildingBlocks.EventBus.RabbitMQ;
	using KitsorLab.BuildingBlocks.EventBus.SubscriptionsManagers;
	using KitsorLab.PushServer.BackgroudTasks.Configuration;
	using KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events;
	using KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Handlers;
	using KitsorLab.PushServer.BackgroudTasks.Queries;
	using KitsorLab.PushServer.BackgroudTasks.Services;
	using KitsorLab.PushServer.BackgroudTasks.Tasks;
	using KitsorLab.PushServer.BackgroudTasks.Tasks.Queue;
	using KitsorLab.PushServer.Infastructure;
	using KitsorLab.PushServer.Infastructure.Repositories;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using RabbitMQ.Client;

	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder();

			builder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<PushServerDbContext>(options =>
				options.UseSqlServer(Configuration["ConnectionString"],
					sqlOptions => sqlOptions.MigrationsAssembly(typeof(PushServerDbContext).GetTypeInfo().Assembly.GetName().Name)));

			services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

			ConfigureEventBus(services);
			RegisterEventBus(services);
			RegisterEventBusHandlers(services);

			// options
			services.Configure<DeliveryTaskSettings>(Configuration.GetSection("DeliveryTaskSettings"));
			services.Configure<AppleDeliveryTaskSettings>(Configuration.GetSection("AppleDeliveryTaskSettings"));
			services.Configure<QueueDeliveryTaskSettings>(Configuration.GetSection("QueueDeliveryTaskSettings"));
			services.Configure<WebPushOptions>(Configuration.GetSection("WebPushOptions"));
			services.Configure<AppleWebPushOptions>(Configuration.GetSection("AppleWebPushOptions"));

			// push services
			services.AddTransient<IWebPushService, WebPushService>();
			services.AddTransient<IAppleWebPushService, AppleWebPushService>();

			services.AddSingleton<IDeliveryQueries>(x => new DeliveryQueries(Configuration["ConnectionString"]));
			services.AddSingleton<IDeliveryItemsQueue, DeliveryItemsQueue>();
			services.AddSingleton<IAppleDeliveryItemsQueue, AppleDeliveryItemsQueue>();
			services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
			services.AddTransient<IDeliveryRepository, DeliveryRepository>();

			// background tasks
			services.AddSingleton<IHostedService, QueueDeliveryService>();
			services.AddSingleton<IHostedService, DeliveryService>();
			services.AddSingleton<IHostedService, AppleDeliveryService>();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			RegisterEventBusSubscriptions(app);
		}

		/// <param name="services"></param>
		private void ConfigureEventBus(IServiceCollection services)
		{
			IConfigurationSection configSection = Configuration.GetSection("EventBus");

			services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
			{
				var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

				var factory = new ConnectionFactory()
				{
					HostName = configSection["Connection"],
				};

				string username = configSection["UserName"];
				if (!string.IsNullOrEmpty(username))
				{
					factory.UserName = username;
				}

				string password = configSection["Password"];
				if (!string.IsNullOrEmpty(password))
				{
					factory.Password = password;
				}

				int retryCount = !string.IsNullOrEmpty(configSection["RetryCount"])
					? int.Parse(configSection["RetryCount"])
					: 5;

				return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
			});
		}

		/// <param name="services"></param>
		private void RegisterEventBus(IServiceCollection services)
		{
			IConfigurationSection configSection = Configuration.GetSection("EventBus");

			services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
			{
				var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
				string brokerName = configSection["BrokerName"];
				string queueName = configSection["QueueName"];

				int retryCount = !string.IsNullOrEmpty(configSection["RetryCount"])
					? int.Parse(configSection["RetryCount"])
					: 5;

				return new EventBusRabbitMQ(rabbitMQPersistentConnection, eventBusSubcriptionsManager, brokerName, logger, sp, queueName, retryCount);
			});

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
		}

		/// <param name="services"></param>
		private void RegisterEventBusHandlers(IServiceCollection services)
		{
			services.AddTransient<IIntegrationEventHandler<CreateDeliveryIntegrationEvent>, CreateDeliveryIntegrationEventHandler>();
			services.AddTransient<IIntegrationEventHandler<DoDeliveryIntegrationEvent>, DoDeliveryIntegrationEventHandler>();
			services.AddTransient<IIntegrationEventHandler<DoAppleDeliveryIntegrationEvent>, DoAppleDeliveryIntegrationEventHandler>();
		}

		/// <param name="app"></param>
		private void RegisterEventBusSubscriptions(IApplicationBuilder app)
		{
			IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

			eventBus.Subscribe<CreateDeliveryIntegrationEvent, IIntegrationEventHandler<CreateDeliveryIntegrationEvent>>();
			eventBus.Subscribe<DoDeliveryIntegrationEvent, IIntegrationEventHandler<DoDeliveryIntegrationEvent>>();
			eventBus.Subscribe<DoAppleDeliveryIntegrationEvent, IIntegrationEventHandler<DoAppleDeliveryIntegrationEvent>>();
		}
	}
}
