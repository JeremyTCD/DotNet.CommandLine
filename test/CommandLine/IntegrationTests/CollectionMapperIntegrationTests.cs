// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Reflection;
using JeremyTCD.DotNetCore.Utils;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CollectionMapperIntegrationTests
    {
        [Fact]
        public void TryMap_ReturnsFalseIfValueIsNull()
        {
            // Arrange
            CollectionMapper collectionMapper = new CollectionMapper(null);

            // Act
            bool result = collectionMapper.TryMap(null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfPropertyTypeIsNotAssignableToCollection()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(StubCommand).GetProperty(nameof(StubCommand.NotCollection));
            CollectionMapper collectionMapper = new CollectionMapper(null);

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfPropertyTypeIsBoolAndMappingIsSuccessful()
        {
            // Arrange
            Mock<ILoggingService<ActivatorService>> mockASLS = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);
            PropertyInfo propertyInfo = typeof(StubCommand).GetProperty(nameof(StubCommand.StringCollection));
            StubCommand stubCommand = new StubCommand();
            CollectionMapper collectionMapper = new CollectionMapper(activatorService);
            string dummyString = "1,2,3";

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, dummyString, stubCommand);

            // Assert
            Assert.Equal(new List<string> { "1", "2", "3" }, stubCommand.StringCollection);
            Assert.True(result);
        }

        [Fact]
        public void TryMap_PerformsConversionsWhenMapping()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(StubCommand).GetProperty(nameof(StubCommand.IntCollection));
            StubCommand stubCommand = new StubCommand();
            string dummyString = "1,2,3";

            Mock<ILoggingService<ActivatorService>> mockASLS = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);

            CollectionMapper collectionMapper = new CollectionMapper(activatorService);

            // Act
            bool result = collectionMapper.TryMap(dummyPropertyInfo, dummyString, stubCommand);

            // Assert
            Assert.Equal(new List<int> { 1, 2, 3 }, stubCommand.IntCollection);
            Assert.True(result);
        }

        private class StubCommand : ICommand
        {
            public List<string> StringCollection { get; set; }

            public List<int> IntCollection { get; set; }

            public string NotCollection { get; set; }

            public string Name => throw new System.NotImplementedException();

            public string Description => throw new System.NotImplementedException();

            public bool IsDefault => throw new System.NotImplementedException();

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
