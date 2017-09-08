using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class StringConvertibleMapper : IMapper
    {
        /// <summary>
        /// If <paramref name="value"/> is not null and the property represented by <paramref name="propertyInfo"/> is of a type that 
        /// can be converted to from <see cref="string"/>, sets corresponding property in <paramref name="command"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <param name="command"></param>
        /// <returns>
        /// True if property was set succesfully, false otherwise.
        /// </returns>
        public bool TryMap(PropertyInfo propertyInfo, string value, Command command)
        {
            if(value == null || !CanBeConvertedToFromString(propertyInfo.PropertyType))
            {
                return false;
            }

            propertyInfo.SetValue(command, Convert.ChangeType(value, propertyInfo.PropertyType));
            return true;
        }

        /// <summary>
        /// Checks if <paramref name="type"/> can be converted to from <see cref="string"/>. All types that <see cref="string"/> has
        /// an <see cref="IConvertible"/> member implementation for are included 
        /// (https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/String.cs).
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// True if <paramref name="type"/> can be converted to from <see cref="string"/>.
        /// </returns>
        internal bool CanBeConvertedToFromString(Type type)
        {
            return type.GetTypeInfo().IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
        }
    }
}
