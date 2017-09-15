using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionsFactoryIntegrationTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void CreateFromCommand_CreatesOptionsFromCommandOrReturnsCachedOptionsIfTheyExist()
        {
            // Arrange
            DummyCommand dummyCommand = new DummyCommand();
            int numProperties = dummyCommand.GetType().GetProperties().Length;

            Mock<OptionsFactory> optionsFactory = _mockRepository.Create<OptionsFactory>();
            optionsFactory.CallBase = true;

            // Act
            List<Option> result1 = optionsFactory.Object.CreateFromCommand(dummyCommand);
            List<Option> result2 = optionsFactory.Object.CreateFromCommand(dummyCommand);

            // Assert
            Assert.Equal(result1, result2);
            optionsFactory.Verify(o => o.TryCreateFromPropertyInfo(It.IsAny<PropertyInfo>()), Times.Exactly(numProperties));
        }

        [Fact]
        public void TryCreateFromPropertyInfo_ReturnsNullIfPropertyInfoDoesNotContainOptionAttribute()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyNoAttributeProperty));

            OptionsFactory optionFactory = new OptionsFactory();

            // Act
            Option result = optionFactory.TryCreateFromPropertyInfo(dummyPropertyInfo);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryCreateFromPropertyInfo_CreatesOptionIfSuccessful()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));

            OptionsFactory optionFactory = new OptionsFactory();

            // Act
            Option result = optionFactory.TryCreateFromPropertyInfo(dummyPropertyInfo);

            // Assert
            Assert.Equal(DummyStrings.OptionShortName_Dummy, result.ShortName);
            Assert.Equal(DummyStrings.OptionLongName_Dummy, result.LongName);
            Assert.Equal(DummyStrings.OptionDescription_Dummy, result.Description);
            Assert.Equal(dummyPropertyInfo, result.PropertyInfo);
        }

        [Fact]
        public void TryCreateFromPropertyInfo_ThrowsInvalidOperationExceptionIfOptionAttributeHasNeitherALongNameOrAShortName()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommandWithNamelessProperty).GetProperty(nameof(DummyCommandWithNamelessProperty.DummyNamelessOptionProperty));

            OptionsFactory optionFactory = new OptionsFactory();

            // Act and Assert
            Exception exception = Assert.Throws<InvalidOperationException>(() => optionFactory.TryCreateFromPropertyInfo(dummyPropertyInfo));
            Assert.Equal(string.Format(Strings.Exception_OptionAttributeMustHaveName, nameof(DummyCommandWithNamelessProperty.DummyNamelessOptionProperty)), exception.Message);
        }

        private class DummyCommandWithNamelessProperty : ICommand
        {
            [Option()]
            public string DummyNamelessOptionProperty { get; }

            public string Name => throw new NotImplementedException();
            public string Description => throw new NotImplementedException();
            public bool IsDefault => throw new NotImplementedException();

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new NotImplementedException();
            }
        }

        private class DummyCommand : ICommand
        {
            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy), 
                nameof(DummyStrings.OptionLongName_Dummy), 
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyOptionProperty { get; }

            public string DummyNoAttributeProperty { get; }

            public string Name => throw new System.NotImplementedException();
            public string Description => throw new System.NotImplementedException();
            public bool IsDefault => throw new System.NotImplementedException();

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
