// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class StringConvertibleMapperIntegrationTests
    {
        [Theory]
        [MemberData(nameof(CanBeConvertedToFromString_ReturnsTrueIfTypeCanBeConvertedToFromString_Data))]
        public void CanBeConvertedToFromString_ReturnsTrueIfTypeCanBeConvertedToFromString(Type type)
        {
            // Arrange
            StringConvertibleMapper testSubject = CreateStringConvertibleMapper();

            // Act
            bool result = testSubject.CanBeConvertedToFromString(type);

            // Assert
            Assert.True(result);
        }

        public static IEnumerable<object[]> CanBeConvertedToFromString_ReturnsTrueIfTypeCanBeConvertedToFromString_Data()
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
        [MemberData(nameof(CanBeConvertedToFromString_ReturnsFalseIfTypeCannotBeConvertedToFromString_Data))]
        public void CanBeConvertedToFromString_ReturnsFalseIfTypeCannotBeConvertedToFromString(Type type)
        {
            // Arrange
            StringConvertibleMapper testSubject = CreateStringConvertibleMapper();

            // Act
            bool result = testSubject.CanBeConvertedToFromString(type);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> CanBeConvertedToFromString_ReturnsFalseIfTypeCannotBeConvertedToFromString_Data()
        {
            yield return new object[] { typeof(List<>) };
            yield return new object[] { typeof(Array) };
            yield return new object[] { typeof(DummyCommand) };
        }

        [Fact]
        public void TryMap_ReturnsFalseIfValueIsNull()
        {
            // Arrange
            StringConvertibleMapper testSubject = CreateStringConvertibleMapper();

            // Act
            bool result = testSubject.TryMap(null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfPropertyTypeCannotBeConvertedToFromString()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.NotConvertible));
            StringConvertibleMapper testSubject = CreateStringConvertibleMapper();

            // Act
            bool result = testSubject.TryMap(propertyInfo, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfMappingIsSuccessful()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.Convertible));
            DummyCommand dummyCommand = new DummyCommand();
            StringConvertibleMapper testSubject = CreateStringConvertibleMapper();
            string dummyString = "1";

            // Act
            bool result = testSubject.TryMap(propertyInfo, dummyString, dummyCommand);

            // Assert
            Assert.Equal(1, dummyCommand.Convertible);
            Assert.True(result);
        }

        private class DummyCommand : ICommand
        {
            public List<int> NotConvertible { get; set; }

            public int Convertible { get; set; }

            public string Name => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public bool IsDefault => throw new NotImplementedException();

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }

        private StringConvertibleMapper CreateStringConvertibleMapper()
        {
            return new StringConvertibleMapper();
        }
    }
}
