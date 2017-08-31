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
        public void CreateFromAttribute_CreatesCommandFromCommandAttribute()
        {
            // Arrange
            Type dummyCommandModelType = typeof(DummyModel);
            CommandAttribute dummyCommandAttribute = dummyCommandModelType.GetTypeInfo().GetCustomAttribute<CommandAttribute>();
            PropertyInfo dummyOptionPropertyInfo = typeof(DummyModel).GetRuntimeProperties().First();
            OptionAttribute dummyOptionAttribute = dummyOptionPropertyInfo.GetCustomAttribute<OptionAttribute>();

            Option dummyOption = new Option(null, null, null, null);
            Mock<IOptionFactory> mockOptionFactory = _mockRepository.Create<IOptionFactory>();
            mockOptionFactory.Setup(o => o.CreateFromAttribute(dummyOptionAttribute, dummyOptionPropertyInfo)).Returns(dummyOption);

            CommandFactory commandFactory = new CommandFactory(mockOptionFactory.Object);

            // Act
            Command result = commandFactory.CreateFromAttribute(dummyCommandAttribute, dummyCommandModelType);

            // Assert
            Assert.Equal(DummyStrings.CommandName_Dummy, result.Name);
            Assert.Equal(DummyStrings.CommandDescription_Dummy, result.Description);
            Assert.True(result.IsDefault);
            Assert.Equal(typeof(DummyModel), result.CommandModelType);
            Assert.Single(result.Options);
            Assert.Equal(dummyOption, result.Options.First());
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
