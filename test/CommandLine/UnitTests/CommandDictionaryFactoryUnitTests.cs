// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandDictionaryFactoryUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        // TODO codeanalysis verify test method name corresponds to test under class member
        // TODO codeanalysis if test under class throws exceptions, verify each exception tested with expected test method name
        [Fact]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfThereAreMultipleDefaultCommands()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";

            Mock<ICommand> mockCommand1 = _mockRepository.Create<ICommand>();
            mockCommand1.Setup(d => d.IsDefault).Returns(true);
            mockCommand1.Setup(d => d.Name).Returns(dummyCommand1Name);

            Mock<ICommand> mockCommand2 = _mockRepository.Create<ICommand>();
            mockCommand2.Setup(d => d.IsDefault).Returns(true);
            mockCommand2.Setup(d => d.Name).Returns(dummyCommand2Name);

            ICommand[] dummyCommands = new[] { mockCommand1.Object, mockCommand2.Object };

            CommandDictionaryFactory commandDictionaryFactory = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            // TODO codeanalysis verifyall required if setup called
            _mockRepository.VerifyAll();
            Assert.Equal(
                string.Format(Strings.Exception_MultipleDefaultCommands, $"\t{dummyCommand1Name}{Environment.NewLine}\t{dummyCommand2Name}"),
                exception.Message);
        }

        [Theory]
        [MemberData(nameof(ThrowsInvalidOperationExceptionIfACommandNameIsNullOrWhitespaceData))]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfACommandsNameIsNullOrWhitespace(string dummyName)
        {
            // Arrange
            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            mockCommand.Setup(m => m.Name).Returns(dummyName);
            mockCommand.Setup(m => m.IsDefault).Returns(true);

            ICommand[] dummyCommands = new[] { mockCommand.Object };

            CommandDictionaryFactory commandDictionaryFactory = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(Strings.Exception_CommandsMustHaveNames, exception.Message);
            _mockRepository.VerifyAll();
        }

        // TODO codeanalysis data method name
        public static IEnumerable<object[]> ThrowsInvalidOperationExceptionIfACommandNameIsNullOrWhitespaceData()
        {
            yield return new object[] { null };
            yield return new object[] { " " };
            yield return new object[] { string.Empty };
        }

        [Fact]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfMultipleCommandsHaveTheSameName()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommandName, false);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommandName, false);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandDictionaryFactory commandDictionaryFactory = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(string.Format(Strings.Exception_MultipleCommandsWithSameName, dummyCommandName), exception.Message);
        }

        [Fact]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfThereIsNoDefaultCommand()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommand1Name, false);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommand2Name, false);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandDictionaryFactory commandDictionaryFactory = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(Strings.Exception_DefaultCommandRequired, exception.Message);
        }

        [Fact]
        public void CreateFromCommands_CreatesCommandDictionary()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommand1Name, true);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommand2Name, false);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandDictionaryFactory commandDictionaryFactory = CreateCommandDictionaryFactory();

            // Act
            ICommandDictionary result = commandDictionaryFactory.CreateFromCommands(dummyCommands);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(dummyCommand1, result[dummyCommand1Name]);
            Assert.Equal(dummyCommand2, result[dummyCommand2Name]);
        }

        private CommandDictionaryFactory CreateCommandDictionaryFactory()
        {
            return new CommandDictionaryFactory();
        }

        private class DummyCommand : ICommand
        {
            public DummyCommand(string name, bool isDefault)
            {
                Name = name;
                IsDefault = isDefault;
            }

            public string Name { get; }

            public string Description => throw new NotImplementedException();

            public bool IsDefault { get; }

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}