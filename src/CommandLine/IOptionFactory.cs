using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOptionFactory
    {
        Option CreateFromAttribute(OptionAttribute optionAttribute, PropertyInfo propertyInfo);
    }
}