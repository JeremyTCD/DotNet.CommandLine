using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class FlagMapper : IMapper
    {
        /// <summary>
        /// If <paramref name="value"/> is null and the property represented by <paramref name="propertyInfo"/> is of type bool, 
        /// sets the corresponding property in <paramref name="command"/> to true. 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <param name="command"></param>
        /// <returns>
        /// True if property was set succesfully, false otherwise.
        /// </returns>
        public bool TryMap(PropertyInfo propertyInfo, string value, ICommand command)
        {
            if(value != null || propertyInfo.PropertyType != typeof(bool))
            {
                return false;
            }

            propertyInfo.SetValue(command, true);

            return true;
        }
    }
}
