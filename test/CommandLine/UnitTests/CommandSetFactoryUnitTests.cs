// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandSetFactoryUnitTests
    {
        private MockRepository _mockRepository { get; } = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfThereAreMultipleDefaultCommands()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommand1Name, true);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommand2Name, true);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandSetFactory commandSetFactory = new CommandSetFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandSetFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(string.Format(Strings.Exception_MultipleDefaultCommands, $"\t{dummyCommand1Name}{Environment.NewLine}\t{dummyCommand2Name}"),
                exception.Message);
        }

        [Theory]
        [MemberData(nameof(ThrowsInvalidOperationExceptionIfACommandNameIsNullOrWhitespaceData))]
        public void CreateFromCommands_ThrowsInvalidOperationExceptionIfACommandsNameIsNullOrWhitespace(string dummyName)
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand(dummyName, true);
            DummyCommand[] dummyCommands = new[] { dummyCommand };

            CommandSetFactory commandSetFactory = new CommandSetFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandSetFactory.CreateFromCommands(dummyCommands));
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

            CommandSetFactory commandSetFactory = new CommandSetFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandSetFactory.CreateFromCommands(dummyCommands));
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

            CommandSetFactory commandSetFactory = new CommandSetFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => commandSetFactory.CreateFromCommands(dummyCommands));
            Assert.Equal(Strings.Exception_DefaultCommandRequired, exception.Message);
        }

        [Fact]
        public void CreateFromCommands_CreatesCommandSet()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            DummyCommand dummyCommand1 = new DummyCommand(dummyCommand1Name, true);
            DummyCommand dummyCommand2 = new DummyCommand(dummyCommand2Name, false);
            DummyCommand[] dummyCommands = new[] { dummyCommand1, dummyCommand2 };

            CommandSetFactory commandSetFactory = new CommandSetFactory();

            // Act 
            CommandSet result = commandSetFactory.CreateFromCommands(dummyCommands);

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

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}