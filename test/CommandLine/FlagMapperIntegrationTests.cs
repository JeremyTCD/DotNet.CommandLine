using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class FlagMapperIntegrationTests
    {
        [Fact]
        public void TryMap_ReturnsFalseIfPropertyTypeIsNotBool()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.NotBool));
            FlagMapper flagMapper = new FlagMapper();

            // Act
            bool result = flagMapper.TryMap(propertyInfo, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfValueIsNotNull()
        {
            // Arrange
            FlagMapper flagMapper = new FlagMapper();

            // Act
            bool result = flagMapper.TryMap(null, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfMappingIsSuccessful()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.Bool));
            DummyModel dummyModel = new DummyModel();
            FlagMapper flagMapper = new FlagMapper();

            // Act
            bool result = flagMapper.TryMap(propertyInfo, null, dummyModel);

            // Assert
            Assert.True(dummyModel.Bool);
            Assert.True(result);
        }

        private class DummyModel
        {
            public bool Bool { get; set; }
            public string NotBool { get; set; }
        }
    }
}
