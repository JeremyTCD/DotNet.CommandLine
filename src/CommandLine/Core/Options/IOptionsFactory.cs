using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOptionsFactory
    {
        List<Option> CreateFromCommand(ICommand command);
    }
}