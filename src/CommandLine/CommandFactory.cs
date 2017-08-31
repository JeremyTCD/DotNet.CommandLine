using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandFactory : ICommandFactory
    {
        private IOptionFactory _optionFactory { get; }

        public CommandFactory(IOptionFactory optionFactory)
        {
            _optionFactory = optionFactory;
        }

        public Command CreateFromAttribute(CommandAttribute commandAttribute, Type commandModelType)
        {
            List<Option> options = new List<Option>();
            foreach(PropertyInfo propertyInfo in commandModelType.GetRuntimeProperties())
            {
                OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

                if(optionAttribute != null)
                {
                    options.Add(_optionFactory.CreateFromAttribute(optionAttribute, propertyInfo));
                }
            }

            return new Command(commandModelType, commandAttribute.IsDefault, 
                commandAttribute.Name, commandAttribute.Description, options);
        }
    }
}
