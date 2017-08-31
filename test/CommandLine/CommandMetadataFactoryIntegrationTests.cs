using Moq;
using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandMetadataFactoryIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateFromAttribute_CreatesCommandMetadataFromCommandAttribute()
        {
            // Arrange
            CommandAttribute dummyCommandAttribute = typeof(DummyModel).GetTypeInfo().GetCustomAttribute<CommandAttribute>();
            OptionAttribute dummyOptionAttribute = typeof(DummyModel).GetRuntimeProperties().First().GetCustomAttribute<OptionAttribute>();

            OptionMetadata dummyOptionMetadata = new OptionMetadata(null, null, null);
            Mock<IOptionMetadataFactory> mockOptionMetadataFactory = _mockRepository.Create<IOptionMetadataFactory>();
            mockOptionMetadataFactory.Setup(o => o.CreateFromAttribute(dummyOptionAttribute)).Returns(dummyOptionMetadata);

            CommandMetadataFactory commandMetadataFactory = new CommandMetadataFactory(mockOptionMetadataFactory.Object);

            // Act
            CommandMetadata result = commandMetadataFactory.CreateFromAttribute(dummyCommandAttribute);

            // Assert
            Assert.Equal(DummyStrings.CommandName_Dummy, result.Name);
            Assert.Equal(DummyStrings.CommandDescription_Dummy, result.Description);
            Assert.True(result.IsDefault);
            Assert.Equal(typeof(DummyModel), result.CommandModelType);
            Assert.Single(result.OptionMetadata);
            Assert.Equal(dummyOptionMetadata, result.OptionMetadata.First());
        }

        [Command(typeof(DummyModel), typeof(DummyStrings), nameof(DummyStrings.CommandName_Dummy), nameof(DummyStrings.CommandDescription_Dummy), true)]
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
