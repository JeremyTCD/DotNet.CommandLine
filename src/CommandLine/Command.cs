namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class Command
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsDefault;

        public Command(string name, string description, bool isDefault)
        {
            Name = name;
            Description = description;
            IsDefault = isDefault;
        }

        public abstract int Run(ParseResult parseResult, IPrinter printer);
    }
}
