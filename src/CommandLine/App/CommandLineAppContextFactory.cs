// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class CommandLineAppContextFactory : ICommandLineAppContextFactory
    {
        private ICommandLineAppPrinterFactory _appPrinterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineAppContextFactory"/> class.
        /// </summary>
        /// <param name="commandLineAppPrinterFactory">
        /// The <see cref="ICommandLineAppPrinterFactory"/> used to create <see cref="ICommandLineAppPrinter"/>s for
        /// <see cref="CommandLineAppContext"/>s.
        /// </param>
        public CommandLineAppContextFactory(ICommandLineAppPrinterFactory commandLineAppPrinterFactory)
        {
            _appPrinterFactory = commandLineAppPrinterFactory;
        }

        /// <inheritdoc/>
        public ICommandLineAppContext Create(CommandDictionary commandDictionary, CommandLineAppOptions appOptions)
        {
            ICommandLineAppPrinter appPrinter = _appPrinterFactory.Create(commandDictionary, appOptions);

            return new CommandLineAppContext(commandDictionary, appOptions, appPrinter);
        }
    }
}
