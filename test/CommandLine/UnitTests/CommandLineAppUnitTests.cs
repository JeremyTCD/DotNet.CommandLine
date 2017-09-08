using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace JeremyTCD.DotNet.CommandLine.Tests.UnitTests
{
    public class CommandLineAppUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        [Fact]
        public void Run_ParsesAndCallsCommandRunIfParsingIsSuccessful()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            CommandSet dummyCommandSet = new CommandSet();
            string[] dummyArgs = new string[0];
            int dummyExitCode = 1;
            AppOptions dummyAppOptions = new AppOptions();
            AppContext dummyAppContext = new AppContext(null, null);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();

            Mock<ICommand> mockCommand = _mockRepository.Create<ICommand>();
            ParseResult dummyParseResult = new ParseResult(null, mockCommand.Object);
            mockCommand.Setup(c => c.Run(dummyParseResult, mockPrinter.Object, dummyAppContext)).Returns(dummyExitCode);

            Mock<ICommandSetFactory> mockCommandSetFactory = _mockRepository.Create<ICommandSetFactory>();
            mockCommandSetFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(dummyCommandSet);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandSet)).Returns(dummyParseResult);

            Mock<IOptions<AppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<AppOptions>>();
            mockOptionsAccessor.Setup(o => o.Value).Returns(dummyAppOptions);

            Mock<IAppContextFactory> mockAppContextFactory = _mockRepository.Create<IAppContextFactory>();
            mockAppContextFactory.Setup(a => a.Create(dummyCommandSet, dummyAppOptions)).Returns(dummyAppContext);

            CommandLineApp commandLineApp = new CommandLineApp(mockParser.Object, mockCommandSetFactory.Object, mockPrinter.Object, dummyCommands, 
                mockOptionsAccessor.Object, mockAppContextFactory.Object);

            // Act
            int result = commandLineApp.Run(dummyArgs); 

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(dummyExitCode, result);
        }

        [Fact]
        public void Run_PrintsParseExceptionAppGetHelpHintAndReturns1ifParseResultHasNoCommand()
        {
            // Arrange
            ICommand[] dummyCommands = new ICommand[0];
            CommandSet dummyCommandSet = new CommandSet();
            string[] dummyArgs = new string[0];
            ParseException dummyParseException = new ParseException();
            ParseResult dummyParseResult = new ParseResult(dummyParseException, null);

            Mock<IPrinter> mockPrinter = _mockRepository.Create<IPrinter>();
            mockPrinter.Setup(p => p.PrintParseException(dummyParseException));
            mockPrinter.Setup(p => p.PrintGetHelpHint());

            Mock<ICommandSetFactory> mockCommandSetFactory = _mockRepository.Create<ICommandSetFactory>();
            mockCommandSetFactory.Setup(c => c.CreateFromCommands(dummyCommands)).Returns(dummyCommandSet);

            Mock<IParser> mockParser = _mockRepository.Create<IParser>();
            mockParser.Setup(p => p.Parse(dummyArgs, dummyCommandSet)).Returns(dummyParseResult);

            Mock<IOptions<AppOptions>> mockOptionsAccessor = _mockRepository.Create<IOptions<AppOptions>>();

            CommandLineApp commandLineApp = new CommandLineApp(mockParser.Object, mockCommandSetFactory.Object, mockPrinter.Object, 
                dummyCommands, mockOptionsAccessor.Object, null);

            // Act
            int result = commandLineApp.Run(dummyArgs);

            // Assert
            _mockRepository.VerifyAll();
            Assert.Equal(1, result);
        }
    }
}
