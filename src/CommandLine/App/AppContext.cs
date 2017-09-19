// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContext
    {
        public readonly CommandSet CommandSet;
        public readonly AppOptions AppOptions;
        public readonly IAppPrinter AppPrinter;

        public AppContext(CommandSet commandSet, AppOptions appOptions, IAppPrinter appPrinter)
        {
            AppPrinter = appPrinter;
            CommandSet = commandSet;
            AppOptions = appOptions;
        }
    }
}
