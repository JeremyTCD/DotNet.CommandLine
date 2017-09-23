// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a default <see cref="ICommand"/>.
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public abstract string Description { get; }

        /// <inheritdoc/>
        public abstract bool IsDefault { get; }

        /// <summary>
        /// Gets or sets a value indicating whether help should be printed by the command. True if help should be printed; otherwise, false.
        /// </summary>
        [Option(typeof(Strings), nameof(Strings.OptionShortName_Help), nameof(Strings.OptionLongName_Help), nameof(Strings.OptionDescription_Help))]
        public bool Help { get; set; }

        /// <inheritdoc/>
        /// <summary>
        /// Contains logic for handling common scenarios:
        /// If <paramref name="parseResult"/> contains a <see cref="ParseException"/>, prints exception and a get help tip before returning 0.
        /// Otherwise, if <see cref="Help"/> is true, prints help and returns 1.
        /// <para/>
        /// If the current scenario is not one of the above scenarios, calls <see cref="RunCommand(ParseResult, ICommandLineAppContext)"/>.
        /// </summary>
        public int Run(ParseResult parseResult, ICommandLineAppContext appContext)
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

        /// <summary>
        /// Runs logic specific to the command. <see cref="Run(ParseResult, ICommandLineAppContext)"/> attempts to handle common scenarios,
        /// it calls this method if it is unable to handle the current scenario.
        /// </summary>
        /// <param name="parseResult">Result of parsing command line arguments.</param>
        /// <param name="commandLineAppContext">Context of executing command line application.</param>
        /// <returns>Exit code.</returns>
        public abstract int RunCommand(ParseResult parseResult, ICommandLineAppContext commandLineAppContext);
    }
}
