// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class AppPrinterUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void AppendHeader_AppendsHeader()
        {
            // Arrange
            string dummyFullName = "dummyFullName";
            string dummyVersion = "dummyVersion";
            AppOptions dummyAppOptions = new AppOptions() { FullName = dummyFullName, Version = dummyVersion };

            AppPrinter printer = new AppPrinter(null, dummyAppOptions, null);

            // Act
            printer.AppendHeader();
            string result = printer.ToString();

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
            // Dummy commands
            DummyCommand dummyDefaultCommand = new DummyCommand(isDefault: true);
            string dummyCommandName = "dummyCommandName";
            string dummyCommandDescription = "dummyCommandDescription";
            DummyCommand dummyCommand = new DummyCommand(dummyCommandName, dummyCommandDescription);
            CommandSet dummyCommandSet = new CommandSet(new Dictionary<string, ICommand>(){
                { dummyCommandName, dummyCommand },
                { "", dummyDefaultCommand }
            });
            // Dummy context
            string dummyExecutableName = "dummyExecutableName";
            AppOptions dummyAppOptions = new AppOptions() { ExecutableName = dummyExecutableName };
            // Dummy options
            string dummyOptionLongName = "dummyOptionLongName";
            string dummyOptionDescription = "dummyOptionDescription";
            Option dummyOption = new Option(null, null, dummyOptionLongName, dummyOptionDescription);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyDefaultCommand)).Returns(new List<Option> { dummyOption });

            AppPrinter printer = new AppPrinter(dummyCommandSet, dummyAppOptions, mockOptionsFactory.Object);

            // Act
            printer.AppendAppHelp(rowPrefix, columnGap);
            string result = printer.ToString();

            // Assert
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
        }

        [Theory]
        [MemberData(nameof(AppendsGetHelpTipData))]
        public void AppendGetHelpTip_AppendsGetHelpTip(string dummyCommandPosValue, string dummyTargetPosValue, string dummyExecutableName, string expected)
        {
            // Arrange
            AppOptions dummyAppOptions = new AppOptions() { ExecutableName = dummyExecutableName };

            AppPrinter printer = new AppPrinter(null, dummyAppOptions, null);

            // Act
            printer.AppendGetHelpTip(dummyTargetPosValue, dummyCommandPosValue);
            string result = printer.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsGetHelpTipData()
        {
            string dummyCommandPosValue = "dummyCommandPosValue";
            string dummyTargetPosValue = "dummyTargetPosValue";
            string dummyExecutableName = "dummyExecutableName";

            yield return new object[] { dummyCommandPosValue,
                dummyTargetPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_GetHelpTip, dummyExecutableName, dummyCommandPosValue + " ", dummyTargetPosValue)
            };
            yield return new object[] { null,
                dummyTargetPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_GetHelpTip, dummyExecutableName, "", dummyTargetPosValue)
            };
        }

        [Theory]
        [MemberData(nameof(AppendsUsageData))]
        public void AppendUsage_AppendsUsage(string dummyOptionsPosValue, string dummyCommandPosValue, string dummyExecutableName, string expected)
        {
            // Arrange
            AppOptions dummyAppOptions = new AppOptions() { ExecutableName = dummyExecutableName };

            AppPrinter printer = new AppPrinter(null, dummyAppOptions, null);

            // Act
            printer.AppendUsage(dummyOptionsPosValue, dummyCommandPosValue);
            string result = printer.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsUsageData()
        {
            string dummyExecutableName = "dummyExecutableName";
            string dummyOptionsPosValue = "dummyOptionsPosValue";
            string dummyCommandPosValue = "dummyCommandPosValue";

            yield return new object[] { dummyOptionsPosValue,
                dummyCommandPosValue,
                dummyExecutableName,
                string.Format(Strings.Printer_Usage, dummyExecutableName, dummyCommandPosValue + " ", dummyOptionsPosValue)
            };
            yield return new object[] { dummyOptionsPosValue,
                null,
                dummyExecutableName,
                string.Format(Strings.Printer_Usage, dummyExecutableName, "", dummyOptionsPosValue)
            };
        }

        [Fact]
        public void AppendDescription_AppendsDescription()
        {
            // Arrange
            string dummyDescription = "dummyDescription";
            DummyCommand dummyCommand = new DummyCommand(description: dummyDescription);

            AppPrinter printer = new AppPrinter(null, null, null);

            // Act
            printer.AppendDescription(dummyDescription);
            string result = printer.ToString();

            // Assert
            Assert.Equal(string.Format(Strings.Printer_Description, dummyDescription), result);
        }

        [Fact]
        public void AppendCommandHelp_AppendsCommandHelp()
        {
            // Arrange
            int columnGap = 2;
            string columnSeparator = new string(' ', columnGap);
            string rowPrefix = "    ";
            // Dummy command
            string dummyCommandName = "dummyCommandName";
            string dummyDescription = "dummyDescription";
            DummyCommand dummyCommand = new DummyCommand(dummyCommandName, dummyDescription);
            CommandSet dummyCommandSet = new CommandSet(new Dictionary<string, ICommand>(){
                { dummyCommandName, dummyCommand },
            });
            // Dummy context
            string dummyExecutableName = "dummyExecutableName";
            AppOptions dummyAppOptions = new AppOptions() { ExecutableName = dummyExecutableName };
            // Dummy options
            string dummyOptionLongName = "dummyOptionLongName";
            string dummyOptionDescription = "dummyOptionDescription";
            Option dummyOption = new Option(null, null, dummyOptionLongName, dummyOptionDescription);

            Mock<IOptionsFactory> mockOptionsFactory = _mockRepository.Create<IOptionsFactory>();
            mockOptionsFactory.Setup(o => o.CreateFromCommand(dummyCommand)).Returns(new List<Option> { dummyOption });

            AppPrinter printer = new AppPrinter(dummyCommandSet, dummyAppOptions, mockOptionsFactory.Object);

            // Act
            printer.AppendCommandHelp(dummyCommandName, rowPrefix, columnGap);
            string result = printer.ToString();

            // Assert
            string expected = $"Description: {dummyDescription}{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Usage: '{dummyExecutableName} {dummyCommandName} [options]'{Environment.NewLine}" +
                              $"{Environment.NewLine}" +
                              $"Options:{Environment.NewLine}" +
                              $"{rowPrefix}-{dummyOptionLongName}{columnSeparator}{dummyOptionDescription}";
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(AppendsParseExceptionData))]
        public void AppendParseException_AppendsParseException(ParseException parseException, string expected)
        {
            // Arrange
            AppPrinter printer = new AppPrinter(null, null, null);

            // Act
            printer.AppendParseException(parseException);
            string result = printer.ToString();

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
            AppPrinter printer = new AppPrinter(null, null, null);

            // Act
            string result = printer.GetOptionNames(dummyOption);

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
            AppPrinter printer = new AppPrinter(null, null, null);

            // Act
            string result = printer.GetNormalizedPosValue(dummyPosValue);

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetsNormalizedPosValueData()
        {
            string dummyPosValue = "dummyPosValue";

            yield return new object[] { dummyPosValue, $"{dummyPosValue} " };
            yield return new object[] { null, "" };
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
                new string[] { "", "123", "1"},
                new string[] { "1", "12", "12"},
                new string[] { "12", "1", "1"}
            };

            AppPrinter printer = new AppPrinter(null, null, null);

            // Act
            printer.AppendRows(rows, dummyColumnGap, dummyRowPrefix);
            string result = printer.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> AppendsRowsData()
        {
            string dummyRowPrefix = "    ";
            int dummyColumnGap = 2;
            string columnSeparator = new string(' ', dummyColumnGap);

            yield return new object[] { dummyRowPrefix,
                dummyColumnGap, $"{dummyRowPrefix}  {columnSeparator}123{columnSeparator}1 {Environment.NewLine}{dummyRowPrefix}1 {columnSeparator}12 {columnSeparator}12{Environment.NewLine}{dummyRowPrefix}12{columnSeparator}1  {columnSeparator}1 " };
            yield return new object[] { null,
                dummyColumnGap, $"  {columnSeparator}123{columnSeparator}1 {Environment.NewLine}1 {columnSeparator}12 {columnSeparator}12{Environment.NewLine}12{columnSeparator}1  {columnSeparator}1 " };
        }

        private class DummyCommandWithOptions : ICommand
        {
            public string Name { get; }
            public string Description { get; }
            public bool IsDefault { get; }

            [Option(LongName = "DummyOptionLongName", Description = "DummyOptionDescription")]
            public string DummyOption { get; }

            public DummyCommandWithOptions(string name = null, string description = null, bool isDefault = false)
            {
                Name = name;
                Description = description;
                IsDefault = isDefault;
            }

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new NotImplementedException();
            }
        }

        private class DummyCommand : ICommand
        {
            public string Name { get; }
            public string Description { get; }
            public bool IsDefault { get; }

            public DummyCommand(string name = null, string description = null, bool isDefault = false)
            {
                Name = name;
                Description = description;
                IsDefault = isDefault;
            }

            public int Run(ParseResult parseResult, AppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
