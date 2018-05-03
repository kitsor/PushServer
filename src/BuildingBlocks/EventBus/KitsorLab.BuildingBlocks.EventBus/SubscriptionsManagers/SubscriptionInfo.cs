namespace KitsorLab.BuildingBlocks.EventBus.SubscriptionsManagers
{
	using System;

	public class SubscriptionInfo
	{
		public bool IsDynamic { get; }
		public Type HandlerType { get; }

		/// <param name="isDynamic"></param>
		/// <param name="handlerType"></param>
		private SubscriptionInfo(bool isDynamic, Type handlerType)
		{
			IsDynamic = isDynamic;
			HandlerType = handlerType;
		}

		/// <param name="handlerType"></param>
		/// <returns></returns>
		public static SubscriptionInfo Dynamic(Type handlerType)
		{
			return new SubscriptionInfo(true, handlerType);
		}

		/// <param name="handlerType"></param>
		/// <returns></returns>
		public static SubscriptionInfo Typed(Type handlerType)
		{
			return new SubscriptionInfo(false, handlerType);
		}
	}
}
