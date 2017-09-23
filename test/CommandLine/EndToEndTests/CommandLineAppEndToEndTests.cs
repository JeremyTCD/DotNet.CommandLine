// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.EndToEndTests
{
    /// <summary>
    /// Battery of tests for typical usage scenarios. These tests also serve as basic usage examples.
    /// </summary>
    public class CommandLineAppEndToEndTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Theory]
        [MemberData(nameof(ReturnsExpectedExitCodeData))]
        public void Run_ReturnsExpectedExitCode(string dummyArguments, int expectedExitCode)
        {
            // Arrange
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.
                AddCommandLine().
                AddSingleton<ICommand, DummyCommand>().// Add commands
                AddSingleton<ICommand, DummyDefaultCommand>().
                Configure<CommandLineAppOptions>(a =>
                {
                    // Configure app options
                    a.ExecutableName = "dummyExecutableName";
                    a.FullName = "dummyFullName";
                    a.Version = "dummyVersion";
                });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            ICommandLineApp commandLineApp = serviceProvider.GetService<ICommandLineApp>();

            // Act
            int resultExitCode = commandLineApp.Run(dummyArguments.Split(' '));

            // Assert
            (serviceProvider as IDisposable)?.Dispose();
            Assert.Equal(expectedExitCode, resultExitCode);
        }

        // Only verify exit codes
        // - printer output is tested in printer unit tests
        // - command output is tested in command unit tests
        public static IEnumerable<object[]> ReturnsExpectedExitCodeData()
        {
            yield return new object[] { $"test", 0 };
            yield return new object[] { $"-{Strings.OptionLongName_Help}", 1 };
            yield return new object[] { $"{DummyStrings.CommandName_Dummy} -test", 0 };
            yield return new object[] { $"{DummyStrings.CommandName_Dummy} -{Strings.OptionLongName_Help}", 1 };
            yield return new object[] { $"{DummyStrings.CommandName_Dummy} -{DummyStrings.OptionLongName_Dummy}", 1 };
        }

        private class DummyCommand : Command
        {
            public override string Name { get; } = DummyStrings.CommandName_Dummy;

            public override string Description { get; } = DummyStrings.CommandDescription_Dummy;

            public override bool IsDefault { get; } = false;

            [Option(typeof(DummyStrings), nameof(DummyStrings.OptionShortName_Dummy), nameof(DummyStrings.OptionLongName_Dummy), nameof(DummyStrings.OptionDescription_Dummy))]
            public bool DummyOption { get; set; }

            public override int RunCommand(ParseResult parseResult, ICommandLineAppContext appContext)
            {
                return 1;
            }
        }

        private class DummyDefaultCommand : Command
        {
            public override string Name { get; } = "Default";

            public override string Description { get; } = null;

            public override bool IsDefault { get; } = true;

            public override int RunCommand(ParseResult parseResult, ICommandLineAppContext appContext)
            {
                return 1;
            }
        }
    }
}
