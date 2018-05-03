namespace KitsorLab.PushServer.BackgroudTasks
{
	using KitsorLab.BuildingBlocks.WebHost;
	using KitsorLab.PushServer.Infastructure;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;

	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args)
				.MigrateDbContext<PushServerDbContext>()
				.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
