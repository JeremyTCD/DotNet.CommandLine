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
            PropertyInfo dummyPropertyInfo = typeof(DummyModel).
                GetRuntimeProperties().
                First();
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.
                GetCustomAttribute<OptionAttribute>();

            OptionFactory optionMetadataFactory = new OptionFactory();

            // Act
            Option optionMetadata = optionMetadataFactory.CreateFromAttribute(dummyOptionAttribute, dummyPropertyInfo);

            // Assert
            Assert.Equal(DummyStrings.OptionShortName_Dummy, optionMetadata.ShortName);
            Assert.Equal(DummyStrings.OptionLongName_Dummy, optionMetadata.LongName);
            Assert.Equal(DummyStrings.OptionDescription_Dummy, optionMetadata.Description);
            Assert.Equal(dummyPropertyInfo, optionMetadata.PropertyInfo);
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
