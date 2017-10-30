// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.NotBool));
            FlagMapper testSubject = CreateFlagMapper();

            // Act
            bool result = testSubject.TryMap(propertyInfo, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsFalseIfValueIsNotNull()
        {
            // Arrange
            FlagMapper testSubject = CreateFlagMapper();

            // Act
            bool result = testSubject.TryMap(null, "dummy", null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryMap_ReturnsTrueIfMappingIsSuccessful()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.Bool));
            DummyCommand dummyCommand = new DummyCommand();
            FlagMapper testSubject = CreateFlagMapper();

            // Act
            bool result = testSubject.TryMap(propertyInfo, null, dummyCommand);

            // Assert
            Assert.True(dummyCommand.Bool);
            Assert.True(result);
        }

        private FlagMapper CreateFlagMapper()
        {
            return new FlagMapper();
        }

        private class DummyCommand : ICommand
        {
            public bool Bool { get; set; }

            public string NotBool { get; set; }

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
