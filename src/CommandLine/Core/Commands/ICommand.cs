using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        bool IsDefault { get; }

        int Run(ParseResult parseResult, AppContext appContext);
    }
}
