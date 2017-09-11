using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineApp : ICommandLineApp
    {
        private readonly IParser _parser;
        private readonly ICommandSetFactory _commandSetFactory;
        private readonly IPrinter _printer;
        private readonly IEnumerable<ICommand> _commands;
        private readonly AppOptions _appOptions;

        /// <summary>
        /// Creates a <see cref="CommandLineApp"/> instance.
        /// </summary>
        /// <param name="commandSetFactory"></param>
        /// <param name="parser"></param>
        /// <param name="printer"></param>
        /// <param name="environmentService"></param>
        /// <param name="commands"></param>
        public CommandLineApp(IParser parser, ICommandSetFactory commandSetFactory, IPrinter printer, IEnumerable<ICommand> commands)
        {
            _appOptions = optionsAccessor.Value;
            _commands = commands;
            _parser = parser;
            _printerFactory = printerFactory;
            _commandSetFactory = commandSetFactory;
        }

        /// <summary>
        /// Parses <paramref name="args"/>, creating a <see cref="ParseResult"/> instance. 
        /// If <see cref="ParseResult"/> instance has an <see cref="ICommand"/> instance, calls <see cref="ICommand.Run(ParseResult, IPrinter)"/> and returns its return value.
        /// Otherwise, prints <see cref="ParseResult.ParseException"/> and app get help hint then returns 1.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="printerOptions"></param>
        /// <returns>
        /// <see cref="int"/>
        /// </returns>
        public int Run(string[] args)
        {
            CommandSet commandSet = _commandSetFactory.CreateFromCommands(_commands);
            ParseResult result = _parser.Parse(args, commandSet);

            if(result.Command == null)
            {
                _printer.PrintParseException(result.ParseException);
                _printer.PrintGetHelpHint();

                return 1;
            }

            // If command is not null, allow it to handle ParseExceptions
            return result.Command.Run(result, _printer);
        }
    }
}
