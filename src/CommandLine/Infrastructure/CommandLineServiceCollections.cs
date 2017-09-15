using JeremyTCD.DotNetCore.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JeremyTCD.DotNet.CommandLine
{
    public static class CommandLineServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandLine(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineApp, CommandLineApp>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandSetFactory, CommandSetFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IParser, Parser>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IAppPrinter, AppPrinter>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandMapper, CommandMapper>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IOptionsFactory, OptionsFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IArgumentsFactory, ArgumentsFactory>());

            // Multiple implementations of IMapper are allowed
            serviceCollection.AddSingleton<IMapper, CollectionMapper>();
            serviceCollection.AddSingleton<IMapper, FlagMapper>();
            serviceCollection.AddSingleton<IMapper, StringConvertibleMapper>();

            serviceCollection.AddUtils();

            return serviceCollection;
        }
    }
}
