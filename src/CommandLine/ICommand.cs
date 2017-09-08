namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        bool IsDefault { get; }

        int Run(ParseResult parseResult, IPrinter printer, AppContext context);
    }
}
