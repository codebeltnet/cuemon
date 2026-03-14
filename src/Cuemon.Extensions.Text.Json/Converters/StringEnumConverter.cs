using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converter to convert enums to and from strings.
    /// </summary>
    public class StringEnumConverter : JsonConverterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEnumConverter"/> class.
        /// </summary>
        public StringEnumConverter()
        {
        }

        /// <summary>
        /// Determines whether the type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type is checked whether it can be converted.</param>
        /// <returns><c>true</c> if the type can be converted, <c>false</c> otherwise.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum && !typeToConvert.IsDefined(typeof(FlagsAttribute), false);
        }

        /// <summary>
        /// Creates a converter for a specified <paramref name="typeToConvert"/>.
        /// </summary>
        /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
        /// <returns>
        /// An instance of a <see cref="JsonConverter{T}"/> where T is compatible with <paramref name="typeToConvert"/>.
        /// If <see langword="null"/> is returned, a <see cref="NotSupportedException"/> will be thrown.
        /// </returns>
        /// <remarks>This is an insane reflection implementation due to, IMO, odd design choice by Microsoft (why have separate namingPolicy when PropertyNamingPolicy is already part of options). Personally I like the NamingStrategy found in Newtonsoft.Json better.</remarks>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var enumConverterOptions = typeof(JsonConverterFactory).Assembly.GetType("System.Text.Json.Serialization.Converters.EnumConverterOptions");
            var enumConverterFactory = typeof(JsonConverterFactory).Assembly.GetType("System.Text.Json.Serialization.Converters.EnumConverterFactory");
            if (enumConverterOptions != null && enumConverterFactory != null)
            {
#if NET10_0_OR_GREATER
                var createMethod = enumConverterFactory.GetMethod("Create", MemberReflection.Everything, new[] { typeof(Type), enumConverterOptions, typeof(JsonNamingPolicy), typeof(JsonSerializerOptions) });
                if (createMethod != null)
                {
                    return (JsonConverter)createMethod.Invoke(null, new object[] { typeToConvert, 1, options.PropertyNamingPolicy, options });
                }
#else
                var createMethod = enumConverterFactory.GetMethod("Create", MemberReflection.Everything, null, new[] { typeof(Type), enumConverterOptions, typeof(JsonNamingPolicy), typeof(JsonSerializerOptions) }, null);
                if (createMethod != null)
                {
                    return (JsonConverter)createMethod.Invoke(null, new object[] { typeToConvert, 1, options.PropertyNamingPolicy, options });
                }
#endif
            }
            throw new NotSupportedException("Unable to locate internal members required by this method.");
        }
    }
}
