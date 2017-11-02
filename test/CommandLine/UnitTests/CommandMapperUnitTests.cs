// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandMapperUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Map_ThrowsParseExceptionIfArgumentAccessorContainsAnOptionThatDoesNotExist()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            List<Option> dummyOptions = new List<Option>();
            Dictionary<string, string> dummyOptionArgs = new Dictionary<string, string>() { { dummyOptionKey, null } };
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<IArgumentAccessor> mockArgumentAccessor = _mockRepository.Create<IArgumentAccessor>();
            mockArgumentAccessor.Setup(a => a.OptionArgs).Returns(dummyOptionArgs);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand.Object)).Returns(dummyOptions);

            CommandMapper testSubject = CreateCommandMapper(optionsFactory: mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => testSubject.Map(mockArgumentAccessor.Object, dummyCommand.Object));
            Assert.Equal(string.Format(Strings.ParseException_OptionDoesNotExist, dummyOptionKey), parseException.Message);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfAnExceptionIsThrownWhileMapping()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Dictionary<string, string> dummyOptionArgs = new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } };
            Exception dummyException = new Exception();
            Mock<PropertyInfo> dummyPropertyInfo = _mockRepository.Create<PropertyInfo>();
            Option dummyOption = new Option(dummyPropertyInfo.Object, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<IArgumentAccessor> mockArgumentAccessor = _mockRepository.Create<IArgumentAccessor>();
            mockArgumentAccessor.Setup(a => a.OptionArgs).Returns(dummyOptionArgs);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand.Object)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo.Object, dummyOptionValue, dummyCommand.Object)).Throws(dummyException);

            CommandMapper testSubject = CreateCommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => testSubject.Map(mockArgumentAccessor.Object, dummyCommand.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
            Assert.Equal(dummyException, parseException.InnerException);
        }

        [Fact]
        public void Map_ThrowsParseExceptionIfArgumentAccessorContainsAnInvalidOptionValue()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Dictionary<string, string> dummyOptionArgs = new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } };
            Mock<PropertyInfo> dummyPropertyInfo = _mockRepository.Create<PropertyInfo>();
            Option dummyOption = new Option(dummyPropertyInfo.Object, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<IArgumentAccessor> mockArgumentAccessor = _mockRepository.Create<IArgumentAccessor>();
            mockArgumentAccessor.Setup(a => a.OptionArgs).Returns(dummyOptionArgs);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand.Object)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo.Object, dummyOptionValue, dummyCommand.Object)).Returns(false);

            CommandMapper testSubject = CreateCommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act and Assert
            ParseException parseException = Assert.Throws<ParseException>(() => testSubject.Map(mockArgumentAccessor.Object, dummyCommand.Object));
            _mockRepository.VerifyAll();
            Assert.Equal(string.Format(Strings.ParseException_InvalidOptionValue, dummyOptionValue, dummyOptionKey), parseException.Message);
        }

        [Fact]
        public void Map_MapsArgumentOptionsToCommandsProperties()
        {
            // Arrange
            string dummyOptionKey = "dummyOptionKey";
            string dummyOptionValue = "dummyOptionValue";
            Dictionary<string, string> dummyOptionArgs = new Dictionary<string, string>() { { dummyOptionKey, dummyOptionValue } };
            Mock<PropertyInfo> dummyPropertyInfo = _mockRepository.Create<PropertyInfo>();
            Option dummyOption = new Option(dummyPropertyInfo.Object, dummyOptionKey, null, null);
            List<Option> dummyOptions = new List<Option> { dummyOption };
            Mock<ICommand> dummyCommand = _mockRepository.Create<ICommand>();

            Mock<IArgumentAccessor> mockArgumentAccessor = _mockRepository.Create<IArgumentAccessor>();
            mockArgumentAccessor.Setup(a => a.OptionArgs).Returns(dummyOptionArgs);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand.Object)).Returns(dummyOptions);

            Mock<IMapper> mockMapper = _mockRepository.Create<IMapper>();
            mockMapper.Setup(m => m.TryMap(dummyPropertyInfo.Object, dummyOptionValue, dummyCommand.Object)).Returns(true);

            CommandMapper testSubject = CreateCommandMapper(new IMapper[] { mockMapper.Object }, mockOptionsFactory.Object);

            // Act
            testSubject.Map(mockArgumentAccessor.Object, dummyCommand.Object);

            // Assert
            _mockRepository.VerifyAll();
        }

        private CommandMapper CreateCommandMapper(IEnumerable<IMapper> mappers = null, IOptionsFactory optionsFactory = null)
        {
            return new CommandMapper(mappers, optionsFactory);
        }
    }
}
