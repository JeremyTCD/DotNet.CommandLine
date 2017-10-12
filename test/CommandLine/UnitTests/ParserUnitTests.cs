// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
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
            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.TryGetValue(dummyCommandName, out dummyCommand));

            Parser parser = CreateParser();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => parser.GetCommand(dummyCommandName, mockCommandDictionary.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_CommandDoesNotExist, dummyCommandName), exception.Message);
        }

        [Fact]
        public void GetCommand_ReturnsRequestedCommandIfCommandNameIsNotNull()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            ICommand dummyCommand = new DummyCommand();

            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.TryGetValue(dummyCommandName, out dummyCommand));

            Parser parser = CreateParser();

            // Act
            ICommand result = parser.GetCommand(dummyCommandName, mockCommandDictionary.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand, result);
        }

        [Fact]
        public void GetCommand_ReturnsDefaultCommandIfCommandNameIsNull()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand();

            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(dummyCommand);

            Parser parser = CreateParser();

            // Act
            ICommand result = parser.GetCommand(null, mockCommandDictionary.Object);

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
            CommandDictionary dummyCommandDictionary = new CommandDictionary();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<ICommandMapper> mockCommandMapper = _mockRepository.Create<ICommandMapper>();
            mockCommandMapper.Setup(c => c.Map(dummyArguments, dummyCommand));

            Mock<Parser> mockParser = _mockRepository.
                Create<Parser>(mockArgumentsFactory.Object, mockCommandMapper.Object);
            mockParser.Setup(p => p.GetCommand(dummyCommandName, dummyCommandDictionary)).Returns(dummyCommand);
            mockParser.CallBase = true;

            // Act
            ParseResult result = mockParser.Object.Parse(dummyArgs, dummyCommandDictionary);

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

            Parser parser = CreateParser(mockArgumentsFactory.Object);

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

            Parser parser = CreateParser(mockArgumentsFactory.Object);

            // Act
            ParseResult result = parser.Parse(dummyArgs, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result.ParseException);
            Assert.Equal(dummyException, result.ParseException.InnerException);
        }

        private class DummyCommand : ICommand
        {
            public DummyCommand(bool isDefault = false)
            {
                IsDefault = isDefault;
            }

            public string Name => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public bool IsDefault { get; }

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }

        private Parser CreateParser(IArgumentsFactory argumentsFactory = null, ICommandMapper commandMapper = null)
        {
            return new Parser(argumentsFactory, commandMapper);
        }
    }
}
