using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IArgumentAccessor
    {
        string CommandName { get; }

        Dictionary<string, string> OptionArgs { get; }
    }
}