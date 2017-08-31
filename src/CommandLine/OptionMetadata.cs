using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionMetadata
    {
        public PropertyInfo PropertyInfo { get; }
        public string ShortName { get; }
        public string LongName { get; }
        public string Description { get; }

        public OptionMetadata(PropertyInfo optionPropertyInfo, string shortName, string longName, string description)
        {
            PropertyInfo = optionPropertyInfo;
            ShortName = shortName;
            LongName = longName;
            Description = description;
        }
    }
}
