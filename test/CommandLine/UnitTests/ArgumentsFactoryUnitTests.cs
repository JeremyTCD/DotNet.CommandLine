﻿// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class ArgumentsFactoryUnitTests
    {
        [Fact]
        public void CreateFromArray_CreatesArgumentsInstanceFromArray()
        {
            // Arrange
            string dummyOptionName = "dummyOptionName";
            string dummyOptionValue = "dummyOptionValue";
            string dummyCommandName = "dummyCommandName";
            string[] dummyArgs = new[] { dummyCommandName, $"-{dummyOptionName}={dummyOptionValue}" };

            ArgumentsFactory testSubject = CreateArgumentsFactory();

            // Act
            IArguments result = testSubject.CreateFromArray(dummyArgs);

            // Assert
            Assert.Equal(dummyCommandName, result.CommandName);
            Assert.Single(result.OptionArgs);
            Assert.Equal(dummyOptionName, result.OptionArgs.First().Key);
            Assert.Equal(dummyOptionValue, result.OptionArgs.First().Value);
        }

        [Theory]
        [MemberData(nameof(CreateFromArray_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements_Data))]
        public void CreateFromArray_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements(string arg)
        {
            // Arrange
            string[] dummyArgs = new string[] { arg };

            ArgumentsFactory testSubject = CreateArgumentsFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => testSubject.CreateFromArray(dummyArgs));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", dummyArgs)), exception.Message);
        }

        public static IEnumerable<object[]> CreateFromArray_ThrowsParseExceptionIfArrayContainsNullOrWhitespaceElements_Data()
        {
            yield return new object[] { null };
            yield return new object[] { " " };
        }

        [Theory]
        [MemberData(nameof(CreateFromArray_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand_Data))]
        public void CreateFromArray_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand(string[] args)
        {
            // Arrange
            ArgumentsFactory testSubject = CreateArgumentsFactory();

            // Act and Assert
            ParseException exception = Assert.Throws<ParseException>(() => testSubject.CreateFromArray(args));
            Assert.Equal(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", args)), exception.Message);
        }

        public static IEnumerable<object[]> CreateFromArray_ThrowsParseExceptionIfAnElementInArgsOtherThanTheFirstElementHasTheFormatOfACommand_Data()
        {
            yield return new object[] { new string[] { "-optionName", "commandName" } };
            yield return new object[] { new string[] { "commandName", "-optionName", "commandName" } };
        }

        private ArgumentsFactory CreateArgumentsFactory()
        {
            return new ArgumentsFactory();
        }
    }
}
