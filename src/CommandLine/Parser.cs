using System;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Parser : IParser
    {
        private readonly IArgumentsFactory _argumentsFactory;
        private readonly ICommandMapper _commandMapper;

        /// <summary>
        /// Creates a <see cref="Parser"/> instance.
        /// </summary>
        /// <param name="argumentsFactory"></param>
        /// <param name="commandMapper"></param>
        public Parser(IArgumentsFactory argumentsFactory, ICommandMapper commandMapper)
        {
            _argumentsFactory = argumentsFactory;
            _commandMapper = commandMapper;
        }

        /// <summary>
        /// Parses <paramref name="args"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commandSet"></param>
        /// <returns>
        /// <see cref="ParseResult"/> 
        /// </returns>
        public ParseResult Parse(string[] args, CommandSet commandSet)
        {
            ICommand command = null;
            ParseException parseException = null;

            try
            {
                Arguments arguments = _argumentsFactory.CreateFromArray(args);
                command = GetCommandByName(arguments.CommandName, commandSet);

                _commandMapper.Map(arguments, command);
            }
            catch(Exception exception) 
            {
                parseException = exception is ParseException ? exception as ParseException : new ParseException(exception);
            }

            return new ParseResult(parseException, command);
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> specified by <paramref name="commandName"/> from <paramref name="commandSet"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandSet"></param>
        /// <returns>
        /// <see cref="ICommand"/> with name <paramref name="commandName"/> if <paramref name="commandName"/> is not null. Default command if it is null.
        /// </returns>
        internal virtual ICommand GetCommandByName(string commandName, CommandSet commandSet)
        {
            if (commandName != null)
            {
                commandSet.TryGetValue(commandName, out ICommand command);

                if (command == null)
                {
                    throw new ParseException(string.Format(Strings.ParseException_CommandDoesNotExist, commandName));
                }

                return command;
            }
            else
            {
                ICommand defaultCommand = commandSet.Values.SingleOrDefault(c => c.IsDefault);

                if (defaultCommand == null)
                {
                    throw new ParseException(Strings.ParseException_NoDefaultCommand);
                }

                return defaultCommand;
            }
        }
    }
}
