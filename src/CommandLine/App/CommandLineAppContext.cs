// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class CommandLineAppContext : ICommandLineAppContext
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

        /// <inheritdoc/>
        public CommandDictionary CommandDictionary { get; }

        /// <inheritdoc/>
        public CommandLineAppOptions CommandLineAppOptions { get; }

        /// <inheritdoc/>
        public ICommandLineAppPrinter CommandLineAppPrinter { get; }
    }
}
