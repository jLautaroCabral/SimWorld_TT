using System;
using System.Globalization;
using UnityEngine;

namespace SimWorld
{
	/// <summary>
	/// This class is code I found somewhere
	/// </summary>
	public static class ArgumentValidator
	{
		/// <summary>
		/// Throw an ArgumentException if the string argument is null or empty.
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static bool ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
		{
			ArgumentNotNull(argumentValue, argumentName);

			if (argumentValue.Trim().Length != 0) return false;

			var errorMessage = string.Format(CultureInfo.CurrentCulture, "String cannot be empty", argumentName);
			Debug.LogError(errorMessage);
			//throw new ArgumentException(errorMessage);
			return true;
		}

		/// <summary>
		/// Throw an ArgumentNullException if the argument is null.
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		public static bool ArgumentNotNull(object argumentValue, string argumentName)
		{
			if (argumentValue != null) return false;

			Debug.LogError($"ArgumentNullException: {argumentName} cannot be null");
			//throw new ArgumentNullException(argumentName);
			return true;
		}

		/// <summary>
		/// Throw an ArgumentException if an Enum argument value is not defined by the specified Enum type.
		/// </summary>
		/// <param name="enumType">The Enum type the value should correspond to.</param>
		/// <param name="value">The value to check for.</param>
		/// <param name="argumentName">The name of the argument holding the value.</param>
		public static bool EnumValueIsDefined(Type enumType, object value, string argumentName)
		{
			if (Enum.IsDefined(enumType, value) != false) return false;

			var errorMessage = string.Format(CultureInfo.CurrentCulture, "Invalid enum value", argumentName,
				enumType.ToString());
			Debug.LogError(errorMessage);
			//throw new ArgumentException();
			return true;
		}

		/// <summary>
		/// Verifies that an argument type is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy).
		/// Throw an ArgumentException if not
		/// </summary>
		/// <param name="assignee">The argument type.</param>
		/// <param name="providedType">The type it must be assignable from.</param>
		/// <param name="argumentName">The argument name.</param>
		public static bool TypeIsAssignableFromType(Type assignee, Type providedType, string argumentName)
		{
			if (providedType.IsAssignableFrom(assignee)) return false;

			var errorMessage = string.Format(CultureInfo.CurrentCulture, "Type not compatible", assignee,
				providedType);
			Debug.LogError(errorMessage);
			//throw new ArgumentException(errorMessage, argumentName);
			return true;
		}
	}

	/// <summary>
	/// It is pretty generic but it will be useful in the future
	/// </summary>
	public abstract class ViewModelBase : MonoBehaviour
	{
		protected virtual void Awake()
		{
			Initialize();
		}

		public virtual void Initialize(params object[] viewModelArgs) { }

		private static void ValidateArgument<T>(object argumentToValidate, ref T outParameter, ref bool error)
		{
			error = ArgumentValidator.ArgumentNotNull(argumentToValidate, nameof(argumentToValidate));
			error = ArgumentValidator.TypeIsAssignableFromType(argumentToValidate.GetType(), typeof(T), nameof(argumentToValidate));
			outParameter = (T)argumentToValidate;
		}

		protected bool ValidateArguments<T1, T2>(object[] args, ref T1 arg1, ref T2 arg2)
		{
			var argumentsAreInvalid = false;
			if (args.Length == 0)
			{
				argumentsAreInvalid = true;
			}
			else
			{
				ValidateArgument(args[0], ref arg1, ref argumentsAreInvalid);
				ValidateArgument(args[1], ref arg2, ref argumentsAreInvalid);
			}
			return !argumentsAreInvalid;
		}

		protected bool ValidateArguments<T1, T2, T3>(object[] args, ref T1 arg1, ref T2 arg2, ref T3 arg3)
		{
			var argumentsAreInvalid = false;
			if (args.Length == 0)
			{
				argumentsAreInvalid = true;
			}
			else
			{
				ValidateArgument(args[0], ref arg1, ref argumentsAreInvalid);
				ValidateArgument(args[1], ref arg2, ref argumentsAreInvalid);
				ValidateArgument(args[2], ref arg3, ref argumentsAreInvalid);
			}
			return !argumentsAreInvalid;
		}

		protected bool ValidateArguments<T1, T2, T3, T4>(object[] args, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4)
		{
			var argumentsAreInvalid = false;
			if (args.Length == 0)
			{
				argumentsAreInvalid = true;
			}
			else
			{
				ValidateArgument(args[0], ref arg1, ref argumentsAreInvalid);
				ValidateArgument(args[1], ref arg2, ref argumentsAreInvalid);
				ValidateArgument(args[2], ref arg3, ref argumentsAreInvalid);
				ValidateArgument(args[3], ref arg4, ref argumentsAreInvalid);
			}
			return !argumentsAreInvalid;
		}
	}
}
