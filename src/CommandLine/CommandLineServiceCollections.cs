using JeremyTCD.DotNetCore.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JeremyTCD.DotNet.CommandLine
{
    public static class CommandLineServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandLine(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandLineTool, CommandLineTool>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandSetFactory, CommandSetFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IParser, Parser>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IPrinter, Printer>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ICommandFactory, CommandFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IOptionFactory, OptionFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IArgumentsFactory, ArgumentsFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton<IModelFactory, ModelFactory>());

            // Multiple implementations of IMapper are allowed
            serviceCollection.AddSingleton<IMapper, CollectionMapper>();
            serviceCollection.AddSingleton<IMapper, FlagMapper>();
            serviceCollection.AddSingleton<IMapper, StringConvertibleMapper>();

            serviceCollection.AddUtils();

            return serviceCollection;
        }
    }
}
