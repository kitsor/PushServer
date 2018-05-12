namespace KitsorLab.PushServer.BackgroudTasks.Queries
{
	using Dapper;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Threading.Tasks;

	public class DeliveryQueries : IDeliveryQueries
	{
		private string _connectionString = string.Empty;

		public DeliveryQueries(string connStr)
		{
			_connectionString = !string.IsNullOrWhiteSpace(connStr) 
				? connStr 
				: throw new ArgumentNullException(nameof(connStr));

		}

		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<DeliveryModel> GetDeliveryAsync(long key)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var result = await connection.QueryAsync<dynamic>(@"
					SELECT 
						d.[DeliveryKey] as DeliveryKey,
						d.[Status] as Status,

						n.[NotificationKey] as NotificationKey,
						n.[NotificationId] as NotificationId,
						n.[Title] as Title, 
						n.[Message] as Message,
						n.[Url] as Url,
						n.[IconUrl] as IconUrl,
						n.[ImageUrl] as ImageUrl,

						s.[SubscriptionKey] as SubscriptionKey,
						s.[Endpoint] as Endpoint,
						s.[PublicKey] as PublicKey,
						s.[Token] as Token,
						s.[DeviceToken] as DeviceToken

					FROM [pushServer].[Deliveries] d
          INNER JOIN [pushServer].[Notifications] n ON d.[NotificationKey] = n.[NotificationKey] 
          INNER JOIN [pushServer].[Subscriptions] s on d.[SubscriptionKey] = s.[SubscriptionKey]
          WHERE d.[DeliveryKey]=@key", 
					new { key = key });

				dynamic entry = result.FirstOrDefault();
				if (entry == null)
					throw new KeyNotFoundException();

				return new DeliveryModel(entry);
			}
		}

		/// <param name="maxItems"></param>
		/// <returns></returns>
		public async Task<IDictionary<long, SubscriptionType>> GetNewDeliveriesAndSetInQueue(int maxItems = 10)
		{
			IDictionary<long, SubscriptionType> retVal = new Dictionary<long, SubscriptionType>();

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = @"
					DECLARE @deliveryKeys table (DeliveryKey bigint);

					;WITH cte AS
					(
						SELECT TOP (@Top) d.*
						FROM [pushServer].[Deliveries] d WITH (ROWLOCK, READPAST)
						INNER JOIN [pushServer].[Subscriptions] s ON (d.[SubscriptionKey] = s.[SubscriptionKey])
						WHERE d.[Status] = @Status
						AND d.[ScheduledOn] <= GETDATE()
						AND d.[ScheduledOn] IS NOT NULL
						ORDER BY d.[ScheduledOn] ASC
					)
					UPDATE cte
					SET [Status] = @NewStatus
					OUTPUT inserted.[DeliveryKey] INTO @deliveryKeys;
				
					SELECT d.[DeliveryKey], s.[Type]
						FROM [pushServer].[Deliveries] d
						INNER JOIN [pushServer].[Subscriptions] s ON (d.[SubscriptionKey] = s.[SubscriptionKey])
						WHERE d.[DeliveryKey] IN (SELECT [DeliveryKey] FROM @deliveryKeys)
					";

				var result = await connection.QueryAsync<dynamic>(query,
					new
					{
						Top = maxItems,
						Status = (int)DeliveryStatus.New,
						NewStatus = (int)DeliveryStatus.InQueue,
					});


				foreach(dynamic delivery in result)
				{
					if (Enum.IsDefined(typeof(SubscriptionType), delivery.Type))
					{
						retVal.Add(delivery.DeliveryKey, (SubscriptionType)delivery.Type);
					}
				}
			}

			return retVal;
		}
	}
}
