// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JeremyTCD.DotNetCore.Utils;
using Moq;
using System.Collections.Generic;
using System.Reflection;
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
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.NotCollection));
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
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.StringCollection));
            DummyCommand dummyCommand = new DummyCommand();
            CollectionMapper collectionMapper = new CollectionMapper(activatorService);
            string dummyString = "1,2,3";

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, dummyString, dummyCommand);

            // Assert
            Assert.Equal(new List<string> { "1", "2", "3" }, dummyCommand.StringCollection);
            Assert.True(result);
        }

        [Fact]
        public void TryMap_PerformsConversionsWhenMapping()
        {
            // Arrange
            Mock<ILoggingService<ActivatorService>> mockASLS = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.IntCollection));
            DummyCommand dummyCommand = new DummyCommand();
            CollectionMapper collectionMapper = new CollectionMapper(activatorService);
            string dummyString = "1,2,3";

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, dummyString, dummyCommand);

            // Assert
            Assert.Equal(new List<int> { 1, 2, 3 }, dummyCommand.IntCollection);
            Assert.True(result);
        }

        private class DummyCommand : ICommand
        {
            public List<string> StringCollection { get; set; }
            public List<int> IntCollection { get; set; }
            public string NotCollection { get; set; }

            public string Name => throw new System.NotImplementedException();

            public string Description => throw new System.NotImplementedException();

            public bool IsDefault => throw new System.NotImplementedException();

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
