// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineAppContextFactory : ICommandLineAppContextFactory
    {
        private ICommandLineAppPrinterFactory _appPrinterFactory;

        public CommandLineAppContextFactory(ICommandLineAppPrinterFactory appPrinterFactory)
        {
            _appPrinterFactory = appPrinterFactory;
        }

        public CommandLineAppContext Create(CommandSet commandSet, CommandLineAppOptions appOptions)
        {
            ICommandLineAppPrinter appPrinter = _appPrinterFactory.Create(commandSet, appOptions);

            return new CommandLineAppContext(commandSet, appOptions, appPrinter);
        }
    }
}
