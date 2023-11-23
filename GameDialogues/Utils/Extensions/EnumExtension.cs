using System;
using System.Reflection;
using System.Linq;
using Marshtown.Attributes;

namespace Marshtown.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Converts enum value to String attribute value.
        /// <para>Method uses System.Reflection. Do not use in Update/FixedUpdate</para>
        /// </summary>
        /// <param name="e">Value to convert</param>
        /// <returns>String property value</returns>
        public static string ToStringValue(this Enum e)
        {
            var attribute = e.GetType()
                .GetTypeInfo()
                .GetMember(e.ToString())
                .FirstOrDefault(m => m.MemberType == MemberTypes.Field)?
                .GetCustomAttributes<StringValueAttribute>()
                .SingleOrDefault();

            return attribute != null ? attribute.StringValue : e.ToString();
        }

        /// <summary>
        /// Converts string to enum value of type <typeparamref name="T"/>. 
        /// <para>Method uses System.Reflection. Do not use in Update/FixedUpdate</para>
        /// </summary>
        /// <typeparam name="T">Enum to convert to</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Enum value of type <typeparamref name="T"/></returns>
        public static T ToEnumOrDefault<T>(this string value) where T : struct, IConvertible
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            var result = typeof(T).GetFields()
                .Where(info => info.GetCustomAttributes<StringValueAttribute>()
                    .Any(propertyValue => value.Equals(propertyValue.StringValue, StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault()?
                .GetValue(null);

            if (result == null)
            {
                if (Enum.TryParse(value.ToString(), true, out T resultValue))
                {
                    result = resultValue;
                }
            }

            return result == null ? default : (T)result;
        }
    }
}
