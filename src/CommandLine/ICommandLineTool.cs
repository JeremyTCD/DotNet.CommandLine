using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandLineTool
    {
        ParseResult Run(string[] args, IEnumerable<Type> modelTypes, string appName, string appVersion);
    }
}
