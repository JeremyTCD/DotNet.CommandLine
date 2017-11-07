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
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Create_ThrowsInvalidOperationExceptionIfThereAreMultipleDefaultCommands()
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

            CommandDictionaryFactory testSubject = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => testSubject.Create(dummyCommands));
            _mockRepository.VerifyAll();
            Assert.Equal(
                string.Format(Strings.Exception_MultipleDefaultCommands, $"\t{dummyCommand1Name}{Environment.NewLine}\t{dummyCommand2Name}"),
                exception.Message);
        }

        [Theory]
        [MemberData(nameof(Create_ThrowsInvalidOperationExceptionIfAnICommandsNamePropertyIsNullOrWhitespace_Data))]
        public void Create_ThrowsInvalidOperationExceptionIfAnICommandsNamePropertyIsNullOrWhitespace(string dummyName)
        {
            // Arrange
            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            mockCommand.Setup(m => m.Name).Returns(dummyName);
            mockCommand.Setup(m => m.IsDefault).Returns(true);

            ICommand[] dummyCommands = new[] { mockCommand.Object };

            CommandDictionaryFactory testSubject = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => testSubject.Create(dummyCommands));
            Assert.Equal(Strings.Exception_CommandsMustHaveNames, exception.Message);
            _mockRepository.VerifyAll();
        }

        public static IEnumerable<object[]> Create_ThrowsInvalidOperationExceptionIfAnICommandsNamePropertyIsNullOrWhitespace_Data()
        {
            yield return new object[] { null };
            yield return new object[] { " " };
            yield return new object[] { string.Empty };
        }

        [Fact]
        public void Create_ThrowsInvalidOperationExceptionIfMultipleCommandsHaveTheSameName()
        {
            // Arrange
            string dummyCommandName = "dummyCommandName";

            Mock<ICommand> mockCommand1 = _mockRepository.Create<ICommand>();
            mockCommand1.Setup(d => d.IsDefault).Returns(false);
            mockCommand1.Setup(d => d.Name).Returns(dummyCommandName);

            Mock<ICommand> mockCommand2 = _mockRepository.Create<ICommand>();
            mockCommand2.Setup(d => d.IsDefault).Returns(false);
            mockCommand2.Setup(d => d.Name).Returns(dummyCommandName);

            ICommand[] dummyCommands = new[] { mockCommand1.Object, mockCommand2.Object};

            CommandDictionaryFactory testSubject = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => testSubject.Create(dummyCommands));
            Assert.Equal(string.Format(Strings.Exception_MultipleCommandsWithSameName, dummyCommandName), exception.Message);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_ThrowsInvalidOperationExceptionIfThereIsNoDefaultCommand()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";

            Mock<ICommand> mockCommand1 = _mockRepository.Create<ICommand>();
            mockCommand1.Setup(d => d.IsDefault).Returns(false);
            mockCommand1.Setup(d => d.Name).Returns(dummyCommand1Name);

            Mock<ICommand> mockCommand2 = _mockRepository.Create<ICommand>();
            mockCommand2.Setup(d => d.IsDefault).Returns(false);
            mockCommand2.Setup(d => d.Name).Returns(dummyCommand2Name);

            ICommand[] dummyCommands = new[] { mockCommand1.Object, mockCommand2.Object };

            CommandDictionaryFactory testSubject = CreateCommandDictionaryFactory();

            // Act and Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => testSubject.Create(dummyCommands));
            Assert.Equal(Strings.Exception_DefaultCommandRequired, exception.Message);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_CreatesCommandDictionary()
        {
            // Arrange
            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";

            Mock<ICommand> mockCommand1 = _mockRepository.Create<ICommand>();
            mockCommand1.Setup(d => d.IsDefault).Returns(true);
            mockCommand1.Setup(d => d.Name).Returns(dummyCommand1Name);

            Mock<ICommand> mockCommand2 = _mockRepository.Create<ICommand>();
            mockCommand2.Setup(d => d.IsDefault).Returns(false);
            mockCommand2.Setup(d => d.Name).Returns(dummyCommand2Name);

            ICommand[] dummyCommands = new[] { mockCommand1.Object, mockCommand2.Object };

            CommandDictionaryFactory testSubject = CreateCommandDictionaryFactory();

            // Act
            ICommandDictionary result = testSubject.Create(dummyCommands);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(mockCommand1.Object, result[dummyCommand1Name]);
            Assert.Equal(mockCommand2.Object, result[dummyCommand2Name]);
            _mockRepository.VerifyAll();
        }

        private CommandDictionaryFactory CreateCommandDictionaryFactory()
        {
            return new CommandDictionaryFactory();
        }
    }
}