using JeremyTCD.DotNetCore.Utils;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandLineToolUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Run_PrintsHeaderAndReturnsParseResultIfParseIsSuccessful()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            IEnumerable<Type> dummyModelTypes = new Type[0];
            CommandSet dummyCommandSet = new CommandSet();
            ParseResult dummyParseResult = CreateParseResult();

            Mock<ICommandSetFactory> mockCommandSetFactory = _mockRepository.Create<ICommandSetFactory>();
            mockCommandSetFactory.Setup(c => c.CreateFromTypes(dummyModelTypes)).Returns(dummyCommandSet);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandSet)).Returns(dummyParseResult);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();
            mockPrinter.Setup(p => p.PrintHeader());

            CommandLineTool commandLineTool = new CommandLineTool(mockParser.Object, mockPrinter.Object, mockCommandSetFactory.Object, null);

            // Act
            ParseResult result = commandLineTool.Run(dummyArgs, dummyModelTypes, null, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyParseResult, result);
        }

        [Fact]
        public void Run_HandlesParseExceptionIfParseResultHasParseException()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            IEnumerable<Type> dummyModelTypes = new Type[0];
            CommandSet dummyCommandSet = new CommandSet();
            ParseException dummyParseException = new ParseException();
            ParseResult dummyParseResult = CreateParseResult(parseException: dummyParseException);

            Mock<ICommandSetFactory> mockCommandSetFactory = _mockRepository.Create<ICommandSetFactory>();
            mockCommandSetFactory.Setup(c => c.CreateFromTypes(dummyModelTypes)).Returns(dummyCommandSet);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandSet)).Returns(dummyParseResult);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();

            Mock<CommandLineTool> commandLineTool = _mockRepository.Create<CommandLineTool>(mockParser.Object, mockPrinter.Object, mockCommandSetFactory.Object, null);
            commandLineTool.Setup(c => c.HandleParseException(dummyParseResult));
            commandLineTool.CallBase = true;

            // Act
            ParseResult result = commandLineTool.Object.Run(dummyArgs, dummyModelTypes, null, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.NotNull(result.ParseException);
        }

        [Fact]
        public void Run_HandlesRunnableIfParseResultModelIsAssignableToRunnable()
        {
            // Arrange
            string[] dummyArgs = new string[0];
            IEnumerable<Type> dummyModelTypes = new Type[0];
            CommandSet dummyCommandSet = new CommandSet();
            DummyRunnable dummyRunnable = new DummyRunnable();
            ParseResult dummyParseResult = CreateParseResult(model: dummyRunnable);

            Mock<ICommandSetFactory> mockCommandSetFactory = _mockRepository.Create<ICommandSetFactory>();
            mockCommandSetFactory.Setup(c => c.CreateFromTypes(dummyModelTypes)).Returns(dummyCommandSet);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandSet)).Returns(dummyParseResult);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();

            Mock<CommandLineTool> commandLineTool = _mockRepository.Create<CommandLineTool>(mockParser.Object, mockPrinter.Object, mockCommandSetFactory.Object, null);
            commandLineTool.Setup(c => c.HandleRunnable(dummyParseResult));
            commandLineTool.CallBase = true;

            // Act
            ParseResult result = commandLineTool.Object.Run(dummyArgs, dummyModelTypes, null, null);

            // Assert
            _mockRepository.VerifyAll();
            Assert.True(result.Model is IRunnable);
        }

        [Fact]
        public void HandleRunnable_CallsRunnableRun()
        {
            // Arrange
            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();

            Mock<IRunnable> mockRunnable = _mockRepository.Create<IRunnable>();
            mockRunnable.Setup(r => r.Run(mockPrinter.Object)).Returns(-1);

            ParseResult dummyParseResult = CreateParseResult(model: mockRunnable.Object);

            CommandLineTool commandLineTool = new CommandLineTool(null, mockPrinter.Object, null, null);

            // Act
            commandLineTool.HandleRunnable(dummyParseResult);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleRunnable_ExitsProcessIfExitCodeIsNotNegative()
        {
            // Arrange
            int dummyExitCode = 0;

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();

            Mock<IRunnable> mockRunnable = _mockRepository.Create<IRunnable>();
            mockRunnable.Setup(r => r.Run(mockPrinter.Object)).Returns(dummyExitCode);

            ParseResult dummyParseResult = CreateParseResult(model: mockRunnable.Object);

            Mock<IEnvironmentService> mockEnvironmentService = _mockRepository.Create<IEnvironmentService>();
            mockEnvironmentService.Setup(e => e.Exit(dummyExitCode));

            CommandLineTool commandLineTool = new CommandLineTool(null, mockPrinter.Object, null, mockEnvironmentService.Object);

            // Act
            commandLineTool.HandleRunnable(dummyParseResult);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleParseException_PrintsParseExceptionAndGetHelpHintThenExitsProcess()
        {
            // Arrange
            ParseException dummyParseException = new ParseException();
            ParseResult dummyParseResult = CreateParseResult(parseException: dummyParseException);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();
            mockPrinter.Setup(p => p.PrintParseException(dummyParseException));
            mockPrinter.Setup(p => p.PrintGetHelpHint());

            Mock<IEnvironmentService> mockEnvironmentService = _mockRepository.Create<IEnvironmentService>();
            mockEnvironmentService.Setup(e => e.Exit(1));

            CommandLineTool commandLineTool = new CommandLineTool(null, mockPrinter.Object, null, mockEnvironmentService.Object);

            // Act
            commandLineTool.HandleParseException(dummyParseResult);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleParseException_PrintsCommandSpecificGetHelpHintIfParseResultHasCommand()
        {
            // Arrange
            Command dummyCommand = CreateCommand();
            ParseResult dummyParseResult = CreateParseResult(command: dummyCommand);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();
            mockPrinter.Setup(p => p.PrintGetHelpHint(dummyCommand));

            Mock<IEnvironmentService> mockEnvironmentService = _mockRepository.Create<IEnvironmentService>();

            CommandLineTool commandLineTool = new CommandLineTool(null, mockPrinter.Object, null, mockEnvironmentService.Object);

            // Act
            commandLineTool.HandleParseException(dummyParseResult);

            // Assert
            _mockRepository.VerifyAll();
        }

        private ParseResult CreateParseResult(ParseException parseException = null, Command command = null, object model = null)
        {
            return new ParseResult(parseException, command, model);
        }

        private Command CreateCommand(Type modelType = null, bool isDefault = false, string name = null, string description = null,
            IEnumerable<Option> options = null)
        {
            return new Command(modelType, isDefault, name, description, options);
        }

        private class DummyRunnable : IRunnable
        {
            public int Run(IPrinter printer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
