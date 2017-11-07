// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineAppPrinterFactory : ICommandLineAppPrinterFactory
    {
        private readonly IOptionCollectionFactory _optionCollectionFactory;

        public CommandLineAppPrinterFactory(IOptionCollectionFactory optionCollectionFactory)
        {
            _optionCollectionFactory = optionCollectionFactory;
        }

        public virtual ICommandLineAppPrinter Create(ICommandDictionary commandDictionary, CommandLineAppOptions appOptions)
        {
            return new CommandLineAppPrinter(commandDictionary, appOptions, _optionCollectionFactory);
        }
    }
}
