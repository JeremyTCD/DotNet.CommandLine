using JeremyTCD.DotNetCore.Utils;
using Moq;
using System;
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
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.NotCollection));
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
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.StringCollection));
            DummyModel dummyModel = new DummyModel();
            CollectionMapper collectionMapper = new CollectionMapper(activatorService);
            string dummyString = "1,2,3";

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, dummyString, dummyModel);

            // Assert
            Assert.Equal(new List<string> { "1", "2", "3" }, dummyModel.StringCollection);
            Assert.True(result);
        }

        [Fact]
        public void TryMap_PerformsConversionsWhenMapping()
        {
            // Arrange
            Mock<ILoggingService<ActivatorService>> mockASLS = new Mock<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.IntCollection));
            DummyModel dummyModel = new DummyModel();
            CollectionMapper collectionMapper = new CollectionMapper(activatorService);
            string dummyString = "1,2,3";

            // Act
            bool result = collectionMapper.TryMap(propertyInfo, dummyString, dummyModel);

            // Assert
            Assert.Equal(new List<int> { 1, 2, 3 }, dummyModel.IntCollection);
            Assert.True(result);
        }

        private class DummyModel
        {
            public List<string> StringCollection { get; set; }
            public List<int> IntCollection { get; set; }
            public string NotCollection { get; set; }
        }
    }
}
