namespace FunctionalTests.Services.PushServer
{
	using KitsorLab.BuildingBlocks.WebHost;
	using KitsorLab.PushServer.Infastructure;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.TestHost;
	using System.IO;

	public class PushServerTestsBase
	{
		public TestServer CreateServer()
		{
			var webHostBuilder = WebHost.CreateDefaultBuilder();
			webHostBuilder.UseContentRoot("..\\..\\..\\Services\\PushServer");
			webHostBuilder.UseStartup<PushServerTestsStartup>();

			var testServer = new TestServer(webHostBuilder);

			testServer.Host.MigrateDbContext<PushServerDbContext>();

			return testServer;
		}

		public static class Post
		{
			public static string Subscriptions = "/subscriptions";
			public static string Notifications = "/notifications";
		}

		public static class Delete
		{
			public static string Subscriptions = "/subscriptions/uid/{userId}";
		}
	}
}
