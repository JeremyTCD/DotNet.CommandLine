using JeremyTCD.DotNet.CommandLine.src;
using JeremyTCD.DotNetCore.Utils;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandModelFactoryIntegrationTests
    {
        private MockRepository _mockRepository { get; } = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
        private CommandFactory _commandFactory { get; }
        private OptionFactory _optionFactory { get; }

        public CommandModelFactoryIntegrationTests()
        {
            _optionFactory = new OptionFactory();
            _commandFactory = new CommandFactory(_optionFactory);
        }

        [Fact]
        public void Create_CreatesModelThatRequiresNoCasting()
        {
            // Arrange
            string dummyMultipleValues = "1,2,3";
            string dummySingleValue = "dummySingleValue";

            Dictionary<string, string> options = new Dictionary<string, string> {
                { DummyStrings.OptionShortName_NoValue, null},
                { DummyStrings.OptionShortName_MultipleValues, dummyMultipleValues },
                { DummyStrings.OptionShortName_SingleValue, dummySingleValue }
            };
            Arguments arguments = new Arguments(null, options);

            Mock<ILoggingService<ActivatorService>> mockASLS = _mockRepository.Create<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);

            Command dummyCommand = _commandFactory.
                CreateFromAttribute(typeof(DummyNoCastingModel).GetTypeInfo().GetCustomAttribute<CommandAttribute>(), typeof(DummyNoCastingModel));
            CommandModelFactory modelFactory = new CommandModelFactory(activatorService);

            // Act
            DummyNoCastingModel result = modelFactory.Create(arguments, dummyCommand) as DummyNoCastingModel;

            // Assert
            Assert.True(result.NoValue);
            Assert.Equal(dummyMultipleValues.Split(',').ToList(), result.MultipleValues);
            Assert.Equal(dummySingleValue, result.SingleValue);
        }

        [Fact]
        public void Create_CreatesModelThatRequiresCasting()
        {
            // Arrange
            string dummyMultipleValues = "1,2,3";
            string dummySingleValue = "2.2";

            Dictionary<string, string> options = new Dictionary<string, string> {
                { DummyStrings.OptionShortName_MultipleValues, dummyMultipleValues },
                { DummyStrings.OptionShortName_SingleValue, dummySingleValue }
            };
            Arguments arguments = new Arguments(null, options);

            Mock<ILoggingService<ActivatorService>> mockASLS = _mockRepository.Create<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);

            Command dummyCommand = _commandFactory.
                CreateFromAttribute(typeof(DummyCastingRequiredModel).GetTypeInfo().GetCustomAttribute<CommandAttribute>(), typeof(DummyCastingRequiredModel));
            CommandModelFactory modelFactory = new CommandModelFactory(activatorService);

            // Act
            DummyCastingRequiredModel result = modelFactory.Create(arguments, dummyCommand) as DummyCastingRequiredModel;

            // Assert
            Assert.Equal(new List<int> { 1, 2, 3 }, result.MultipleValues);
            Assert.Equal(2.2, result.SingleValue);
        }

        [Fact]
        public void Create_IgnoresPropertiesThatAreNotOptions()
        {
            // Arrange
            Mock<ILoggingService<ActivatorService>> mockASLS = _mockRepository.Create<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);

            CommandModelFactory modelFactory = new CommandModelFactory(activatorService);

            Command dummyCommand = _commandFactory.
                CreateFromAttribute(typeof(DummyNoOptionsModel).GetTypeInfo().GetCustomAttribute<CommandAttribute>(), typeof(DummyNoOptionsModel));

            // Act
            DummyNoOptionsModel result = modelFactory.
                Create(new Arguments(null, new Dictionary<string, string>()), dummyCommand) as DummyNoOptionsModel;

            // Assert
            Assert.Null(result.NonOption);
        }

        [Fact]
        public void Create_IgnoresOptionsThatHaveNoCorrespondingArgument()
        {
            // Arrange
            Mock<ILoggingService<ActivatorService>> mockASLS = _mockRepository.Create<ILoggingService<ActivatorService>>();
            IActivatorService activatorService = new ActivatorService(mockASLS.Object);

            CommandModelFactory modelFactory = new CommandModelFactory(activatorService);

            Command dummyCommand = _commandFactory.
                CreateFromAttribute(typeof(DummyNoCastingModel).GetTypeInfo().GetCustomAttribute<CommandAttribute>(), typeof(DummyNoCastingModel));

            // Act
            DummyNoCastingModel result = modelFactory.
                Create(new Arguments(null, new Dictionary<string, string>()), dummyCommand) as DummyNoCastingModel;

            // Assert
            Assert.False(result.NoValue);
            Assert.Null(result.MultipleValues);
            Assert.Null(result.SingleValue);
        }

        private class DummyNoOptionsModel
        {
            public string NonOption { get; set; }
        }

        private class DummyCastingRequiredModel
        {
            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_MultipleValues),
                nameof(DummyStrings.OptionLongName_MultipleValues))]
            public List<int> MultipleValues { get; set; }

            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_SingleValue),
                nameof(DummyStrings.OptionLongName_SingleValue))]
            public double SingleValue { get; set; }
        }

        private class DummyNoCastingModel
        {
            [Option(typeof(DummyStrings), 
                nameof(DummyStrings.OptionShortName_NoValue),
                nameof(DummyStrings.OptionLongName_NoValue))]
            public bool NoValue { get; set; }

            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_MultipleValues),
                nameof(DummyStrings.OptionLongName_MultipleValues))]
            public List<string> MultipleValues { get; set; }

            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_SingleValue),
                nameof(DummyStrings.OptionLongName_SingleValue))]
            public string SingleValue { get; set; }
        }
    }
}
