﻿// Copyright (c) JeremyTCD. All rights reserved.
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
            CollectionMapper collectionMapper = CreateCollectionMapper();

            // Act
            bool result = collectionMapper.TryMap(null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfPropertyTypeIsNotAssignableToCollection()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.NotCollection));

            CollectionMapper collectionMapper = CreateCollectionMapper();

            // Act
            bool result = collectionMapper.TryMap(dummyPropertyInfo, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfPropertyTypeIsBoolAndMappingIsSuccessful()
        {
            // Arrange
            string dummyString = "1,2,3";
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.StringCollection));
            DummyCommand dummyCommand = new DummyCommand();

            Mock<ILoggingService<ActivatorService>> mockLoggingService = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockLoggingService.Object);

            CollectionMapper collectionMapper = CreateCollectionMapper(activatorService);

            // Act
            bool result = collectionMapper.TryMap(dummyPropertyInfo, dummyString, dummyCommand);

            // Assert
            Assert.Equal(new List<string> { "1", "2", "3" }, dummyCommand.StringCollection);
            Assert.True(result);
        }

        [Fact]
        public void TryMap_PerformsConversionsWhenMapping()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.IntCollection));
            DummyCommand dummyCommand = new DummyCommand();
            string dummyString = "1,2,3";

            Mock<ILoggingService<ActivatorService>> mockLoggingService = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockLoggingService.Object);

            CollectionMapper collectionMapper = CreateCollectionMapper(activatorService);

            // Act
            bool result = collectionMapper.TryMap(dummyPropertyInfo, dummyString, dummyCommand);

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

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new System.NotImplementedException();
            }
        }

        private CollectionMapper CreateCollectionMapper(IActivatorService activatorService = null)
        {
            return new CollectionMapper(activatorService);
        }
    }
}
