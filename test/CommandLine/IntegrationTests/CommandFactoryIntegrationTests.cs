using Moq;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandFactoryIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void TryCreateFromType_ReturnsNullIfTypeDoesNotContainCommandAttribute()
        {
            // Arrange
            Type dummyModelType = typeof(DummyNoAttributeModel);
            CommandFactory commandFactory = new CommandFactory(null);

            // Act
            Command command = commandFactory.TryCreateFromType(dummyModelType);

            // Assert
            Assert.Null(command);
        }

        [Fact]
        public void TryCreateFromType_CreatesCommandIfSuccessful()
        {
            // Arrange
            Type dummyCommandModelType = typeof(DummyModel);
            PropertyInfo dummyPropertyInfo = dummyCommandModelType.GetProperty(nameof(DummyModel.DummyProperty));
            Option dummyOption = new Option(null, null, null, null);

            Mock<IOptionFactory> mockOptionFactory = _mockRepository.Create<IOptionFactory>();
            mockOptionFactory.Setup(o => o.TryCreateFromPropertyInfo(dummyPropertyInfo)).Returns(dummyOption);

            CommandFactory commandFactory = new CommandFactory(mockOptionFactory.Object);

            // Act
            Command result = commandFactory.TryCreateFromType(dummyCommandModelType);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(DummyStrings.CommandName_Dummy, result.Name);
            Assert.Equal(DummyStrings.CommandDescription_Dummy, result.Description);
            Assert.True(result.IsDefault);
            Assert.Equal(typeof(DummyModel), result.ModelType);
            Assert.Single(result.Options);
            Assert.Equal(dummyOption, result.Options.First());
        }

        private class DummyNoAttributeModel
        {
        }

        [Command(typeof(DummyStrings), nameof(DummyStrings.CommandName_Dummy), nameof(DummyStrings.CommandDescription_Dummy), true)]
        private class DummyModel
        {
            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy),
                nameof(DummyStrings.OptionLongName_Dummy),
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyProperty { get; }
        }
    }
}
