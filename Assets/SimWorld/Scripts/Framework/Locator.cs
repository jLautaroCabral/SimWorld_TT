using System;
using System.Collections.Generic;

namespace SimWorld
{
	/// <summary>
	/// We use this interface to register objects inside the locator, the locator implementation is intended for managers
	/// </summary>
	public interface ILocalizableManager
	{

	}

	/// <summary>
	/// Basic service locator implementation
	/// </summary>
	public static class Locator
	{
		private static readonly IDictionary<Type, object> Services = new Dictionary<Type, System.Object>();

		/// <summary>
		/// Register a locator element
		/// </summary>
		public static void Register<T>(T locatorElement) where T : ILocalizableManager
		{
			if (!Services.ContainsKey(typeof(T)))
			{
				Services[typeof(T)] = locatorElement;
			}
			else
			{
				throw new ApplicationException($"{nameof(T)}: Locator element already registered");
			}
		}

		/// <summary>
		/// Get a locator element
		/// </summary>
		public static T Resolve<T>() where T : ILocalizableManager
		{
			try
			{
				return (T)Services[typeof(T)];
			}
			catch
			{
				throw new ApplicationException($"{nameof(T)}: Locator element not found.");
			}
		}
	}
}
