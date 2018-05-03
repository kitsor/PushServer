namespace KitsorLab.BuildingBlocks.EventBus.SubscriptionsManagers
{
	using KitsorLab.BuildingBlocks.EventBus.Events;
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
	{
		private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
		private readonly List<Type> _eventTypes;

		public event EventHandler<string> OnSubscriptionRemoved;

		public InMemoryEventBusSubscriptionsManager()
		{
			_handlers = new Dictionary<string, List<SubscriptionInfo>>();
			_eventTypes = new List<Type>();
		}

		public bool IsEmpty => !_handlers.Keys.Any();
		public void Clear() => _handlers.Clear();

		/// <typeparam name="TH"></typeparam>
		/// <param name="eventName"></param>
		public void AddDynamicSubscription<TH>(string eventName)
						where TH : IDynamicIntegrationEventHandler
		{
			DoAddSubscription(typeof(TH), eventName, isDynamic: true);
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TH"></typeparam>
		public void AddSubscription<T, TH>()
				where T : IntegrationEvent
				where TH : IIntegrationEventHandler<T>
		{
			string eventName = GetEventKey<T>();
			DoAddSubscription(typeof(TH), eventName, isDynamic: false);
			_eventTypes.Add(typeof(T));
		}

		/// <typeparam name="TH"></typeparam>
		/// <param name="eventName"></param>
		public void RemoveDynamicSubscription<TH>(string eventName)
				where TH : IDynamicIntegrationEventHandler
		{
			var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
			DoRemoveHandler(eventName, handlerToRemove);
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TH"></typeparam>
		public void RemoveSubscription<T, TH>()
				where TH : IIntegrationEventHandler<T>
				where T : IntegrationEvent
		{
			SubscriptionInfo handlerToRemove = FindSubscriptionToRemove<T, TH>();
			string eventName = GetEventKey<T>();
			DoRemoveHandler(eventName, handlerToRemove);
		}

		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
		{
			string key = GetEventKey<T>();
			return HasSubscriptionsForEvent(key);
		}

		/// <param name="eventName"></param>
		/// <returns></returns>
		public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

		/// <param name="eventName"></param>
		/// <returns></returns>
		public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
		{
			string key = GetEventKey<T>();
			return GetHandlersForEvent(key);
		}

		/// <param name="eventName"></param>
		/// <returns></returns>
		public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public string GetEventKey<T>() => typeof(T).Name;

		/// <param name="handlerType"></param>
		/// <param name="eventName"></param>
		/// <param name="isDynamic"></param>
		private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
		{
			if (!HasSubscriptionsForEvent(eventName))
			{
				_handlers.Add(eventName, new List<SubscriptionInfo>());
			}

			if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
			{
				throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
			}

			if (isDynamic)
			{
				_handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
			}
			else
			{
				_handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
			}
		}

		/// <param name="eventName"></param>
		/// <param name="subsToRemove"></param>
		private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
		{
			if (subsToRemove != null)
			{
				_handlers[eventName].Remove(subsToRemove);

				if (!_handlers[eventName].Any())
				{
					_handlers.Remove(eventName);
					Type eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

					if (eventType != null)
					{
						_eventTypes.Remove(eventType);
					}

					RaiseOnEventRemoved(eventName);
				}
			}
		}

		/// <param name="eventName"></param>
		private void RaiseOnEventRemoved(string eventName)
		{
			OnSubscriptionRemoved?.Invoke(this, eventName);
		}

		/// <typeparam name="TH"></typeparam>
		/// <param name="eventName"></param>
		/// <returns></returns>
		private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
				where TH : IDynamicIntegrationEventHandler
		{
			return DoFindSubscriptionToRemove(eventName, typeof(TH));
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TH"></typeparam>
		/// <returns></returns>
		private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
				 where T : IntegrationEvent
				 where TH : IIntegrationEventHandler<T>
		{
			string eventName = GetEventKey<T>();
			return DoFindSubscriptionToRemove(eventName, typeof(TH));
		}

		/// <param name="eventName"></param>
		/// <param name="handlerType"></param>
		/// <returns></returns>
		private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
		{
			if (!HasSubscriptionsForEvent(eventName))
			{
				return null;
			}

			return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
		}
	}
}
