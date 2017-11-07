using Moq;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionCollectionFactoryUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default);
         
        [Fact]
        public void Create_CreatesOptionCollectionFromCommand()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand();
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            Mock<IOption> dummyOption = _mockRepository.Create<IOption>();

            Mock<IOptionFactory> mockOptionFactory = _mockRepository.Create<IOptionFactory>();
            mockOptionFactory.Setup(o => o.TryCreate(dummyPropertyInfo)).Returns(dummyOption.Object);

            OptionCollectionFactory testSubject = CreateOptionCollectionFactory(mockOptionFactory.Object);

            // Act
            IOptionCollection result = testSubject.Create(dummyCommand);

            // Assert
            Assert.Single(result);
            Assert.Same(dummyOption.Object, result.Single());
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_RetrievesCachedOptionCollectionIfItExists()
        {
            // Arrange
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<IOptionFactory> mockOptionFactory = _mockRepository.Create<IOptionFactory>();
            mockOptionFactory.Setup(o => o.TryCreate(It.IsAny<PropertyInfo>())).Returns((IOption)null);

            OptionCollectionFactory testSubject = CreateOptionCollectionFactory(mockOptionFactory.Object);

            // Act
            IOptionCollection result1 = testSubject.Create(dummyCommand.Object);
            IOptionCollection result2 = testSubject.Create(dummyCommand.Object);

            // Assert
            Assert.Same(result1, result2);
            _mockRepository.VerifyAll();
        }

        private Mock<OptionCollectionFactory> CreateMockOptionCollectionFactory(IOptionFactory optionFactory = null)
        {
            return _mockRepository.Create<OptionCollectionFactory>(optionFactory);
        }

        private OptionCollectionFactory CreateOptionCollectionFactory(IOptionFactory optionFactory = null)
        {
            return new OptionCollectionFactory(optionFactory);
        }


        private class DummyCommand : ICommand
        {
            [Option(
                typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy),
                nameof(DummyStrings.OptionLongName_Dummy),
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyOptionProperty { get; }

            public string Name => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public bool IsDefault => throw new NotImplementedException();

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}