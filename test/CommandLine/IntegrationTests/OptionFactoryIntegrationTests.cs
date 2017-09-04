using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionFactoryIntegrationTests
    {
        [Fact]
        public void TryCreateFromPropertyInfo_ReturnsNullIfPropertyInfoDoesNotContainOptionAttribute()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.DummyNoAttributeProperty));

            OptionFactory optionFactory = new OptionFactory();

            // Act
            Option option = optionFactory.TryCreateFromPropertyInfo(dummyPropertyInfo);

            // Assert
            Assert.Null(option);
        }

        [Fact]
        public void TryCreateFromPropertyInfo_CreatesOptionIfSuccessful()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyModel).GetProperty(nameof(DummyModel.DummyProperty));

            OptionFactory optionFactory = new OptionFactory();

            // Act
            Option option = optionFactory.TryCreateFromPropertyInfo(dummyPropertyInfo);

            // Assert
            Assert.Equal(DummyStrings.OptionShortName_Dummy, option.ShortName);
            Assert.Equal(DummyStrings.OptionLongName_Dummy, option.LongName);
            Assert.Equal(DummyStrings.OptionDescription_Dummy, option.Description);
            Assert.Equal(dummyPropertyInfo, option.PropertyInfo);
        }

        private class DummyModel
        {
            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy), 
                nameof(DummyStrings.OptionLongName_Dummy), 
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyProperty { get; }

            public string DummyNoAttributeProperty { get; }
        }
    }
}
