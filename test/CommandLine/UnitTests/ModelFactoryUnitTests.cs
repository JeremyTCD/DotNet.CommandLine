using JeremyTCD.DotNetCore.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class ModelFactoryUnitTests
    {
        private MockRepository _mockRepository { get; } = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
        private CommandFactory _commandFactory { get; }
        private OptionFactory _optionFactory { get; }

        public ModelFactoryUnitTests()
        {
            _optionFactory = new OptionFactory();
            _commandFactory = new CommandFactory(_optionFactory);
        }

        [Fact]
        public void Create_ThrowsParseExceptionIfAnArgumentOptionDoesNotExist()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, null } });
            Command dummyCommand = _commandFactory.CreateFromType(typeof(DummyModel));
            DummyModel dummyModel = new DummyModel();

            Mock<IActivatorService> mockActivatorService = _mockRepository.Create<IActivatorService>();
            mockActivatorService.Setup(a => a.CreateInstance(dummyCommand.ModelType)).Returns(dummyModel);

            //Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            //mockMapper.Setup(m => m.TryMap(dummyCommand.Options.First().PropertyInfo, ))

            ModelFactory modelFactory = new ModelFactory(mockActivatorService.Object, null);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => modelFactory.Create(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.Exception_OptionDoesNotExist, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Create_ThrowsParseExceptionIfAnExceptionIsThrownWhileMapping()
        {
            // Arrange
            string dummyOptionKey = DummyStrings.OptionShortName_Dummy;
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            Command dummyCommand = _commandFactory.CreateFromType(typeof(DummyModel));
            DummyModel dummyModel = new DummyModel();

            Mock<IActivatorService> mockActivatorService = _mockRepository.Create<IActivatorService>();
            mockActivatorService.Setup(a => a.CreateInstance(dummyCommand.ModelType)).Returns(dummyModel);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyCommand.Options.First().PropertyInfo, dummyOptionValue, dummyModel)).Throws<Exception>();

            ModelFactory modelFactory = new ModelFactory(mockActivatorService.Object, new[] { mockMapper.Object });

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => modelFactory.Create(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.Exception_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Create_ThrowsParseExceptionIfNoMapperCanHandleAnArgumentOptionValue()
        {
            // Arrange
            string dummyOptionKey = DummyStrings.OptionShortName_Dummy;
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            Command dummyCommand = _commandFactory.CreateFromType(typeof(DummyModel));
            DummyModel dummyModel = new DummyModel();

            Mock<IActivatorService> mockActivatorService = _mockRepository.Create<IActivatorService>();
            mockActivatorService.Setup(a => a.CreateInstance(dummyCommand.ModelType)).Returns(dummyModel);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyCommand.Options.First().PropertyInfo, dummyOptionValue, dummyModel)).Returns(false);

            ModelFactory modelFactory = new ModelFactory(mockActivatorService.Object, new[] { mockMapper.Object });

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => modelFactory.Create(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.Exception_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Create_CreatesACommandsModelAndMapsArgumentOptionsToItsProperties()
        {
            // Arrange
            string dummyOptionKey = DummyStrings.OptionShortName_Dummy;
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            Command dummyCommand = _commandFactory.CreateFromType(typeof(DummyModel));
            DummyModel dummyModel = new DummyModel();

            Mock<IActivatorService> mockActivatorService = _mockRepository.Create<IActivatorService>();
            mockActivatorService.Setup(a => a.CreateInstance(dummyCommand.ModelType)).Returns(dummyModel);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyCommand.Options.First().PropertyInfo, dummyOptionValue, dummyModel)).Returns(true);

            ModelFactory modelFactory = new ModelFactory(mockActivatorService.Object, new[] { mockMapper.Object });

            // Act
            DummyModel result = modelFactory.Create(dummyArguments, dummyCommand) as DummyModel;

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyModel, result);
        }

        [Command(typeof(DummyStrings), nameof(DummyStrings.CommandName_Dummy), nameof(DummyStrings.CommandDescription_Dummy))]
        private class DummyModel
        {
            [Option(typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy),
                nameof(DummyStrings.OptionLongName_Dummy), 
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyProperty { get; set; }
        }
    }
}
