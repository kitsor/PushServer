namespace KitsorLab.WebApp.WebPushAdmin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using KitsorLab.WebApp.WebPushAdmin.Controllers;
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient;
	using KitsorLab.WebApp.WebPushAdmin.Services;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<PushServerClientSettings>(Configuration.GetSection("PushServerClientSettings"));

			services.AddTransient<ISubscriptionService, SubscriptionService>();
			services.AddTransient<INotificationService, NotificationService>();

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(SubscriptionsController.ROUTE_INDEX, SubscriptionsController.ROUTE_INDEX,
					new { controller = "Subscriptions", action = nameof(SubscriptionsController.Index), routeName = SubscriptionsController.ROUTE_INDEX });

				routes.MapRoute(NotificationsController.ROUTE_INDEX, NotificationsController.ROUTE_INDEX,
					new { controller = "Notifications", action = nameof(NotificationsController.Index), routeName = NotificationsController.ROUTE_INDEX });

				routes.MapRoute(
									name: "default",
									template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
