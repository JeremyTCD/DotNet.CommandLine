using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionFactoryIntegrationTests
    {
        [Fact]
        public void CreateFromAttribute_CreatesOptionFromOptionAttribute()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyModel).
                GetRuntimeProperties().
                First();
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.
                GetCustomAttribute<OptionAttribute>();

            OptionFactory optionFactory = new OptionFactory();

            // Act
            Option option = optionFactory.CreateFromAttribute(dummyOptionAttribute, dummyPropertyInfo);

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
        }
    }
}
