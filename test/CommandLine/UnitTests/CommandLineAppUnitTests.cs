// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandLineAppUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        [Fact]
        public void Run_RunsCommandSpecifiedByCommandLineArguments()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            string[] dummyArgs = new string[0];
            int dummyExitCode = 1;
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions();

            Mock<ICommandDictionary> dummyCommandDictionary = _mockRepository.Create<ICommandDictionary>();

            Mock<ICommandLineAppContext> dummyCommandLineAppContext = _mockRepository.Create<ICommandLineAppContext>();

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            ParseResult dummyParseResult = new ParseResult(null, mockCommand.Object);
            mockCommand.Setup(c => c.Run(dummyParseResult, dummyCommandLineAppContext.Object)).Returns(dummyExitCode);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(dummyCommandDictionary.Object);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandDictionary.Object)).Returns(dummyParseResult);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(dummyCommandDictionary.Object, dummyAppOptions)).Returns(dummyCommandLineAppContext.Object);

            CommandLineApp commandLineApp = CreateCommandLineApp(
                mockParser.Object,
                mockAppContextFactory.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockOptionsAccessor.Object);

            // Act
            int result = commandLineApp.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }

        [Fact]
        public void Run_RunsDefaultCommandIfCommandLineArgumentsdoNotSpecifyACommand()
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

            Mock<ICommandDictionary> mockCommandDictionary = _mockRepository.Create<ICommandDictionary>();
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(mockCommand.Object);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(mockCommandDictionary.Object);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, mockCommandDictionary.Object)).Returns(dummyParseResult);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(mockCommandDictionary.Object, dummyAppOptions)).Returns(dummyAppContext);

            CommandLineApp commandLineApp = CreateCommandLineApp(
                mockParser.Object,
                mockAppContextFactory.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockOptionsAccessor.Object);

            // Act
            int result = commandLineApp.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }

        public CommandLineApp CreateCommandLineApp(
            IParser parser = null,
            ICommandLineAppContextFactory commandLineAppContextFactory = null,
            ICommandDictionaryFactory commandDictionaryFactory = null,
            IEnumerable<ICommand> commands = null,
            IOptions<CommandLineAppOptions> optionsAccessor = null)
        {
            return new CommandLineApp(
                parser ?? _mockRepository.Create<IParser>().Object,
                commandLineAppContextFactory ?? _mockRepository.Create<ICommandLineAppContextFactory>().Object,
                commandDictionaryFactory ?? _mockRepository.Create<ICommandDictionaryFactory>().Object,
                commands,
                optionsAccessor);
        }
    }
}
