using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionMetadataFactory : IOptionMetadataFactory
    {
        public OptionMetadata CreateFromAttribute(OptionAttribute optionAttribute, PropertyInfo propertyInfo)
        {
            return new OptionMetadata(propertyInfo, optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
