// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a default <see cref="ICommandLineAppContextFactory"/>.
    /// </summary>
    public class CommandLineAppContextFactory : ICommandLineAppContextFactory
    {
        private ICommandLineAppPrinterFactory _appPrinterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineAppContextFactory"/> class.
        /// </summary>
        /// <param name="commandLineAppPrinterFactory">
        /// The <see cref="ICommandLineAppPrinterFactory"/> used to create <see cref="ICommandLineAppPrinter"/> instances for
        /// <see cref="CommandLineAppContext"/> instances.
        /// </param>
        public CommandLineAppContextFactory(ICommandLineAppPrinterFactory commandLineAppPrinterFactory)
        {
            _appPrinterFactory = commandLineAppPrinterFactory;
        }

        /// <inheritdoc/>
        public CommandLineAppContext Create(CommandDictionary commandDictionary, CommandLineAppOptions appOptions)
        {
            ICommandLineAppPrinter appPrinter = _appPrinterFactory.Create(commandDictionary, appOptions);

            return new CommandLineAppContext(commandDictionary, appOptions, appPrinter);
        }
    }
}
