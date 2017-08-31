using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionMetadataFactoryIntegrationTests
    {
        [Fact]
        public void CreateFromAttribute_CreatesOptionMetadataFromOptionAttribute()
        {
            // Arrange
            OptionAttribute dummyOptionAttribute = typeof(DummyModel).
                GetRuntimeProperties().
                First().
                GetCustomAttribute<OptionAttribute>();

            OptionMetadataFactory optionMetadataFactory = new OptionMetadataFactory();

            // Act
            OptionMetadata optionMetadata = optionMetadataFactory.CreateFromAttribute(dummyOptionAttribute);

            // Assert
            Assert.Equal(DummyStrings.OptionShortName_Dummy, optionMetadata.ShortName);
            Assert.Equal(DummyStrings.OptionLongName_Dummy, optionMetadata.LongName);
            Assert.Equal(DummyStrings.OptionDescription_Dummy, optionMetadata.Description);
        }

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
