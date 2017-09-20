// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandDictionaryFactoryUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfThereAreMultipleDefaultCommands()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommand1Name, true);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommand2Name, true);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandDictionaryFactory commandDictionaryFactory = new CommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(
                string.Format(Strings.Exception_MultipleDefaultCommands, $"\t{dummyCommand1Name}{Environment.NewLine}\t{dummyCommand2Name}"),
                exception.Message);
        }

        [Theory]
        [MemberData(nameof(ThrowsInvalidOperationExceptionIfACommandNameIsNullOrWhitespaceData))]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfACommandsNameIsNullOrWhitespace(string dummyName)
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand(dummyName, true);
            DummyCommand[] dummyCommands = new[] { dummyCommand };

            CommandDictionaryFactory commandDictionaryFactory = new CommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandDictionaryFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(Strings.Exception_CommandsMustHaveNames, exception.Message);
        }

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

            CommandDictionaryFactory commandDictionaryFactory = new CommandDictionaryFactory();

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

            CommandDictionaryFactory commandDictionaryFactory = new CommandDictionaryFactory();

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

            CommandDictionaryFactory commandDictionaryFactory = new CommandDictionaryFactory();

            // Act
            CommandDictionary result = commandDictionaryFactory.CreateFromCommands(dummyCommands);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(dummyCommand1, result[dummyCommand1Name]);
            Assert.Equal(dummyCommand2, result[dummyCommand2Name]);
        }

        private class DummyCommand : ICommand
        {
            public string Name { get; }

            public string Description => throw new NotImplementedException();

            public bool IsDefault { get; }

            public DummyCommand(string name, bool isDefault)
            {
                Name = name;
                IsDefault = isDefault;
            }

            public int Run(ParseResult parseResult, CommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}