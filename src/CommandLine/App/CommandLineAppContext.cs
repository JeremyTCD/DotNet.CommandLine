// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application's context.
    /// </summary>
    public class CommandLineAppContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineAppContext"/> class for a command line application.
        /// </summary>
        /// <param name="commandDictionary">The command line application's <see cref="CommandLine.CommandDictionary"/>.</param>
        /// <param name="commandLineAppOptions">The command line application's <see cref="CommandLine.CommandLineAppOptions"/>.</param>
        /// <param name="commandLineAppPrinter">The command line application's <see cref="ICommandLineAppPrinter"/>.</param>
        public CommandLineAppContext(CommandDictionary commandDictionary, CommandLineAppOptions commandLineAppOptions, ICommandLineAppPrinter commandLineAppPrinter)
        {
            CommandLineAppPrinter = commandLineAppPrinter;
            CommandDictionary = commandDictionary;
            CommandLineAppOptions = commandLineAppOptions;
        }

        /// <summary>
        /// Gets the command line application context's <see cref="CommandLine.CommandDictionary"/>.
        /// </summary>
        public CommandDictionary CommandDictionary { get; }

        /// <summary>
        /// Gets the command line application context's <see cref="CommandLine.CommandLineAppOptions"/>.
        /// </summary>
        public CommandLineAppOptions CommandLineAppOptions { get; }

        /// <summary>
        /// Gets the command line application context's <see cref="ICommandLineAppPrinter"/>.
        /// </summary>
        public ICommandLineAppPrinter CommandLineAppPrinter { get; }
    }
}
