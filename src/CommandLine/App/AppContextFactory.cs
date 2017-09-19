// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContextFactory : IAppContextFactory
    {
        private IAppPrinterFactory _appPrinterFactory;

        public AppContextFactory(IAppPrinterFactory appPrinterFactory)
        {
            _appPrinterFactory = appPrinterFactory;
        }

        public AppContext Create(CommandSet commandSet, AppOptions appOptions)
        {
            IAppPrinter appPrinter = _appPrinterFactory.Create(commandSet, appOptions);

            return new AppContext(commandSet, appOptions, appPrinter);
        }
    }
}
