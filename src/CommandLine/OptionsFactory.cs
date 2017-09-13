using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionsFactory : IOptionsFactory
    {
        private readonly IDictionary<ICommand, List<Option>> _optionsCache = new Dictionary<ICommand, List<Option>>();

        /// <summary>
        /// Creates a <see cref="List{T}"/> of <see cref="Option"/> instances from properties in <paramref name="command"/> that have an
        /// <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// <see cref="List{Option}"/>
        /// </returns>
        public List<Option> CreateFromCommand(ICommand command)
        {
            _optionsCache.TryGetValue(command, out List<Option> options);

            if (options == null)
            {
                options = new List<Option>();
                foreach (PropertyInfo propertyInfo in command.GetType().GetProperties())
                {
                    Option option = TryCreateFromPropertyInfo(propertyInfo);
                    if (option != null)
                    {
                        options.Add(option);
                    }
                }
            }
            return options;
        }

        /// <summary>
        /// Creates an <see cref="Option"/> instance from <paramref name="propertyInfo"/>'s <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// <see cref="PropertyInfo"/> instance with an <see cref="OptionAttribute"/>
        /// </param>
        /// <returns>
        /// <see cref="Option"/> if successful, null if <paramref name="propertyInfo"/> does not contain an <see cref="OptionAttribute"/>.
        /// </returns>
        internal virtual Option TryCreateFromPropertyInfo(PropertyInfo propertyInfo)
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
