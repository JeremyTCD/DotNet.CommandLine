using JeremyTCD.DotNetCore.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.IntegrationTests
{
    public class CommandLineServiceCollectionExtensionsIntegrationTests
    {
        [Fact]
        public void AddCommandLine_ConfiguresServicesCorrectly()
        {
            // Arrange 
            ServiceCollection services = new ServiceCollection();

            // Act
            services.AddCommandLine();

            // Assert
            ServiceDescriptorComparer comparer = new ServiceDescriptorComparer();
            Assert.Contains(ServiceDescriptor.Singleton<ICommandLineApp, CommandLineApp>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<ICommandSetFactory, CommandSetFactory>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IParser, Parser>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IAppPrinter, AppPrinter>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<ICommandMapper, CommandMapper>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IOptionsFactory, OptionsFactory>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IArgumentsFactory, ArgumentsFactory>(), services, comparer);

            Assert.Contains(ServiceDescriptor.Singleton<IMapper, CollectionMapper>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IMapper, FlagMapper>(), services, comparer);
            Assert.Contains(ServiceDescriptor.Singleton<IMapper, StringConvertibleMapper>(), services, comparer);
        }

        [Fact]
        public void AddCommandLine_ConfiguresMappersSoTheyCanBeInjectedInIEnumerable()
        {
            // Arrange 
            ServiceCollection services = new ServiceCollection();

            // Act
            services.AddCommandLine();
            IEnumerable<IMapper> mappers = services.BuildServiceProvider().GetService<IEnumerable<IMapper>>();

            // Assert
            Assert.Equal(3, mappers.Count());
            Assert.Contains(mappers, m => m.GetType() == typeof(CollectionMapper));
            Assert.Contains(mappers, m => m.GetType() == typeof(FlagMapper));
            Assert.Contains(mappers, m => m.GetType() == typeof(StringConvertibleMapper));
        }
    }
}
