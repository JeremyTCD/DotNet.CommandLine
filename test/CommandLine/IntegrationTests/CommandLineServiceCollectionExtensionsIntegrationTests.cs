// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ICommandLineApp commandLineApp = serviceProvider.GetService<ICommandLineApp>();

            // Mappers are injected in an IEnumerable that can be empty, manually assert that the expected mappers are registered
            IEnumerable<IMapper> mappers = services.BuildServiceProvider().GetService<IEnumerable<IMapper>>();
            Assert.Equal(3, mappers.Count());
            Assert.Contains(mappers, m => m.GetType() == typeof(CollectionMapper));
            Assert.Contains(mappers, m => m.GetType() == typeof(FlagMapper));
            Assert.Contains(mappers, m => m.GetType() == typeof(StringConvertibleMapper));
        }
    }
}
