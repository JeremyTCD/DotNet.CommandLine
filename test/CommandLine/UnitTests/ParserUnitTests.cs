using Moq;
using System;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class ParserUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void GetCommandByName_ThrowsParseExceptionIfNoCommandWithProvidedCommandNameExists()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            CommandSet dummyCommandSet = new CommandSet();
            Parser parser = new Parser(null, null);

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => parser.GetCommandByName(dummyCommandName, dummyCommandSet));
            Assert.Equal(string.Format(Strings.ParseException_CommandDoesNotExist, dummyCommandName), exception.Message);
        }

        [Fact]
        public void GetCommandByName_ReturnsRequestedCommandIfCommandNameProvided()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            DummyCommand dummyCommand = new DummyCommand();

            CommandSet dummyCommandSet = new CommandSet
            {
                { dummyCommandName, dummyCommand }
            };

            Parser parser = new Parser(null, null);

            // Act 
            ICommand result = parser.GetCommandByName(dummyCommandName, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void GetCommandByName_ThrowsParseExceptionIfNoCommandNameProvidedAndNoDefaultCommandExists()
        {
            // Arrange
            CommandSet dummyCommandSet = new CommandSet();
            Parser parser = new Parser(null, null);

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => parser.GetCommandByName(null, dummyCommandSet));
            Assert.Equal(Strings.ParseException_NoDefaultCommand, exception.Message);
        }

        [Fact]
        public void GetCommandByName_ReturnsDefaultCommandIfNoCommandNameProvided()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand(true);

            CommandSet dummyCommandSet = new CommandSet
            {
                { "", dummyCommand }
            };

            Parser parser = new Parser(null, null);

            // Act 
            ICommand result = parser.GetCommandByName(null, dummyCommandSet);

            // Assert
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

            Mock<Parser> mockParser = _mockRepository.Create<Parser>(mockArgumentsFactory.Object, mockCommandMapper.Object);
            mockParser.Setup(p => p.GetCommandByName(dummyCommandName, dummyCommandSet)).Returns(dummyCommand);
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
