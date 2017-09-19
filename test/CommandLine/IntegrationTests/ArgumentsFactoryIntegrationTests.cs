// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class ArgumentsFactoryIntegrationTests
    {
        [Fact]
        public void CreateFromArray_CreatesArgumentsInstanceFromArray()
        {
            // Arrange
            string dummyOptionName = "dummyOptionName";
            string dummyOptionValue = "dummyOptionValue";
            string dummyCommandName = "dummyCommandName";
            string[] dummyArgs = new[] { dummyCommandName, $"-{dummyOptionName}={dummyOptionValue}" };

            ArgumentsFactory argumentsFactory = new ArgumentsFactory();

            // Act
            Arguments result = argumentsFactory.CreateFromArray(dummyArgs);

            // Assert
            Assert.Equal(dummyCommandName, result.CommandName);
            Assert.Single(result.OptionArgs);
            Assert.Equal(dummyOptionName, result.OptionArgs.First().Key);
            Assert.Equal(dummyOptionValue, result.OptionArgs.First().Value);
        }

        [Theory]
        [MemberData(nameof(ThrowsArgumentsExceptionIfArrayContainsNullOrWhitespaceData))]
        public void CreateFromArray_ThrowsArgumentsExceptionIfArrayContainsNullOrWhitespace(string arg)
        {
            // Arrange
            string[] dummyArgs = new string[] { arg };

            ArgumentsFactory argumentsFactory = new ArgumentsFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => argumentsFactory.CreateFromArray(dummyArgs));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", dummyArgs)), exception.Message);
        }

        public static IEnumerable<object[]> ThrowsArgumentsExceptionIfArrayContainsNullOrWhitespaceData()
        {
            yield return new object[] { null };
            yield return new object[] { " " };
        }

        [Theory]
        [MemberData(nameof(ThrowsArgumentsExceptionIfACommandNameIsNotTheFirstElementInArray))]
        public void CreateFromArray_ThrowsArgumentsExceptionIfAnElementOtherThanTheFirstElementHasTheFormatOfACommand(string[] args)
        {
            // Arrange
            ArgumentsFactory argumentsFactory = new ArgumentsFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => argumentsFactory.CreateFromArray(args));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", args)), exception.Message);
        }

        public static IEnumerable<object[]> ThrowsArgumentsExceptionIfACommandNameIsNotTheFirstElementInArray()
        {
            yield return new object[] { new string[] { "-optionName", "commandName" } };
            yield return new object[] { new string[] { "commandName", "-optionName", "commandName" } };
        }
    }
}
