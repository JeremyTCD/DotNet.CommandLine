// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineAppPrinterFactory : ICommandLineAppPrinterFactory
    {
        private readonly IOptionsFactory _optionsFactory;

        public CommandLineAppPrinterFactory(IOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        public ICommandLineAppPrinter Create(CommandDictionary commandDictionary, CommandLineAppOptions appOptions)
        {
            return new CommandLineAppPrinter(commandDictionary, appOptions, _optionsFactory);
        }
    }
}
