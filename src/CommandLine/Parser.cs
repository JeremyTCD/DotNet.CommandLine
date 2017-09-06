using System;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Parser : IParser
    {
        private IArgumentsFactory _argumentsFactory { get; }
        private IModelFactory _modelFactory { get; }

        /// <summary>
        /// Creates a <see cref="Parser"/> instance.
        /// </summary>
        /// <param name="argumentsFactory"></param>
        /// <param name="modelFactory"></param>
        public Parser(IArgumentsFactory argumentsFactory, IModelFactory modelFactory)
        {
            _argumentsFactory = argumentsFactory;
            _modelFactory = modelFactory;
        }

        /// <summary>
        /// Processes <paramref name="args"/>, selects the relevant <see cref="Command"/> from <paramref name="commandSet"/> and creates
        /// an instance of its model type.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commands"></param>
        /// <returns>
        /// <see cref="ParseResult"/> containing model and <see cref="Command"/> instances if successful. <see cref="ParseResult"/> containing 
        /// a <see cref="ParseException"/> if unsuccessful.
        /// </returns>
        public ParseResult Parse(string[] args, CommandSet commandSet)
        {
            Command command = null;
            object model = null;
            ParseException parseException = null;

            try
            {
                Arguments arguments = _argumentsFactory.CreateFromArray(args);

                command = GetCommandByName(arguments.CommandName, commandSet);
                model = _modelFactory.Create(arguments, command);
            }
            catch(Exception exception) 
            {
                parseException = exception is ParseException ? exception as ParseException : new ParseException(exception);
            }

            return new ParseResult(parseException, command, model);
        }

        /// <summary>
        /// Gets <see cref="Command"/> specified by <paramref name="commandName"/> from <paramref name="commandSet"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandSet"></param>
        /// <returns>
        /// <see cref="Command"/> with name <paramref name="commandName"/> if <paramref name="commandName"/> is not null. Default command if it is null.
        /// </returns>
        internal Command GetCommandByName(string commandName, CommandSet commandSet)
        {
            if (commandName != null)
            {
                commandSet.TryGetValue(commandName, out Command command);

                if (command == null)
                {
                    throw new ParseException(string.Format(Strings.Exception_CommandDoesNotExist, commandName));
                }

                return command;
            }
            else
            {
                Command defaultCommand = commandSet.Values.SingleOrDefault(c => c.IsDefault);

                if (defaultCommand == null)
                {
                    throw new ParseException(Strings.Exception_NoDefaultCommand);
                }

                return defaultCommand;
            }
        }
    }
}
