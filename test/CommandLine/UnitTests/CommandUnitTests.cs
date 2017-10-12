// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests
{
    public class CommandUnitTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Default);

        [Fact]
        public void Run_PrintsParseExceptionAndCommandGetHelpTipIfParseResultContainsAParseExceptionAndCommandIsNotDefaultCommand()
        {
            // Arrange
            bool dummyIsDefault = false;
            string dummyTargetPosValue = "this command";
            string dummyCommandName = "dummyCommandName";
            ParseException dummyParseException = new ParseException();
            ParseResult dummyParseResult = new ParseResult(dummyParseException, null);

            Mock<ICommandLineAppPrinter> mockAppPrinter = _mockRepository.Create<ICommandLineAppPrinter>();
            mockAppPrinter.Setup(a => a.AppendHeader()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendLine()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendParseException(dummyParseException)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendGetHelpTip(dummyTargetPosValue, dummyCommandName)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.Print());

            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, mockAppPrinter.Object);

            // TODO codeanalysis mock of class under test does not need mock prefix
            Mock<Command> command = _mockRepository.Create<Command>();
            command.Setup(c => c.Name).Returns(dummyCommandName);
            command.Setup(c => c.IsDefault).Returns(dummyIsDefault);
            command.CallBase = true;

            // Act
            int result = command.Object.Run(dummyParseResult, dummyAppContext);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(0, result);
        }

        [Fact]
        public void Run_PrintsParseExceptionAndAppGetHelpTipIfParseResultContainsAParseExceptionAndCommandIsDefaultCommand()
        {
            // Arrange
            bool dummyIsDefault = true;
            string dummyTargetPosValue = "this application";
            ParseException dummyParseException = new ParseException();
            ParseResult dummyParseResult = new ParseResult(dummyParseException, null);

            Mock<ICommandLineAppPrinter> mockAppPrinter = _mockRepository.Create<ICommandLineAppPrinter>();
            mockAppPrinter.Setup(a => a.AppendHeader()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendLine()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendParseException(dummyParseException)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendGetHelpTip(dummyTargetPosValue, null)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.Print());

            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, mockAppPrinter.Object);

            Mock<Command> command = _mockRepository.Create<Command>();
            command.Setup(c => c.IsDefault).Returns(dummyIsDefault);
            command.CallBase = true;

            // Act
            int result = command.Object.Run(dummyParseResult, dummyAppContext);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(0, result);
        }

        [Fact]
        public void Run_PrintsAppHelpIfHelpIsTrueAndCommandIsDefaultCommand()
        {
            Mock<IParseResult> dummyParseResult = _mockRepository.Create<IParseResult>();

            Mock<ICommandLineAppPrinter> mockAppPrinter = _mockRepository.Create<ICommandLineAppPrinter>();
            mockAppPrinter.Setup(a => a.AppendHeader()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendLine()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendAppHelp(null, 2)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.Print());

            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, mockAppPrinter.Object);

            DummyCommand dummyCommand = new DummyCommand(isDefault: true, help: true);

            // Act
            int result = dummyCommand.Run(dummyParseResult.Object, dummyAppContext);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(1, result);
        }

        [Fact]
        public void Run_PrintsCommandHelpIfHelpIsTrueAndCommandIsNotDefaultCommand()
        {
            // Arrange
            string dummyName = "dummyName";
            Mock<IParseResult> dummyParseResult = _mockRepository.Create<IParseResult>();

            Mock<ICommandLineAppPrinter> mockAppPrinter = _mockRepository.Create<ICommandLineAppPrinter>();
            mockAppPrinter.Setup(a => a.AppendHeader()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendLine()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendCommandHelp(dummyName, null, 2)).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.Print());

            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, mockAppPrinter.Object);

            DummyCommand dummyCommand = new DummyCommand(name: dummyName, help: true);

            // Act
            int result = dummyCommand.Run(dummyParseResult.Object, dummyAppContext);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(1, result);
        }

        [Fact]
        public void Run_CallsRunCommand()
        {
            // Arrange
            int exitCode = 1;

            Mock<ICommandLineAppPrinter> mockAppPrinter = _mockRepository.Create<ICommandLineAppPrinter>();
            mockAppPrinter.Setup(a => a.AppendHeader()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.AppendLine()).Returns(mockAppPrinter.Object);
            mockAppPrinter.Setup(a => a.Print());

            Mock<IParseResult> dummyParseResult = _mockRepository.Create<IParseResult>();

            CommandLineAppContext dummyAppContext = new CommandLineAppContext(null, null, mockAppPrinter.Object);

            Mock<DummyCommand> dummyCommand = _mockRepository.Create<DummyCommand>();
            dummyCommand.Setup(c => c.RunCommand(dummyParseResult.Object, dummyAppContext)).Returns(exitCode);
            dummyCommand.CallBase = true;

            // Act
            int result = dummyCommand.Object.Run(dummyParseResult.Object, dummyAppContext);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(exitCode, result);
        }

        public class DummyCommand : Command
        {
            public DummyCommand()
                : this(null, null, false, false)
            {
            }

            public DummyCommand(string name = null, string description = null, bool isDefault = false, bool help = false)
            {
                Name = name;
                Description = description;
                IsDefault = isDefault;
                Help = help;
            }

            public override string Name { get; }

            public override string Description { get; }

            public override bool IsDefault { get; }

            public override int RunCommand(IParseResult parseResult, ICommandLineAppContext appContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
