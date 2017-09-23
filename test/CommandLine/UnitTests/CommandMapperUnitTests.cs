// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandMapperUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Map_ThrowsParseExceptionIfAnArgumentOptionDoesNotExist()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, null } });
            List<Option> dummyOptions = new List<Option>();
            DummyCommand dummyCommand = new DummyCommand();

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand)).Returns(dummyOptions);

            CommandMapper commandMapper = new CommandMapper(null, mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => commandMapper.Map(dummyArguments, dummyCommand));
            Assert.Equal(string.Format(Strings.ParseException_OptionDoesNotExist, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfAnExceptionIsThrownWhileMapping()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            Exception dummyException = new Exception();
            DummyCommand dummyCommand = new DummyCommand();

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, dummyCommand)).Throws(dummyException);

            CommandMapper commandMapper = new CommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => commandMapper.Map(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
            Assert.Equal(dummyException, parseException.InnerException);
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfNoMapperCanHandleAnArgumentOptionValue()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            DummyCommand dummyCommand = new DummyCommand();

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, dummyCommand)).Returns(false);

            CommandMapper commandMapper = new CommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => commandMapper.Map(dummyArguments, dummyCommand));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Map_MapsArgumentOptionsToCommandsProperties()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Arguments dummyArguments = new Arguments(null, new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } });
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyProperty));
            Option dummyOption = new Option(dummyPropertyInfo, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            DummyCommand dummyCommand = new DummyCommand();

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo, dummyOptionValue, dummyCommand)).Returns(true);

            CommandMapper commandMapper = new CommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act
            commandMapper.Map(dummyArguments, dummyCommand);

            // Assert
            _mockRepository.VerifyAll();
        }

        private class DummyCommand : ICommand
        {
            public string Name => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public bool IsDefault => throw new NotImplementedException();

            [Option]
            public string DummyProperty { get; }

            public int Run(ParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
