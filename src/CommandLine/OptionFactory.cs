using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionFactory : IOptionFactory
    {
        /// <summary>
        /// Creates an <see cref="Option"/> instance from <paramref name="propertyInfo"/>'s <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// <see cref="PropertyInfo"/> instance with an <see cref="OptionAttribute"/>
        /// </param>
        /// <returns>
        /// <see cref="Option"/> if successful, null if <paramref name="propertyInfo"/> does not contain an <see cref="OptionAttribute"/>.
        /// </returns>
        public Option TryCreateFromPropertyInfo(PropertyInfo propertyInfo)
        {
            OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

            if (optionAttribute == null)
            {
                return null;
            }

            return new Option(propertyInfo, optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
