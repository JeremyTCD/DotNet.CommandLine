// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JeremyTCD.DotNetCore.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JeremyTCD.DotNet.CommandLine
{
    public static class CommandLineServiceCollectionExtensions
    {
        public static virtual IServiceCollection AddCommandLine(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineAppPrinterFactory, CommandLineAppPrinterFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineAppContextFactory, CommandLineAppContextFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineApp, CommandLineApp>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandDictionaryFactory, CommandDictionaryFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IParser, Parser>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineAppPrinter, CommandLineAppPrinter>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandMapper, CommandMapper>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IOptionsFactory, OptionsFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IArgumentsFactory, ArgumentsFactory>());

            // Multiple implementations of IMapper are allowed
            serviceCollection.AddSingleton<IMapper, CollectionMapper>();
            serviceCollection.AddSingleton<IMapper, FlagMapper>();
            serviceCollection.AddSingleton<IMapper, StringConvertibleMapper>();

            serviceCollection.AddUtils();
            serviceCollection.AddOptions();

            return serviceCollection;
        }
    }
}
