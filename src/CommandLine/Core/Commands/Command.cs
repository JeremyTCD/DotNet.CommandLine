// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract bool IsDefault { get; }

        [Option(typeof(Strings), nameof(Strings.OptionShortName_Help), nameof(Strings.OptionLongName_Help), nameof(Strings.OptionDescription_Help))]
        public bool Help { get; set; }

        /// <summary>
        /// If <paramref name="parseResult"/> contains a <see cref="ParseException"/> instance, prints exception and a get help tip before returning 0.
        /// If <see cref="Help"/> is true, prints help and returns 1. Otherwise, calls <see cref="RunCommand(ParseResult, CommandLineAppContext)"/>.
        /// </summary>
        /// <param name="parseResult"></param>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public virtual int Run(ParseResult parseResult, CommandLineAppContext appContext)
        {
            appContext.
                CommandLineAppPrinter.
                AppendHeader().
                AppendLine();

            if (parseResult.ParseException != null)
            {
                appContext.
                    CommandLineAppPrinter.
                    AppendParseException(parseResult.ParseException).
                    AppendLine().
                    AppendGetHelpTip(IsDefault ? "this application" : "this command", IsDefault ? null : Name).
                    Print();

                return 0;
            }

            if (Help)
            {
                if (IsDefault)
                {
                    appContext.
                        CommandLineAppPrinter.
                        AppendAppHelp();
                }
                else
                {
                    appContext.
                        CommandLineAppPrinter.
                        AppendCommandHelp(Name);
                }

                appContext.
                    CommandLineAppPrinter.
                    Print();

                return 1;
            }

            appContext.
                CommandLineAppPrinter.
                Print();

            return RunCommand(parseResult, appContext);
        }

        public abstract int RunCommand(ParseResult parseResult, CommandLineAppContext appContext);
    }
}
