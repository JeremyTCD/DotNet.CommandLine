using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOptionMetadataFactory
    {
        OptionMetadata CreateFromAttribute(OptionAttribute optionAttribute, PropertyInfo propertyInfo);
    }
}