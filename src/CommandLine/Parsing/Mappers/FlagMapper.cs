using System;using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class FlagMapper : IMapper
    {
        /// <summary>
        /// If <paramref name="value"/> is null and the property represented by <paramref name="propertyInfo"/> is of type bool, 
        /// sets the property in <paramref name="target"/> to true. 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns>
        /// True if property was set succesfully, false otherwise.
        /// </returns>
        public bool TryMap(PropertyInfo propertyInfo, string value, object target)
        {
            if(value != null || propertyInfo.PropertyType != typeof(bool))
            {
                return false;
            }

            propertyInfo.SetValue(target, true);

            return true;
        }
    }
}
