namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionMetadata
    {
        public string ShortName { get; }
        public string LongName { get; }
        public string Description { get; }

        public OptionMetadata(string shortName, string longName, string description)
        {
            ShortName = shortName;
            LongName = longName;
            Description = description;
        }
    }
}
