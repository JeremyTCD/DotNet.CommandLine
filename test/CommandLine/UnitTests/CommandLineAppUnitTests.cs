// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandLineAppUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Run_ParsesCallsCommandRunAndReturnsExitCode()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            CommandDictionary dummyCommandDictionary = new CommandDictionary();
            string[] dummyArgs = new string[0];
            int dummyExitCode = 1;
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions();
            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, null);

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            ParseResult dummyParseResult = new ParseResult(null, mockCommand.Object);
            mockCommand.Setup(c => c.Run(dummyParseResult, dummyAppContext)).Returns(dummyExitCode);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(dummyCommandDictionary);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandDictionary)).Returns(dummyParseResult);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(dummyCommandDictionary, dummyAppOptions)).Returns(dummyAppContext);

            CommandLineApp commandLineApp = new CommandLineApp(
                mockParser.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockAppContextFactory.Object,
                mockOptionsAccessor.Object);

            // Act
            int result = commandLineApp.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }

        [Fact]
        public void Run_CallsDefaultCommandRunIfParseResultHasNoCommand()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            string[] dummyArgs = new string[0];
            ParseResult dummyParseResult = new ParseResult(null, null);
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions();
            int dummyExitCode = 1;
            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, null);

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            mockCommand.Setup(c => c.Run(dummyParseResult, dummyAppContext)).Returns(dummyExitCode);

            Mock<CommandDictionary> mockCommandDictionary = _mockRepository.Create<CommandDictionary>();
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(mockCommand.Object);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(mockCommandDictionary.Object);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, mockCommandDictionary.Object)).Returns(dummyParseResult);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(mockCommandDictionary.Object, dummyAppOptions)).Returns(dummyAppContext);

            CommandLineApp commandLineApp = new CommandLineApp(
                mockParser.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockAppContextFactory.Object,
                mockOptionsAccessor.Object);

            // Act
            int result = commandLineApp.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }
    }
}
