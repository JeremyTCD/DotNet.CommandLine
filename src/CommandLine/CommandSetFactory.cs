using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandSetFactory : ICommandSetFactory
    {
        private ICommandFactory _commandFactory { get; }

        /// <summary>
        /// Creates a <see cref="CommandSetFactory"/> instance.
        /// </summary>
        /// <param name="commandFactory"></param>
        public CommandSetFactory(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        /// <summary>
        /// Creates a <see cref="CommandSet"/> instance from <paramref name="types"/>. This function serves as an early filter for
        /// incompatible/invalid commands. 
        /// </summary>
        /// <param name="types"></param>
        /// <returns>
        /// <see cref="CommandSet"/>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a <see cref="Type"/> in <paramref name="types"/> does not have a <see cref="CommandAttribute"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown there are multiple default commands.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if multiple commands have the same name
        /// </exception>
        public CommandSet CreateFromTypes(IEnumerable<Type> types)
        {
            Dictionary<string, Command> commands = new Dictionary<string, Command>(types.Count());
            Command defaultCommand = null;

            foreach (Type type in types)
            {
                CommandAttribute commandAttribute = type.GetTypeInfo().GetCustomAttribute<CommandAttribute>();

                if (commandAttribute == null)
                {
                    throw new InvalidOperationException(string.Format(Strings.Exception_TypeDoesNotHaveCommandAttribute, type.Name));
                }

                Command command = _commandFactory.TryCreateFromType(type);

                if (command.IsDefault)
                {
                    if (defaultCommand != null)
                    {
                        throw new InvalidOperationException(string.Format(Strings.Exception_MultipleDefaultCommands, 
                            $"\t{defaultCommand.Name}{Environment.NewLine}\t{command.Name}"));
                    }
                    else
                    {
                        defaultCommand = command;
                    }
                }

                try
                {
                    commands.Add(command.Name, command);
                }
                catch(ArgumentException exception)
                {
                    throw new InvalidOperationException(string.Format(Strings.Exception_MultipleCommandsWithSameName, command.Name), exception);
                }
            }

            return new CommandSet(commands);
        }
    }
}
