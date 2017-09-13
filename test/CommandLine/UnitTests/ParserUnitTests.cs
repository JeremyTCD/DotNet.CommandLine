using Moq;
using System;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class ParserUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void GetCommand_ThrowsParseExceptionIfNoCommandWithNameCommandNameExists()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";

            ICommand dummyCommand = null;
            Mock<CommandSet> mockCommandSet = _mockRepository.Create<CommandSet>();
            mockCommandSet.Setup(c => c.TryGetValue(dummyCommandName, out dummyCommand));

            Parser parser = new Parser(null, null);

            // Act and Assert                                   
            ParseException exception = Assert.Throws<ParseException>(() => parser.GetCommand(dummyCommandName, mockCommandSet.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_CommandDoesNotExist, dummyCommandName), exception.Message);
        }

        [Fact]
        public void GetCommand_ReturnsRequestedCommandIfCommandNameIsNotNull()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            ICommand dummyCommand = new DummyCommand();

            Mock<CommandSet> mockCommandSet = _mockRepository.Create<CommandSet>();
            mockCommandSet.Setup(c => c.TryGetValue(dummyCommandName, out dummyCommand));

            Parser parser = new Parser(null, null);

            // Act 
            ICommand result = parser.GetCommand(dummyCommandName, mockCommandSet.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void GetCommand_ReturnsDefaultCommandIfCommandNameIsNull()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand();

            Mock<CommandSet> mockCommandSet = _mockRepository.Create<CommandSet>();
            mockCommandSet.Setup(c => c.DefaultCommand).Returns(dummyCommand);

            Parser parser = new Parser(null, null);

            // Act 
            ICommand result = parser.GetCommand(null, mockCommandSet.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingCommandInstanceIfSuccessful()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            string dummyCommandName = "dummyCommandName";
            Arguments dummyArguments = new Arguments(dummyCommandName, null);
            DummyCommand dummyCommand = new DummyCommand();
            CommandSet dummyCommandSet = new CommandSet();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<ICommandMapper> mockCommandMapper = _mockRepository.Create<ICommandMapper>();
            mockCommandMapper.Setup(c => c.Map(dummyArguments, dummyCommand));

            Mock<Parser> mockParser = _mockRepository.
                Create<Parser>(mockArgumentsFactory.Object, mockCommandMapper.Object);
            mockParser.Setup(p => p.GetCommand(dummyCommandName, dummyCommandSet)).Returns(dummyCommand);
            mockParser.CallBase = true;

            // Act
            ParseResult result = mockParser.Object.Parse(dummyArgs, dummyCommandSet);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand, result.Command);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingParseExceptionIfParsingLogicThrowsParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            ParseException dummyParseException = new ParseException();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Throws(dummyParseException);

            Parser parser = new Parser(mockArgumentsFactory.Object, null);

            // Act
            ParseResult result = parser.Parse(dummyArgs, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyParseException, result.ParseException);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingParseExceptionWithInnerExceptionIfParsingLogicThrowsAnyExceptionOtherThanParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            Exception dummyException = new Exception();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Throws(dummyException);

            Parser parser = new Parser(mockArgumentsFactory.Object, null);

            // Act
            ParseResult result = parser.Parse(dummyArgs, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result.ParseException);
            Assert.Equal(dummyException, result.ParseException.InnerException);
        }

        private class DummyCommand : ICommand
        {
            public string Name => throw new NotImplementedException();
            public string Description => throw new NotImplementedException();
            public bool IsDefault { get; }

            public DummyCommand(bool isDefault = false) 
            {
                IsDefault = isDefault;
            }

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
