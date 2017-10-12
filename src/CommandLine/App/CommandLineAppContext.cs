// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class CommandLineAppContext : ICommandLineAppContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineAppContext"/> class.
        /// </summary>
        /// <param name="commandDictionary">The command line application's <see cref="ICommandDictionary"/>.</param>
        /// <param name="commandLineAppOptions">The command line application's <see cref="CommandLine.CommandLineAppOptions"/>.</param>
        /// <param name="commandLineAppPrinter">The command line application's <see cref="ICommandLineAppPrinter"/>.</param>
        public CommandLineAppContext(ICommandDictionary commandDictionary, CommandLineAppOptions commandLineAppOptions, ICommandLineAppPrinter commandLineAppPrinter)
        {
            CommandLineAppPrinter = commandLineAppPrinter;
            CommandDictionary = commandDictionary;
            CommandLineAppOptions = commandLineAppOptions;
        }

        /// <inheritdoc/>
        public virtual ICommandDictionary CommandDictionary { get; }

        /// <inheritdoc/>
        public virtual CommandLineAppOptions CommandLineAppOptions { get; }

        /// <inheritdoc/>
        public virtual ICommandLineAppPrinter CommandLineAppPrinter { get; }
    }
}
