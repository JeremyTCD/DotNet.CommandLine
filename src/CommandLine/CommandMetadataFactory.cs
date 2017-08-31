using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandMetadataFactory : ICommandMetadataFactory
    {
        private IOptionMetadataFactory _optionMetadataFactory { get; }

        public CommandMetadataFactory(IOptionMetadataFactory optionMetadataFactory)
        {
            _optionMetadataFactory = optionMetadataFactory;
        }

        public CommandMetadata CreateFromAttribute(CommandAttribute commandAttribute)
        {
            List<OptionMetadata> optionMetadata = new List<OptionMetadata>();
            foreach(PropertyInfo propertyInfo in commandAttribute.CommandModelType.GetRuntimeProperties())
            {
                OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

                if(optionAttribute != null)
                {
                    optionMetadata.Add(_optionMetadataFactory.CreateFromAttribute(optionAttribute));
                }
            }

            return new CommandMetadata(commandAttribute.CommandModelType, commandAttribute.IsDefault, 
                commandAttribute.Name, commandAttribute.Description, optionMetadata);
        }
    }
}
