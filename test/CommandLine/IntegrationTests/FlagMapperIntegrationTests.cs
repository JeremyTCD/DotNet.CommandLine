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
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.Bool));
            DummyCommand dummyCommand = new DummyCommand();
            FlagMapper flagMapper = new FlagMapper();

            // Act
            bool result = flagMapper.TryMap(propertyInfo, null, dummyCommand);

            // Assert
            Assert.True(dummyCommand.Bool);
            Assert.True(result);
        }

        private class DummyCommand : Command
        {
            public bool Bool { get; set; }
            public string NotBool { get; set; }

            public DummyCommand() : base(null, null, false) { }

            public override int Run(ParseResult parseResult, IPrinter printer)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
