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

        /// <summary>
        /// Prints <see cref="ParseException"/> and a get help tip if the specified <see cref="IParseResult"/> contains a <see cref="ParseException"/>.
        /// </summary>
        /// <param name="parseResult">The result from parsing command line arguments.</param>
        /// <param name="commandLineAppContext">The command line application's context.</param>
        /// <returns>Exit code.</returns>
        public int Run(IParseResult parseResult, ICommandLineAppContext commandLineAppContext)
        {
            commandLineAppContext.
                CommandLineAppPrinter.
                AppendHeader().
                AppendLine();

            // Handle parse exception
            if (parseResult.ParseException != null)
            {
                commandLineAppContext.
                    CommandLineAppPrinter.
                    AppendParseException(parseResult.ParseException).
                    AppendLine().
                    AppendGetHelpTip(IsDefault ? "this application" : "this command", IsDefault ? null : Name).
                    Print();

                return 0;
            }

            // Handle help requested
            if (Help)
            {
                if (IsDefault)
                {
                    commandLineAppContext.
                        CommandLineAppPrinter.
                        AppendAppHelp();
                }
                else
                {
                    commandLineAppContext.
                        CommandLineAppPrinter.
                        AppendCommandHelp(Name);
                }

                commandLineAppContext.
                    CommandLineAppPrinter.
                    Print();

                return 1;
            }

            commandLineAppContext.
                CommandLineAppPrinter.
                Print();

            return RunCommand(parseResult, commandLineAppContext);
        }

        /// <summary>
        /// Runs logic specific to the command. <see cref="Run(IParseResult, ICommandLineAppContext)"/> attempts to handle common scenarios,
        /// it calls this method if it is unable to handle the current scenario.
        /// </summary>
        /// <param name="parseResult">The result from parsing command line arguments.</param>
        /// <param name="commandLineAppContext">The command line application's context.</param>
        /// <returns>Exit code.</returns>
        public abstract int RunCommand(IParseResult parseResult, ICommandLineAppContext commandLineAppContext);
    }
}
