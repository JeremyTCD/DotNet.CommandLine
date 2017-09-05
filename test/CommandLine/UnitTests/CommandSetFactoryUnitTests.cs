using Moq;
using System;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.IntegrationTests
{
    public class CommandSetFactoryUnitTests
    {
        private MockRepository _mockRepository { get; } = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateFromTypes_ThrowsInvalidOperationExceptionIfATypeDoesNotHaveACommandAttribute()
        {
            // Arrange
            CommandSetFactory commandSetFactory = new CommandSetFactory(null);

            // Act and Assert
            InvalidOperationException exception = Assert.
                Throws<InvalidOperationException>(() => commandSetFactory.CreateFromTypes(new[] { typeof(DummyNoCommandAttributeModel) }));
            Assert.Equal(string.Format(Strings.Exception_TypeDoesNotHaveCommandAttribute, nameof(DummyNoCommandAttributeModel)), exception.Message);
        }

        [Fact]
        public void CreateFromTypes_ThrowsInvalidOperationExceptionIfThereAreMultipleDefaultCommands()
        {
            // Arrange
            Type dummyType = typeof(DummyModel);
            Type[] dummyTypes = new[] { dummyType, dummyType };

            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            Command dummyCommand1 = new Command(null, true, dummyCommand1Name, null, null);
            Command dummyCommand2 = new Command(null, true, dummyCommand2Name, null, null);
            Mock<ICommandFactory> mockCommandFactory = _mockRepository.Create<ICommandFactory>();
            mockCommandFactory.
                SetupSequence(c => c.TryCreateFromType(dummyType)).
                Returns(dummyCommand1).
                Returns(dummyCommand2);

            CommandSetFactory commandSetFactory = new CommandSetFactory(mockCommandFactory.Object);

            // Act and Assert
            InvalidOperationException exception = Assert.
                Throws<InvalidOperationException>(() => commandSetFactory.CreateFromTypes(dummyTypes));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.Exception_MultipleDefaultCommands, $"\t{dummyCommand1Name}{Environment.NewLine}\t{dummyCommand2Name}"),
                exception.Message);
        }

        [Fact]
        public void CreateFromTypes_ThrowsInvalidOperationExceptionIfMultipleCommandsHaveTheSameName()
        {
            // Arrange
            Type dummyType = typeof(DummyModel);
            Type[] dummyTypes = new[] { dummyType, dummyType };

            string dummyCommandName = "dummyCommandName";
            Command dummyCommand1 = new Command(null, true, dummyCommandName, null, null);
            Command dummyCommand2 = new Command(null, false, dummyCommandName, null, null);
            Mock<ICommandFactory> mockCommandFactory = _mockRepository.Create<ICommandFactory>();
            mockCommandFactory.
                SetupSequence(c => c.TryCreateFromType(dummyType)).
                Returns(dummyCommand1).
                Returns(dummyCommand2);

            CommandSetFactory commandSetFactory = new CommandSetFactory(mockCommandFactory.Object);

            // Act and Assert
            InvalidOperationException exception = Assert.
                Throws<InvalidOperationException>(() => commandSetFactory.CreateFromTypes(dummyTypes));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.Exception_MultipleCommandsWithSameName, dummyCommandName),
                exception.Message);
        }

        [Fact]
        public void CreateFromTypes_CreatesCommandSetFromTypes()
        {
            // Arrange
            Type dummyType = typeof(DummyModel);
            Type[] dummyTypes = new[] { dummyType, dummyType };

            string dummyCommand1Name = "dummyCommand1Name";
            string dummyCommand2Name = "dummyCommand2Name";
            Command dummyCommand1 = new Command(null, true, dummyCommand1Name, null, null);
            Command dummyCommand2 = new Command(null, false, dummyCommand2Name, null, null);
            Mock<ICommandFactory> mockCommandFactory = _mockRepository.Create<ICommandFactory>();
            mockCommandFactory.
                SetupSequence(c => c.TryCreateFromType(dummyType)).
                Returns(dummyCommand1).
                Returns(dummyCommand2);

            CommandSetFactory commandSetFactory = new CommandSetFactory(mockCommandFactory.Object);

            // Act
            CommandSet result = commandSetFactory.CreateFromTypes(dummyTypes);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(2, result.Count);
            Assert.Contains(dummyCommand1, result.Values);
            Assert.Contains(dummyCommand2, result.Values);
        }

        [Command(typeof(DummyStrings), nameof(DummyStrings.CommandName_Dummy), nameof(DummyStrings.CommandDescription_Dummy))]
        private class DummyModel
        {
        }

        private class DummyNoCommandAttributeModel
        {
        }
    }
}