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

        [Option(typeof(Strings), nameof(Strings.OptionShortName_Help), nameof(Strings.OptionLongName_Help), nameof(Strings.OptionDescription_Help))]
        public bool Help { get; }

        // TODO no choice, have to inject printer, if constructor requires it itll be confusing for implementors
        public int Run(ParseResult parseResult, AppContext appContext)
        {


            return 0;
        }
    }
}
