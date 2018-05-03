namespace FunctionalTests.Services.PushServer
{
	using KitsorLab.PushServer.API.Model;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using NUnit.Framework;
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;

	[TestFixture]
	public class PushServerTests
	{
		private static Random _rng = new Random();

		[Test]
		public async Task CreateDeleteSubscription()
		{
			using (var pushServer = new PushServerTestsBase().CreateServer())
			{
				var client = pushServer.CreateClient();
				client.DefaultRequestHeaders.Add("X-Forwarded-For", $"192.168.1.{_rng.Next(1, 255)}");

				StringContent contentSubscription;
				HttpResponseMessage result;
				string content;
				bool isSuccess;
				dynamic response;
				string userId;

				// W3C Subscription
				contentSubscription = new StringContent(BuildSubscriptionRequest(), Encoding.UTF8, "application/json");
				result = await client.PostAsync(PushServerTestsBase.Post.Subscriptions, contentSubscription);
				content = await result.Content.ReadAsStringAsync();
				Assert.True(result.IsSuccessStatusCode, "Unsuccessful request. Response: {0}", content);
				response = JObject.Parse(content);
				isSuccess  = response.isSuccess;
				userId = response.data.userId.ToString();
				Assert.True(isSuccess);
				Assert.IsNotEmpty(userId);

				result = await client.DeleteAsync(PushServerTestsBase.Delete.Subscriptions.Replace("{userId}", userId));
				Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

				// Safari subscription
				contentSubscription = new StringContent(BuildSubscriptionRequest(false), Encoding.UTF8, "application/json");
				result = await client.PostAsync(PushServerTestsBase.Post.Subscriptions, contentSubscription);
				content = await result.Content.ReadAsStringAsync();
				Assert.True(result.IsSuccessStatusCode, "Unsuccessful request. Response: {0}", content);
				response = JObject.Parse(content);
				isSuccess = response.isSuccess;
				userId = response.data.userId.ToString();
				Assert.True(isSuccess);
				Assert.IsNotEmpty(userId);

				//result = await client.DeleteAsync(PushServerTestsBase.Delete.Subscriptions.Replace("{userId}", userId));
				//Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
			}
		}

		[Test]
		public async Task CreateNotification()
		{
			using (var pushServer = new PushServerTestsBase().CreateServer())
			{
				var client = pushServer.CreateClient();

				var contentNotification = new StringContent(BuildNotificationRequest(), Encoding.UTF8, "application/json");
				var result = await client.PostAsync(PushServerTestsBase.Post.Notifications, contentNotification);
				var content = await result.Content.ReadAsStringAsync();
				dynamic response = JObject.Parse(content);
				bool isSuccess = response.isSuccess;
				Assert.True(isSuccess);
			}
		}

		private string BuildSubscriptionRequest(bool w3c = true)
		{
			SubscriptionRequest subscription = null;

			if (w3c)
			{
				subscription = new SubscriptionRequest
				{
					Endpoint = "https://fcm.googleapis.com/fcm/send/" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10) + "5o:APA91bHRw1CPYqYd_Acf3Byv_Fuw9roroRd-vdvlbchX5chmvUqKL3kUaQmlqr4prmBfsRX-1zjXabDy0BiCeWsdHqOm1IHYaEMR1_549SmKe3I4HRNmYXSvVAK29-qrPk8rnQ8BhWGt",
					PublicKey = "BCgyRmvbhJP2l5ROO59e6xCTDuXHa3IuLpZuSCgd8iAwD2M/o7FxZ7qkDdug3mlZMqGZr+F6U/pjua+Yxwh7YiU=",
					Auth = "Gc031fTOObax53o6cCOZkw==",
				};
			}
			else
			{
				subscription = new SubscriptionRequest
				{
					DeviceToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
				};
			}

			return JsonConvert.SerializeObject(subscription);
		}

		private string BuildNotificationRequest()
		{
			var notification = new NotificationRequest
			{
				Title = "Hello World!",
				Message = "Some message: " + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10),
				IconUrl = "/images/pic1.png",
				Url = "https://www.microsoft.com",
			};

			return JsonConvert.SerializeObject(notification);
		}
	}
}
