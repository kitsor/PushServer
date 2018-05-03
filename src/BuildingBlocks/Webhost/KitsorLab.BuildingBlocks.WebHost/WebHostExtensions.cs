namespace KitsorLab.BuildingBlocks.WebHost
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Polly;
	using Polly.Retry;
	using System;
	using System.Data.SqlClient;

	public static class WebHostExtensions
	{
		public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost) where TContext : DbContext
		{
			using (var scope = webHost.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<TContext>>();
				var context = services.GetService<TContext>();

				try
				{
					logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

					RetryPolicy retry = Policy.Handle<SqlException>()
						.WaitAndRetry(new TimeSpan[]
						{
							TimeSpan.FromSeconds(5),
							TimeSpan.FromSeconds(10),
							TimeSpan.FromSeconds(15),
						});

					retry.Execute(() =>
					{
						context.Database.Migrate();
					});

					logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
				}
			}

			return webHost;
		}
	}
}
