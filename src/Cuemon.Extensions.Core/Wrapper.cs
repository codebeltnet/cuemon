using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Provides helper method for a <see cref="Wrapper{T}"/> object.
    /// </summary>
    public static class Wrapper
    {
        /// <summary>
        /// Parses the encapsulated instance of the specified <paramref name="wrapper"/> for a human-readable string value.
        /// </summary>
        /// <typeparam name="T">The type of the encapsulated instance of <paramref name="wrapper"/>.</typeparam>
        /// <param name="wrapper">The wrapper object to parse the instance.</param>
        /// <returns>A human-readable <see cref="string"/> representation of the wrapped instance in the <see cref="Wrapper{T}"/> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="wrapper"/> is null.
        /// </exception>
        public static string ParseInstance<T>(IWrapper<T> wrapper)
        {
            Validator.ThrowIfNull(wrapper);
            switch (Type.GetTypeCode(wrapper.InstanceType))
            {
                case TypeCode.Boolean:
                    return wrapper.Instance.ToString().ToLowerInvariant();
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Double:
                    return wrapper.InstanceAs<IConvertible>().ToString(CultureInfo.InvariantCulture);
                case TypeCode.DateTime:
                    return wrapper.InstanceAs<DateTime>().ToString("O", CultureInfo.InvariantCulture);
                case TypeCode.String:
                    return wrapper.Instance.ToString();
                default:
                    if (Decorator.Enclose(wrapper.InstanceType).HasKeyValuePairImplementation())
                    {
                        var keyProperty = wrapper.InstanceType.GetProperty("Key");
                        var valueProperty = wrapper.InstanceType.GetProperty("Value");
                        var keyValue = keyProperty.GetValue(wrapper.Instance, null) ?? "null";
                        var valueValue = valueProperty.GetValue(wrapper.Instance, null) ?? "null";
                        return string.Format(CultureInfo.InvariantCulture, "[{0},{1}]", keyValue, valueValue);
                    }

                    if (Decorator.Enclose(wrapper.InstanceType).HasComparerImplementation() || Decorator.Enclose(wrapper.InstanceType).HasEqualityComparerImplementation())
                    {
                        return Decorator.Enclose(wrapper.InstanceType).ToFriendlyName();
                    }

                    switch (wrapper.InstanceType.Name.ToUpperInvariant())
                    {
                        case "BYTE[]":
                            return Convert.ToBase64String(wrapper.InstanceAs<byte[]>());
                        case "GUID":
                            return wrapper.InstanceAs<Guid>(CultureInfo.InvariantCulture).ToString("D");
                        case "RUNTIMETYPE":
                            return Decorator.Enclose(wrapper.InstanceAs<Type>()).ToFriendlyName();
                        case "URI":
                            return wrapper.InstanceAs<Uri>().OriginalString;
                        default:
                            return wrapper.Instance.ToString();
                    }
            }
        }
    }

    /// <summary>
    /// Provides a way to wrap an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to wrap.</typeparam>
    public class Wrapper<T> : IWrapper<T>
    {
        private T _instance;
        private Type _instanceType;
        private MemberInfo _memberReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="Wrapper{T}"/> class.
        /// </summary>
        protected Wrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wrapper{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance that this wrapper object represents.</param>
        /// <param name="memberReference">The member from where <paramref name="instance"/> was referenced.</param>
        public Wrapper(T instance, MemberInfo memberReference = null)
        {
            Validator.ThrowIfNull(instance);
            _instance = instance;
            _instanceType = instance.GetType();
            _memberReference = memberReference;
        }

        /// <summary>
        /// Gets the object that this wrapper represents.
        /// </summary>
        /// <value>The object that this wrapper represents.</value>
        public virtual T Instance
        {
            get => _instance;
            protected set => _instance = value;
        }

        /// <summary>
        /// Gets the type of the object that this wrapper represents.
        /// </summary>
        /// <value>The type of the that this wrapper represents.</value>
        public virtual Type InstanceType
        {
            get => _instanceType;
            protected set => _instanceType = value;
        }

        /// <summary>
        /// Gets the member from where <see cref="Instance"/> was referenced.
        /// </summary>
        /// <value>The member from where <see cref="Instance"/> was referenced.</value>
        public virtual MemberInfo MemberReference
        {
            get => _memberReference;
            protected set => _memberReference = value;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a member reference.
        /// </summary>
        /// <value><c>true</c> if this instance has a member reference; otherwise, <c>false</c>.</value>
        public virtual bool HasMemberReference => (_memberReference != null);

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about this wrapper object.
        /// </summary>
        /// <value>An object that implements the <see cref="IDictionary{TKey,TValue}"/> interface and contains a collection of user-defined key/value pairs.</value>
        public virtual IDictionary<string, object> Data { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Returns a value that is equivalent to the instance of the object that this wrapper represents.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <returns>A value that is equivalent to the instance of the object that this wrapper represents.</returns>
        /// <exception cref="InvalidCastException">
        /// The conversion is not supported - or - <see cref="Instance"/> does not implement the <see cref="IConvertible"/> interface.
        /// </exception>
        /// <exception cref="FormatException">
        /// <see cref="Instance"/> is not in a format for <typeparamref name="T"/> recognized by <see cref="CultureInfo.InvariantCulture"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        /// <see cref="Instance"/> represents a number that is out of the range of <typeparamref name="T"/>.
        /// </exception>
        public TResult InstanceAs<TResult>()
        {
            return InstanceAs<TResult>(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a value that is equivalent to the instance of the object that this wrapper represents.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A value that is equivalent to the instance of the object that this wrapper represents.</returns>
        /// <exception cref="InvalidCastException">
        /// The conversion is not supported - or - <see cref="Instance"/> does not implement the <see cref="IConvertible"/> interface.
        /// </exception>
        /// <exception cref="FormatException">
        /// <see cref="Instance"/> is not in a format for <typeparamref name="T"/> recognized by <paramref name="provider"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        /// <see cref="Instance"/> represents a number that is out of the range of <typeparamref name="T"/>.
        /// </exception>
        public TResult InstanceAs<TResult>(IFormatProvider provider)
        {
            var presult = Decorator.Enclose<object>(Instance).ChangeType(InstanceType, o => o.FormatProvider = provider);
            return Decorator.Enclose(presult).ChangeTypeOrDefault<TResult>();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Wrapper.ParseInstance(this);
        }
    }
}
