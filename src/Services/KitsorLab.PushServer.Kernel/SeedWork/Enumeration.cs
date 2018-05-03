namespace KitsorLab.PushServer.Kernel.SeedWork
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public abstract class Enumeration : IComparable
	{
		public string Name { get; private set; }

		public int Id { get; private set; }

		protected Enumeration() { }

		/// <param name="id"></param>
		/// <param name="name"></param>
		protected Enumeration(int id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <returns></returns>
		public override string ToString() => Name;

		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
		{
			var type = typeof(T);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (var info in fields)
			{
				var instance = new T();
				var locatedValue = info.GetValue(instance) as T;

				if (locatedValue != null)
					yield return locatedValue;
			}
		}

		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			var otherValue = obj as Enumeration;

			if (otherValue == null)
				return false;

			var typeMatches = GetType().Equals(obj.GetType());
			var valueMatches = Id.Equals(otherValue.Id);

			return typeMatches && valueMatches;
		}

		/// <returns></returns>
		public override int GetHashCode() => Id.GetHashCode();

		/// <param name="firstValue"></param>
		/// <param name="secondValue"></param>
		/// <returns></returns>
		public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
		{
			var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
			return absoluteDifference;
		}

		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T FromValue<T>(int value) where T : Enumeration, new()
		{
			var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
			return matchingItem;
		}

		/// <typeparam name="T"></typeparam>
		/// <param name="displayName"></param>
		/// <returns></returns>
		public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
		{
			var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
			return matchingItem;
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="value"></param>
		/// <param name="description"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
		{
			var matchingItem = GetAll<T>().FirstOrDefault(predicate);

			if (matchingItem == null)
				throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

			return matchingItem;
		}

		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
	}
}
