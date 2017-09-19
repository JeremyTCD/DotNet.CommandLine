// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class AppPrinterFactory : IAppPrinterFactory
    {
        private readonly IOptionsFactory _optionsFactory;

        public AppPrinterFactory(IOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        public IAppPrinter Create(CommandSet commandSet, AppOptions appOptions)
        {
            return new AppPrinter(commandSet, appOptions, _optionsFactory);
        }
    }
}
