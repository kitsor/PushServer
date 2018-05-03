namespace KitsorLab.PushServer.Kernel.SeedWork
{
	using MediatR;
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public abstract class Entity
	{
		protected int? _requestedHashCode;

		public abstract long PrimaryKey
		{
			get;
		}

		private List<INotification> _domainEvents;
		public List<INotification> DomainEvents => _domainEvents;

		/// <param name="eventItem"></param>
		public void AddDomainEvent(INotification eventItem)
		{
			_domainEvents = _domainEvents ?? new List<INotification>();
			_domainEvents.Add(eventItem);
		}

		/// <param name="eventItem"></param>
		public void RemoveDomainEvent(INotification eventItem)
		{
			if (_domainEvents is null) return;
			_domainEvents.Remove(eventItem);
		}

		/// <returns></returns>
		public bool IsTransient()
		{
			return PrimaryKey == default(Int64);
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Entity))
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			if (GetType() != obj.GetType())
				return false;

			Entity item = (Entity)obj;

			if (item.IsTransient() || IsTransient())
				return false;
			else
				return item.PrimaryKey == PrimaryKey;
		}

		/// <returns></returns>
		public override int GetHashCode()
		{
			if (!IsTransient())
			{
				if (!_requestedHashCode.HasValue)
					_requestedHashCode = PrimaryKey.GetHashCode() ^ 31; 
						// XOR for random distribution 
						// (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

				return _requestedHashCode.Value;
			}

			return base.GetHashCode();
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(Entity left, Entity right)
		{
			if (Object.Equals(left, null))
			{
				return (Object.Equals(right, null)) ? true : false;
			}

			return left.Equals(right);
		}

		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}
	}
}
