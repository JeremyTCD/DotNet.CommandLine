using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandMapperUnitTests
    {
        private MockRepository _mockRepository { get; } = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Map_ThrowsParseExceptionIfAnArgumentOptionDoesNotExist()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, null } });
            List<Option> dummyOptions = new List<Option>();
            DummyCommand dummyCommand = new DummyCommand();

            Mock<CommandMapper> mockCommandMapper = _mockRepository.Create<CommandMapper>(null, null);
            mockCommandMapper.Setup(c => c.GetOptionsFromCommand(dummyCommand)).Returns(dummyOptions);
            mockCommandMapper.CallBase = true;

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => mockCommandMapper.Object.Map(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_OptionDoesNotExist, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfAnExceptionIsThrownWhileMapping()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            Exception dummyException = new Exception();

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, mockCommand.Object)).Throws(dummyException);

            Mock<CommandMapper> mockCommandMapper = _mockRepository.Create<CommandMapper>(new IMapper[] { mockMapper.Object }, null);
            mockCommandMapper.Setup(c => c.GetOptionsFromCommand(mockCommand.Object)).Returns(dummyOptions);
            mockCommandMapper.CallBase = true;

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => mockCommandMapper.Object.Map(dummyArguments, mockCommand.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
            Assert.Equal(dummyException, parseException.InnerException);
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfNoMapperCanHandleAnArgumentOptionValue()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, mockCommand.Object)).Returns(false);

            Mock<CommandMapper> mockCommandMapper = _mockRepository.Create<CommandMapper>(new IMapper[] { mockMapper.Object }, null);
            mockCommandMapper.Setup(c => c.GetOptionsFromCommand(mockCommand.Object)).Returns(dummyOptions);
            mockCommandMapper.CallBase = true;

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => mockCommandMapper.Object.Map(dummyArguments, mockCommand.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Map_MapsArgumentOptionsToCommandsProperties()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, mockCommand.Object)).Returns(true);

            Mock<CommandMapper> mockCommandMapper = _mockRepository.Create<CommandMapper>(new IMapper[] { mockMapper.Object }, null);
            mockCommandMapper.Setup(c => c.GetOptionsFromCommand(mockCommand.Object)).Returns(dummyOptions);
            mockCommandMapper.CallBase = true;

            // Act 
            mockCommandMapper.Object.Map(dummyArguments, mockCommand.Object);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void GetOptionsFromCommand_GetsOptionsFromCommand()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand();
            PropertyInfo propertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(null, null, null, null);

            Mock<IOptionFactory> mockOptionFactory = _mockRepository.Create<IOptionFactory>();
            mockOptionFactory.Setup(o => o.TryCreateFromPropertyInfo(It.IsAny<PropertyInfo>())).Returns((Option)null);
            mockOptionFactory.Setup(o => o.TryCreateFromPropertyInfo(propertyInfo)).Returns(dummyOption);

            CommandMapper commandMapper = new CommandMapper(null, mockOptionFactory.Object);

            // Act
            List<Option> result = commandMapper.GetOptionsFromCommand(dummyCommand);

            // Assert
            Assert.Single(result);
            Assert.Equal(dummyOption, result[0]);
        }

        private class DummyCommand : ICommand
        {
            public string Name => throw new NotImplementedException();
            public string Description => throw new NotImplementedException();
            public bool IsDefault => throw new NotImplementedException();

            [Option()]
            public string DummyProperty { get; }

            public int Run(ParseResult parseResult, IPrinter printer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
