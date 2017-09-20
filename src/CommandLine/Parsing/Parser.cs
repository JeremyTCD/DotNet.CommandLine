// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

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
                command = GetCommand(arguments.CommandName, commandSet);

                _commandMapper.Map(arguments, command);
            }
            catch (Exception exception)
            {
                parseException = exception is ParseException ? exception as ParseException : new ParseException(exception);
            }

            return new ParseResult(parseException, command);
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> with name <paramref name="commandName"/> from <paramref name="commandSet"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandSet"></param>
        /// <returns>
        /// <see cref="ICommand"/> with name <paramref name="commandName"/> if <paramref name="commandName"/> is not null, default
        /// <see cref="ICommand"/> otherwise.
        /// </returns>
        /// <exception cref="ParseException">
        /// Thrown if no command with name <paramref name="commandName"/> exists.
        /// </exception>
        internal virtual ICommand GetCommand(string commandName, CommandSet commandSet)
        {
            ICommand result;

            if (commandName == null)
            {
                result = commandSet.DefaultCommand;
            }
            else
            {
                commandSet.TryGetValue(commandName, out result);

                if (result == null)
                {
                    throw new ParseException(string.Format(Strings.ParseException_CommandDoesNotExist, commandName));
                }
            }

            return result;
        }
    }
}
