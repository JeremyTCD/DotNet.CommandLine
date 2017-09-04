using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class StringConvertibleMapperIntegrationTests
    {
        [Theory]
        [MemberData(nameof(ReturnsTrueIfTypeCanBeConvertedToFromString))]
        public void IsBuiltInType_ReturnsTrueIfTypeCanBeConvertedToFromString(Type type)
        {
            // Arrange
            StringConvertibleMapper builtInTypeMapper = new StringConvertibleMapper();

            // Act
            bool result = builtInTypeMapper.CanBeConvertedToFromString(type);

            // Assert
            Assert.True(result);
        }

        public static IEnumerable<object[]> ReturnsTrueIfTypeCanBeConvertedToFromString()
        {
            yield return new object[] { typeof(bool) };
            yield return new object[] { typeof(byte) };
            yield return new object[] { typeof(sbyte) };
            yield return new object[] { typeof(short) };
            yield return new object[] { typeof(char) };
            yield return new object[] { typeof(double) };
            yield return new object[] { typeof(float) };
            yield return new object[] { typeof(int) };
            yield return new object[] { typeof(uint) };
            yield return new object[] { typeof(long) };
            yield return new object[] { typeof(ulong) };
            yield return new object[] { typeof(ushort) };
            yield return new object[] { typeof(string) };
            yield return new object[] { typeof(DateTime) };
            yield return new object[] { typeof(decimal) };
        }

        [Theory]
        [MemberData(nameof(ReturnsFalseIfTypeCannotBeConvertedToFromString))]
        public void IsBuiltInType_ReturnsFalseIfTypeCannotBeConvertedToFromString(Type type)
        {
            // Arrange
            StringConvertibleMapper builtInTypeMapper = new StringConvertibleMapper();

            // Act
            bool result = builtInTypeMapper.CanBeConvertedToFromString(type);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> ReturnsFalseIfTypeCannotBeConvertedToFromString()
        {
            yield return new object[] { typeof(List<>) };
            yield return new object[] { typeof(Array) };
            yield return new object[] { typeof(DummyModel) };
        }

        [Fact]
        public void TryMap_ReturnsFalseIfValueIsNull()
        {
            // Arrange
            StringConvertibleMapper defaultMapper = new StringConvertibleMapper();

            // Act
            bool result = defaultMapper.TryMap(null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfPropertyTypeCannotBeConvertedToFromString()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.NotConvertible));
            StringConvertibleMapper defaultMapper = new StringConvertibleMapper();

            // Act
            bool result = defaultMapper.TryMap(propertyInfo, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfMappingIsSuccessful()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.Convertible));
            DummyModel dummyModel = new DummyModel();
            StringConvertibleMapper defaultMapper = new StringConvertibleMapper();
            string dummyString = "1";

            // Act
            bool result = defaultMapper.TryMap(propertyInfo, dummyString, dummyModel);

            // Assert
            Assert.Equal(1, dummyModel.Convertible);
            Assert.True(result);
        }

        private class DummyModel
        {
            public List<int> NotConvertible { get; set; }
            public int Convertible { get; set; }
        }
    }
}
