// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandLineAppPrinterUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void AppendHeader_AppendsHeader()
        {
            // Arrange
            string dummyFullName = "dummyFullName";
            string dummyVersion = "dummyVersion";
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions() { FullName = dummyFullName, Version = dummyVersion };

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter(commandLineAppOptions: dummyAppOptions);

            // Act
            testSubject.AppendHeader();
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(string.Format(Strings.Printer_Header, dummyFullName, dummyVersion), result);
        }

        [Fact]
        public void AppendAppHelp_AppendsAppHelp()
        {
            // Arrange
            int columnGap = 2;
            string columnSeparator = new string(' ', columnGap);
            string rowPrefix = "    ";
            string dummyCommandName = "dummyCommandName";
            string dummyCommandDescription = "dummyCommandDescription";
            string dummyExecutableName = "dummyExecutableName";
            string dummyOptionLongName = "dummyOptionLongName";
            string dummyOptionDescription = "dummyOptionDescription";

            Mock<ICommand> mockDefaultCommand = _mockRepository.Create<ICommand>();
            mockDefaultCommand.Setup(m => m.IsDefault).Returns(true);

            Mock<ICommand> mockNamedCommand = _mockRepository.Create<ICommand>();
            mockNamedCommand.Setup(m => m.Name).Returns(dummyCommandName);
            mockNamedCommand.Setup(m => m.Description).Returns(dummyCommandDescription);

            ICommand[] dummyCommands = new[] { mockDefaultCommand.Object, mockNamedCommand.Object };

            Mock<ICommandDictionary> mockCommandDictionary = _mockRepository.Create<ICommandDictionary>();
            mockCommandDictionary.Setup(c => c.Values).Returns(dummyCommands);
            mockCommandDictionary.Setup(c => c.DefaultCommand).Returns(mockDefaultCommand.Object);

            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions() { ExecutableName = dummyExecutableName };

            Option dummyOption = new Option(null, null, dummyOptionLongName, dummyOptionDescription);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(mockDefaultCommand.Object)).Returns(new List<Option> { dummyOption });

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter(mockCommandDictionary.Object, dummyAppOptions, mockOptionsFactory.Object);

            // Act
            testSubject.AppendAppHelp(rowPrefix, columnGap);

            // Assert
            string result = testSubject.ToString();
            string expected = $"Usage: '{dummyExecutableName} [command] [command options]'{Environment.NewLine}" +
                              $"Usage: '{dummyExecutableName} [options]'{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Commands:{Environment.NewLine}" +
                              $"{rowPrefix}{dummyCommandName}{columnSeparator}{dummyCommandDescription}{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Options:{Environment.NewLine}" +
                              $"{rowPrefix}-{dummyOptionLongName}{columnSeparator}{dummyOptionDescription}{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Run '{dummyExecutableName} [command] -help' for more information about a command.";
            Assert.Equal(expected, result);
            _mockRepository.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(AppendsGetHelpTipData))]
        public void AppendGetHelpTip_AppendsGetHelpTip(string dummyCommandPosValue, string dummyTargetPosValue, string dummyExecutableName, string expected)
        {
            // Arrange
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions() { ExecutableName = dummyExecutableName };

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter(commandLineAppOptions: dummyAppOptions);

            // Act
            testSubject.AppendGetHelpTip(dummyTargetPosValue, dummyCommandPosValue);
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsGetHelpTipData()
        {
            string dummyCommandPosValue = "dummyCommandPosValue";
            string dummyTargetPosValue = "dummyTargetPosValue";
            string dummyExecutableName = "dummyExecutableName";

            yield return new object[]
            {
                dummyCommandPosValue,
                dummyTargetPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_GetHelpTip, dummyExecutableName, dummyCommandPosValue + " ", dummyTargetPosValue)
            };
            yield return new object[]
            {
                null,
                dummyTargetPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_GetHelpTip, dummyExecutableName, string.Empty, dummyTargetPosValue)
            };
        }

        [Theory]
        [MemberData(nameof(AppendsUsageData))]
        public void AppendUsage_AppendsUsage(string dummyOptionsPosValue, string dummyCommandPosValue, string dummyExecutableName, string expected)
        {
            // Arrange
            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions() { ExecutableName = dummyExecutableName };

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter(commandLineAppOptions: dummyAppOptions);

            // Act
            testSubject.AppendUsage(dummyOptionsPosValue, dummyCommandPosValue);
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsUsageData()
        {
            string dummyExecutableName = "dummyExecutableName";
            string dummyOptionsPosValue = "dummyOptionsPosValue";
            string dummyCommandPosValue = "dummyCommandPosValue";

            yield return new object[]
            {
                dummyOptionsPosValue,
                dummyCommandPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_Usage, dummyExecutableName, dummyCommandPosValue + " ", dummyOptionsPosValue)
            };
            yield return new object[]
            {
                dummyOptionsPosValue,
                null,
                dummyExecutableName,
                string.Format(Strings.Printer_Usage, dummyExecutableName, string.Empty, dummyOptionsPosValue)
            };
        }

        [Fact]
        public void AppendDescription_AppendsDescription()
        {
            // Arrange
            string dummyDescription = "dummyDescription";

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter();

            // Act
            testSubject.AppendDescription(dummyDescription);

            // Assert
            string result = testSubject.ToString();
            Assert.Equal(string.Format(Strings.Printer_Description, dummyDescription), result);
        }

        [Fact]
        public void AppendCommandHelp_AppendsCommandHelp()
        {
            // Arrange
            int columnGap = 2;
            string columnSeparator = new string(' ', columnGap);
            string rowPrefix = "    ";
            string dummyCommandName = "dummyCommandName";
            string dummyDescription = "dummyDescription";
            string dummyExecutableName = "dummyExecutableName";
            string dummyOptionLongName = "dummyOptionLongName";
            string dummyOptionDescription = "dummyOptionDescription";

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            mockCommand.Setup(c => c.Description).Returns(dummyDescription);

            ICommand outValue = mockCommand.Object;
            Mock<ICommandDictionary> mockCommandDictionary = _mockRepository.Create<ICommandDictionary>();
            mockCommandDictionary.Setup(c => c.TryGetValue(dummyCommandName, out outValue)).Returns(true);

            CommandLineAppOptions dummyAppOptions = new CommandLineAppOptions() { ExecutableName = dummyExecutableName };

            Option dummyOption = new Option(null, null, dummyOptionLongName, dummyOptionDescription);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(mockCommand.Object)).Returns(new List<Option> { dummyOption });

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter(mockCommandDictionary.Object, dummyAppOptions, mockOptionsFactory.Object);

            // Act
            testSubject.AppendCommandHelp(dummyCommandName, rowPrefix, columnGap);

            // Assert
            string result = testSubject.ToString();
            string expected = $"Description: {dummyDescription}{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Usage: '{dummyExecutableName} {dummyCommandName} [options]'{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Options:{Environment.NewLine}" +
                              $"{rowPrefix}-{dummyOptionLongName}{columnSeparator}{dummyOptionDescription}";
            Assert.Equal(expected, result);
            _mockRepository.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(AppendsParseExceptionData))]
        public void AppendParseException_AppendsParseException(ParseException parseException, string expected)
        {
            // Arrange
            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter();

            // Act
            testSubject.AppendParseException(parseException);
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsParseExceptionData()
        {
            string dummyMessage = "dummyMessage";

            yield return new object[] { new ParseException(dummyMessage), dummyMessage };
            yield return new object[] { new ParseException(new ParseException(dummyMessage)), dummyMessage };
        }

        [Theory]
        [MemberData(nameof(GetsOptionNamesData))]
        public void GetOptionNames_GetsOptionNames(string dummyShortName, string dummyLongName, string expected)
        {
            // Arrange
            Option dummyOption = new Option(null, dummyShortName, dummyLongName, null);
            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter();

            // Act
            string result = testSubject.GetOptionNames(dummyOption);

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetsOptionNamesData()
        {
            string dummyShortName = "dummyShortName";
            string dummyLongName = "dummyLongName";

            yield return new object[] { dummyShortName, dummyLongName, $"-{dummyShortName}|-{dummyLongName}" };
            yield return new object[] { null, dummyLongName, $"-{dummyLongName}" };
            yield return new object[] { dummyShortName, null, $"-{dummyShortName}" };
            yield return new object[] { " ", dummyLongName, $"-{dummyLongName}" };
            yield return new object[] { dummyShortName, " ", $"-{dummyShortName}" };
        }

        [Theory]
        [MemberData(nameof(GetsNormalizedPosValueData))]
        public void GetNormalizedPosValue_GetsNormalizedPosValue(string dummyPosValue, string expected)
        {
            // Arrange
            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter();

            // Act
            string result = testSubject.GetNormalizedPosValue(dummyPosValue);

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetsNormalizedPosValueData()
        {
            string dummyPosValue = "dummyPosValue";

            yield return new object[] { dummyPosValue, $"{dummyPosValue} " };
            yield return new object[] { null, string.Empty };
        }

        [Theory]
        [MemberData(nameof(AppendsRowsData))]
        public void AppendRows_AppendsRows(string dummyRowPrefix, int dummyColumnGap, string expected)
        {
            // Arrange
            // Column 1 - Empty string, increasing length
            // Column 2 - Decreasing length
            // Column 3 - Strings with same length, increasing then decreasing length
            string[][] rows = new string[][]
            {
                new string[] { string.Empty, "123", "1" },
                new string[] { "1", "12", "12" },
                new string[] { "12", "1", "1" }
            };

            CommandLineAppPrinter testSubject = CreateCommandLineAppPrinter();

            // Act
            testSubject.AppendRows(rows, dummyColumnGap, dummyRowPrefix);
            string result = testSubject.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsRowsData()
        {
            string dummyRowPrefix = "    ";
            int dummyColumnGap = 2;
            string columnSeparator = new string(' ', dummyColumnGap);

            yield return new object[]
            {
                dummyRowPrefix,
                dummyColumnGap, $"{dummyRowPrefix}  {columnSeparator}123{columnSeparator}1 {Environment.NewLine}{dummyRowPrefix}1 {columnSeparator}12 {columnSeparator}12{Environment.NewLine}{dummyRowPrefix}12{columnSeparator}1  {columnSeparator}1 "
            };
            yield return new object[]
            {
                null,
                dummyColumnGap, $"  {columnSeparator}123{columnSeparator}1 {Environment.NewLine}1 {columnSeparator}12 {columnSeparator}12{Environment.NewLine}12{columnSeparator}1  {columnSeparator}1 "
            };
        }

        private CommandLineAppPrinter CreateCommandLineAppPrinter(
            ICommandDictionary commandDictionary = null,
            CommandLineAppOptions commandLineAppOptions = null,
            IOptionsFactory optionsFactory = null)
        {
            return new CommandLineAppPrinter(
                commandDictionary,
                commandLineAppOptions,
                optionsFactory);
        }

        private class DummyCommandWithOptions : ICommand
        {
            public DummyCommandWithOptions(string name = null, string description = null, bool isDefault = false)
            {
                Name = name;
                Description = description;
                IsDefault = isDefault;
            }

            public string Name { get; }

            public string Description { get; }

            public bool IsDefault { get; }

            [Option(LongName = "DummyOptionLongName", Description = "DummyOptionDescription")]
            public string DummyOption { get; }

            public int Run(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
