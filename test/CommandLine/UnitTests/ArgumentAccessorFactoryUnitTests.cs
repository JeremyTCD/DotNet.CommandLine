// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
 
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class ArgumentAccessorFactoryUnitTests
    {
        [Fact]
        public void Create_CreatesArgumentsInstanceFromArray()
        {
            // Arrange
            string dummyOptionName = "dummyOptionName";
            string dummyOptionValue = "dummyOptionValue";
            string dummyCommandName = "dummyCommandName";
            string[] dummyArgs = new[] { dummyCommandName, $"-{dummyOptionName}={dummyOptionValue}" };

            ArgumentAccessorFactory testSubject = CreateArgumentAccessorFactory();

            // Act
            IArgumentAccessor result = testSubject.Create(dummyArgs);

            // Assert
            Assert.Equal(dummyCommandName, result.CommandName);
            Assert.Single(result.OptionArgs);
            Assert.Equal(dummyOptionName, result.OptionArgs.First().Key);
            Assert.Equal(dummyOptionValue, result.OptionArgs.First().Value);
        }

        [Theory]
        [MemberData(nameof(Create_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements_Data))]
        public void Create_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements(string arg)
        {
            // Arrange
            string[] dummyArgs = new string[] { arg };

            ArgumentAccessorFactory testSubject = CreateArgumentAccessorFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => testSubject.Create(dummyArgs));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", dummyArgs)), exception.Message);
        }

        public static IEnumerable<object[]> Create_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements_Data()
        {
            yield return new object[] { null };
            yield return new object[] { " " };
        }

        [Theory]
        [MemberData(nameof(Create_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand_Data))]
        public void Create_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand(string[] args)
        {
            // Arrange
            ArgumentAccessorFactory testSubject = CreateArgumentAccessorFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => testSubject.Create(args));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", args)), exception.Message);
        }

        public static IEnumerable<object[]> Create_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand_Data()
        {
            yield return new object[] { new string[] { "-optionName", "commandName" } };
            yield return new object[] { new string[] { "commandName", "-optionName", "commandName" } };
        }

        private ArgumentAccessorFactory CreateArgumentAccessorFactory()
        {
            return new ArgumentAccessorFactory();
        }
    }
}
