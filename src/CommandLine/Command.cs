using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsDefault { get; }

        public abstract int Run(ParseResult parseResult, AppContext appContext);
    }
}
