using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandSetFactory
    {
        CommandSet CreateFromCommands(IEnumerable<ICommand> commands);
    }
}
