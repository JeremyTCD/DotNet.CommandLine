using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionFactory : IOptionFactory
    {
        public Option CreateFromAttribute(OptionAttribute optionAttribute, PropertyInfo propertyInfo)
        {
            return new Option(propertyInfo, optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
