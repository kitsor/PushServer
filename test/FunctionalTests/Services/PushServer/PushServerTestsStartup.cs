namespace FunctionalTests.Services.PushServer
{
	using FunctionalTests.Middleware;
	using KitsorLab.PushServer.API;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using System.IO;

	class PushServerTestsStartup : Startup
	{
		public PushServerTestsStartup(IHostingEnvironment env) : base(env)
		{
		}

		protected override IConfiguration CreateConfiguration(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder();

			builder.SetBasePath(Path.Combine(
				new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
				"Services",
				"PushServer"))
				.AddJsonFile("appsettings.json");

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			return builder.Build();
		}


		protected override void ConfigureAuth(IApplicationBuilder app)
		{
			if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
			{
				app.UseMiddleware<AutoAuthorizeMiddleware>();
			}
			else
			{
				base.ConfigureAuth(app);
			}
		}

	}
}
