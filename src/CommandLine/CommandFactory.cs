using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandFactory : ICommandFactory
    {
        private IOptionFactory _optionFactory { get; }

        /// <summary>
        /// Creates a <see cref="CommandFactory"/> instance.
        /// </summary>
        /// <param name="optionFactory"></param>
        public CommandFactory(IOptionFactory optionFactory)
        {
            _optionFactory = optionFactory;
        }

        /// <summary>
        /// Creates a <see cref="Command"/> instance from <paramref name="modelType"/>'s <see cref="CommandAttribute"/>.
        /// </summary>
        /// <param name="modelType">
        /// <see cref="Type"/> instance with a <see cref="CommandAttribute"/>
        /// </param>
        /// <returns>
        /// <see cref="Command"/> if successful, null if <paramref name="modelType"/> does not contain a <see cref="CommandAttribute"/>.
        /// </returns>
        public Command TryCreateFromType(Type modelType)
        {
            CommandAttribute commandAttribute = modelType.GetTypeInfo().GetCustomAttribute<CommandAttribute>();

            if (commandAttribute == null)
            {
                return null;
            }

            List<Option> options = new List<Option>();
            foreach (PropertyInfo propertyInfo in modelType.GetRuntimeProperties())
            {
                Option option = _optionFactory.TryCreateFromPropertyInfo(propertyInfo);
                if (option != null)
                {
                    options.Add(option);
                }
            }

            return new Command(modelType, commandAttribute.IsDefault,
                commandAttribute.Name, commandAttribute.Description, options);
        }
    }
}
