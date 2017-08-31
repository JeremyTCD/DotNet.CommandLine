using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Option
    {
        public PropertyInfo PropertyInfo { get; }
        public string ShortName { get; }
        public string LongName { get; }
        public string Description { get; }

        public Option(PropertyInfo propertyInfo, string shortName, string longName, string description)
        {
            PropertyInfo = propertyInfo;
            ShortName = shortName;
            LongName = longName;
            Description = description;
        }
    }
}
