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

            Mock<IParseResult> mockParseResult = _mockRepository.Create<IParseResult>();
            mockParseResult.Setup(p => p.Command).Returns(mockCommand.Object);

            mockCommand.Setup(c => c.Run(mockParseResult.Object, dummyCommandLineAppContext.Object)).Returns(dummyExitCode);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.Create(dummyCommands)).Returns(dummyCommandDictionary.Object);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandDictionary.Object)).Returns(mockParseResult.Object);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(dummyCommandDictionary.Object, dummyAppOptions)).Returns(dummyCommandLineAppContext.Object);

            CommandLineApp testSubject = CreateCommandLineApp(
                mockParser.Object,
                mockAppContextFactory.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockOptionsAccessor.Object);

            // Act
            int result = testSubject.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }

        [Fact]
        public void Run_RunsDefaultCommandIfCommandLineArgumentsDoNotSpecifyACommand()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            string[] dummyArgs = new string[0];
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions();
            int dummyExitCode = 1;

            Mock<IParseResult> dummyParseResult = _mockRepository.Create<IParseResult>();

            Mock<ICommandLineAppContext> dummyAppContext = _mockRepository.Create<ICommandLineAppContext>();

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            mockCommand.Setup(c => c.Run(dummyParseResult.Object, dummyAppContext.Object)).Returns(dummyExitCode);

            Mock<ICommandDictionary> mockCommandDictionary = _mockRepository.Create<ICommandDictionary>();
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(mockCommand.Object);

            Mock<ICommandDictionaryFactory> mockCommandDictionaryFactory = _mockRepository.Create<ICommandDictionaryFactory>();
            mockCommandDictionaryFactory.Setup(c => c.Create(dummyCommands)).Returns(mockCommandDictionary.Object);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, mockCommandDictionary.Object)).Returns(dummyParseResult.Object);

            Mock<IOptions<CommandLineAppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<CommandLineAppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<ICommandLineAppContextFactory> mockAppContextFactory = _mockRepository.Create<ICommandLineAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(mockCommandDictionary.Object, dummyAppOptions)).Returns(dummyAppContext.Object);

            CommandLineApp testSubject = CreateCommandLineApp(
                mockParser.Object,
                mockAppContextFactory.Object,
                mockCommandDictionaryFactory.Object,
                dummyCommands,
                mockOptionsAccessor.Object);

            // Act
            int result = testSubject.Run(dummyArgs);

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
                parser,
                commandLineAppContextFactory,
                commandDictionaryFactory,
                commands,
                optionsAccessor);
        }
    }
}
