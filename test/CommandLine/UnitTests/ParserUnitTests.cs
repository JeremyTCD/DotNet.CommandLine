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
        public void Parse_ReturnsParseResultContainingCommandInstanceIfSuccessful()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            string dummyCommandName = "dummyCommandName";
            Arguments dummyArguments = new Arguments(dummyCommandName, null);

            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<ICommandDictionary> dummyCommandDictionary = _mockRepository.Create<ICommandDictionary>();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Returns(dummyArguments);

            Mock<ICommandMapper> mockCommandMapper = _mockRepository.Create<ICommandMapper>();
            mockCommandMapper.Setup(c => c.Map(dummyArguments, dummyCommand.Object));

            Mock<Parser> testSubject = CreateMockParser(mockArgumentsFactory.Object, mockCommandMapper.Object);
            testSubject.Setup(p => p.GetCommand(dummyCommandName, dummyCommandDictionary.Object)).Returns(dummyCommand.Object);
            testSubject.CallBase = true;

            // Act
            IParseResult result = testSubject.Object.Parse(dummyArgs, dummyCommandDictionary.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand.Object, result.Command);
        }

        [Fact]
        public void Parse_ReturnsParseResultContainingParseExceptionIfParsingLogicThrowsParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            ParseException dummyParseException = new ParseException();

            Mock<IArgumentsFactory> mockArgumentsFactory = _mockRepository.Create<IArgumentsFactory>();
            mockArgumentsFactory.Setup(a => a.CreateFromArray(dummyArgs)).Throws(dummyParseException);

            Parser testSubject = CreateParser(mockArgumentsFactory.Object);

            // Act
            IParseResult result = testSubject.Parse(dummyArgs, null);

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

            Parser testSubject = CreateParser(mockArgumentsFactory.Object);

            // Act
            IParseResult result = testSubject.Parse(dummyArgs, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result.ParseException);
            Assert.Equal(dummyException, result.ParseException.InnerException);
        }

        [Fact]
        public void GetCommand_ThrowsParseExceptionIfNoCommandWithNameCommandNameExists()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";

            ICommand dummyCommand = null;
            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.TryGetValue(dummyCommandName, out dummyCommand));

            Parser testSubject = CreateParser();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => testSubject.GetCommand(dummyCommandName, mockCommandDictionary.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_CommandDoesNotExist, dummyCommandName), exception.Message);
        }

        [Fact]
        public void GetCommand_ReturnsRequestedCommandIfCommandNameIsNotNull()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";

            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            ICommand outValue = dummyCommand.Object;
            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.TryGetValue(dummyCommandName, out outValue));

            Parser testSubject = CreateParser();

            // Act
            ICommand result = testSubject.GetCommand(dummyCommandName, mockCommandDictionary.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand.Object, result);
        }

        [Fact]
        public void GetCommand_ReturnsDefaultCommandIfCommandNameIsNull()
        {
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(dummyCommand.Object);

            Parser testSubject = CreateParser();

            // Act
            ICommand result = testSubject.GetCommand(null, mockCommandDictionary.Object);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyCommand.Object, result);
        }

        private Mock<Parser> CreateMockParser(IArgumentsFactory argumentsFactory = null, ICommandMapper commandMapper = null)
        {
            return _mockRepository.Create<Parser>(argumentsFactory, commandMapper);
        }

        private Parser CreateParser(IArgumentsFactory argumentsFactory = null, ICommandMapper commandMapper = null)
        {
            return new Parser(argumentsFactory, commandMapper);
        }
    }
}
