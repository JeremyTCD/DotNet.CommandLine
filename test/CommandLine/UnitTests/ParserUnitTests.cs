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
            Assert.Equal(string.Format(Strings.Exception_CommandDoesNotExist, dummyCommandName), exception.Message);
        }

        [Fact]
        public void GetCommandByName_ReturnsRequestedCommandIfCommandNameProvided()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            Command dummyCommand = new Command(null, false, null, null, null);

            CommandSet dummyCommandSet = new CommandSet
            {
                { dummyCommandName, dummyCommand }
            };

            Parser parser = new Parser(null, null);

            // Act 
            Command result = parser.GetCommandByName(dummyCommandName, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void GetCommandByName_ThrowsParseExceptionIfDefaultNoCommandNameProvidedAndNoDefaultCommandExists()
        {
            // Arrange
            CommandSet dummyCommandSet = new CommandSet();
            Parser parser = new Parser(null, null);

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => parser.GetCommandByName(null, dummyCommandSet));
            Assert.Equal(Strings.Exception_NoDefaultCommand, exception.Message);
        }

        [Fact]
        public void GetCommandByName_ReturnsDefaultCommandIfNoCommandNameProvided()
        {
            // Arrange
            Command dummyCommand = new Command(null, true, null, null, null);

            CommandSet dummyCommandSet = new CommandSet
            {
                { "", dummyCommand }
            };

            Parser parser = new Parser(null, null);

            // Act 
            Command result = parser.GetCommandByName(null, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingCommandAndModelInstancesIfSuccessful()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            string dummyCommandName = "dummyCommandName";
            Arguments dummyArguments = new Arguments(dummyCommandName, null);
            Command dummyCommand = new Command(null, false, null, null, null);
            CommandSet dummyCommandSet = new CommandSet() { { dummyCommandName, dummyCommand } };
            object dummyModel = new object();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<IModelFactory> mockModelFactory = _mockRepository.Create<IModelFactory>();
            mockModelFactory.Setup(m => m.Create(dummyArguments, dummyCommand)).Returns(dummyModel);

            Parser parser = new Parser(mockArgumentsFactory.Object, mockModelFactory.Object);

            // Act
            ParseResult result = parser.Parse(dummyArgs, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result.Command);
            Assert.Equal(dummyModel, result.Model);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingParseExceptionIfParsingLogicThrowsParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            string dummyCommandName = "dummyCommandName";
            Arguments dummyArguments = new Arguments(dummyCommandName, null);
            Command dummyCommand = new Command(null, false, null, null, null);
            CommandSet dummyCommandSet = new CommandSet() { { dummyCommandName, dummyCommand } };
            ParseException dummyParseException = new ParseException();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<IModelFactory> mockModelFactory = _mockRepository.Create<IModelFactory>();
            mockModelFactory.Setup(m => m.Create(dummyArguments, dummyCommand)).Throws(dummyParseException);

            Parser parser = new Parser(mockArgumentsFactory.Object, mockModelFactory.Object);

            // Act
            ParseResult result = parser.Parse(dummyArgs, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result.Command);
            Assert.Equal(dummyParseException, result.ParseException);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingParseExceptionWithInnerExceptionIfParsingLogicThrowsAnyExceptionOtherThanParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            string dummyCommandName = "dummyCommandName";
            Arguments dummyArguments = new Arguments(dummyCommandName, null);
            Command dummyCommand = new Command(null, false, null, null, null);
            CommandSet dummyCommandSet = new CommandSet() { { dummyCommandName, dummyCommand } };
            Exception dummyException = new Exception();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<IModelFactory> mockModelFactory = _mockRepository.Create<IModelFactory>();
            mockModelFactory.Setup(m => m.Create(dummyArguments, dummyCommand)).Throws(dummyException);

            Parser parser = new Parser(mockArgumentsFactory.Object, mockModelFactory.Object);

            // Act
            ParseResult result = parser.Parse(dummyArgs, dummyCommandSet);

            // Assert
            Assert.Equal(dummyCommand, result.Command);
            Assert.NotNull(result.ParseException);
            Assert.Equal(dummyException, result.ParseException.InnerException);
        }
    }
}
