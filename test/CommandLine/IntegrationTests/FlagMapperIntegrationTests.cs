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
            PropertyInfo propertyInfo = typeof(StubCommand).GetProperty(nameof(StubCommand.NotBool));
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
            PropertyInfo propertyInfo = typeof(StubCommand).GetProperty(nameof(StubCommand.Bool));
            StubCommand stubCommand = new StubCommand();
            FlagMapper flagMapper = new FlagMapper();

            // Act
            bool result = flagMapper.TryMap(propertyInfo, null, stubCommand);

            // Assert
            Assert.True(stubCommand.Bool);
            Assert.True(result);
        }

        private class StubCommand : ICommand
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
