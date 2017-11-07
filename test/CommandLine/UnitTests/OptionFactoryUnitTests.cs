// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Moq;
using System;
using System.Reflection;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class OptionFactoryUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { };

        [Fact]
        public void TryCreate_ReturnsNullIfPropertyInfoDoesNotContainAnOptionAttribute()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyNoAttributeProperty));

            OptionFactory testSubject = CreateOptionFactory();

            // Act
            IOption result = testSubject.TryCreate(dummyPropertyInfo);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TryCreate_CreatesOptionIfSuccessful()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();
            Mock<IOption> dummyOption = _mockRepository.Create<IOption>();

            Mock<OptionFactory> testSubject = CreateMockOptionFactory();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreateCore(dummyPropertyInfo, dummyOptionAttribute)).Returns(dummyOption.Object);

            // Act
            IOption result = testSubject.Object.TryCreate(dummyPropertyInfo);

            // Assert
            Assert.Same(dummyOption.Object, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void TryCreate_ThrowsInvalidOperationExceptionIfThePropertyInfosOptionAttributeHasNeitherALongNameNorAShortName()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();

            Mock<OptionFactory> testSubject = CreateMockOptionFactory();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreateCore(dummyPropertyInfo, dummyOptionAttribute)).Throws(new InvalidOperationException());

            // Act and assert
            Assert.Throws<InvalidOperationException>(() => testSubject.Object.TryCreate(dummyPropertyInfo));
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_ThrowsArgumentExceptionIfPropertyInfoHasNoOptionAttribute()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyNoAttributeProperty));

            OptionFactory testSubject = CreateOptionFactory();

            // Act and assert
            ArgumentException result = Assert.Throws<ArgumentException>(() => testSubject.Create(dummyPropertyInfo));
            Assert.Equal(string.Format(Strings.ArgumentException_PropertyInfoMustHaveOptionAttribute, dummyPropertyInfo.Name), result.Message);
        }

        [Fact]
        public void Create_CreatesOptionIfSuccessful()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();
            Mock<IOption> dummyOption = _mockRepository.Create<IOption>();

            Mock<OptionFactory> testSubject = CreateMockOptionFactory();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreateCore(dummyPropertyInfo, dummyOptionAttribute)).Returns(dummyOption.Object);

            // Act
            IOption result = testSubject.Object.Create(dummyPropertyInfo);

            // Assert
            Assert.Same(dummyOption.Object, result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Create_ThrowsInvalidOperationExceptionIfThePropertyInfosOptionAttributeHasNeitherALongNameNorAShortName()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();

            Mock<OptionFactory> testSubject = CreateMockOptionFactory();
            testSubject.CallBase = true;
            testSubject.Setup(t => t.CreateCore(dummyPropertyInfo, dummyOptionAttribute)).Throws(new InvalidOperationException());

            // Act and assert
            Assert.Throws<InvalidOperationException>(() => testSubject.Object.Create(dummyPropertyInfo));
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CreateCore_ThrowsInvalidOperationExceptionIfThePropertyInfosOptionAttributeHasNeitherALongNameNorAShortName()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyNamelessOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();

            OptionFactory testSubject = CreateOptionFactory();

            // Act and Assert
            InvalidOperationException result = Assert.Throws<InvalidOperationException>(() => testSubject.CreateCore(dummyPropertyInfo, dummyOptionAttribute));
            Assert.Equal(string.Format(Strings.Exception_OptionAttributeMustHaveName, nameof(DummyCommand.DummyNamelessOptionProperty)), result.Message);
        }

        [Fact]
        public void CreateCore_CreatesOptionIfSuccessful()
        {
            // Arrange
            PropertyInfo dummyPropertyInfo = typeof(DummyCommand).GetProperty(nameof(DummyCommand.DummyOptionProperty));
            OptionAttribute dummyOptionAttribute = dummyPropertyInfo.GetCustomAttribute<OptionAttribute>();

            OptionFactory testSubject = CreateOptionFactory();

            // Act 
            IOption result = testSubject.CreateCore(dummyPropertyInfo, dummyOptionAttribute);

            // Assert
            Assert.Equal(dummyPropertyInfo, result.PropertyInfo);
            Assert.Equal(DummyStrings.OptionShortName_Dummy, result.ShortName);
            Assert.Equal(DummyStrings.OptionLongName_Dummy, result.LongName);
            Assert.Equal(DummyStrings.OptionDescription_Dummy, result.Description);
        }

        private Mock<OptionFactory> CreateMockOptionFactory()
        {
            return _mockRepository.Create<OptionFactory>();
        }

        private OptionFactory CreateOptionFactory()
        {
            return new OptionFactory();
        }

        private class DummyCommand : ICommand
        {
            [Option(
                typeof(DummyStrings),
                nameof(DummyStrings.OptionShortName_Dummy),
                nameof(DummyStrings.OptionLongName_Dummy),
                nameof(DummyStrings.OptionDescription_Dummy))]
            public string DummyOptionProperty { get; }

            [Option]
            public string DummyNamelessOptionProperty { get; }

            public string DummyNoAttributeProperty { get; }

            public string Name => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public bool IsDefault => throw new NotImplementedException();

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
