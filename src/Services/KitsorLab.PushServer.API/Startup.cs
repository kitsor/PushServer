namespace KitsorLab.PushServer.API
{
	using System.IO;
	using System.Reflection;
	using KitsorLab.BuildingBlocks.EventBus;
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.BuildingBlocks.EventBus.RabbitMQ;
	using KitsorLab.BuildingBlocks.EventBus.SubscriptionsManagers;
	using KitsorLab.PushServer.API.Application.IntegrationEvents.Events;
	using KitsorLab.PushServer.API.Application.IntegrationEvents.Handlers;
	using KitsorLab.PushServer.PNS.ApplePush.Safari;
	using KitsorLab.PushServer.Infastructure;
	using KitsorLab.PushServer.Infastructure.Repositories;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using RabbitMQ.Client;

	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			Configuration = CreateConfiguration(env);
		}

		protected virtual IConfiguration CreateConfiguration(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder();

			builder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			return builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<PushServerDbContext>(options =>
				options.UseSqlServer(Configuration["ConnectionString"],
					sqlOptions => sqlOptions.MigrationsAssembly(typeof(PushServerDbContext).GetTypeInfo().Assembly.GetName().Name)));

			services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
			services.AddMvc();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
						builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});

			ConfigureEventBus(services);
			RegisterEventBus(services);
			RegisterEventBusHandlers(services);

			services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
			services.AddTransient<INotificationRepository, NotificationRepository>();

			services.Configure<PushPackageOptions>(Configuration.GetSection("PushPackageOptions"));
			services.AddTransient<PushPackage, PushPackage>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
			});

			app.UseCors("CorsPolicy");
			ConfigureAuth(app);
			app.UseMvc();

			RegisterEventBusSubscriptions(app);
		}

		/// <param name="app"></param>
		protected virtual void ConfigureAuth(IApplicationBuilder app)
		{
			app.UseAuthentication();
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
					:	5;

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
				string subscriptionClientName = configSection["QueueName"];

				int retryCount = !string.IsNullOrEmpty(configSection["RetryCount"])
					? int.Parse(configSection["RetryCount"])
					: 5;

				return new EventBusRabbitMQ(rabbitMQPersistentConnection, eventBusSubcriptionsManager, brokerName, logger, sp, subscriptionClientName, retryCount);
			});

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
		}

		/// <param name="services"></param>
		private void RegisterEventBusHandlers(IServiceCollection services)
		{
			services.AddTransient<IIntegrationEventHandler<AddDeliveryIntegrationEvent>, AddDeliveryIntegrationEventHandler>();
		}

		/// <param name="app"></param>
		private void RegisterEventBusSubscriptions(IApplicationBuilder app)
		{
			if (Configuration["EventBus:Disabled"].ToLowerInvariant() != "false")
			{
				return;
			}

			IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

			eventBus.Subscribe<AddDeliveryIntegrationEvent, IIntegrationEventHandler<AddDeliveryIntegrationEvent>>();
		}
	}
}
