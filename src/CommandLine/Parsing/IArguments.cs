using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IArguments
    {
        string CommandName { get; }

        Dictionary<string, string> OptionArgs { get; }
    }
}