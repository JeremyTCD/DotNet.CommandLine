// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a default <see cref="ICommandDictionaryFactory"/>.
    /// </summary>
    public class CommandDictionaryFactory : ICommandDictionaryFactory
    {
        /// <inheritdoc/>
        public virtual ICommandDictionary Create(IEnumerable<ICommand> commands)
        {
            ICommand defaultCommand = null;
            CommandDictionary result = new CommandDictionary();

            foreach (ICommand command in commands)
            {
                if (command.IsDefault)
                {
                    if (defaultCommand != null)
                    {
                        throw new InvalidOperationException(string.Format(
                            Strings.Exception_MultipleDefaultCommands,
                            $"\t{defaultCommand.Name}{Environment.NewLine}\t{command.Name}"));
                    }
                    else
                    {
                        defaultCommand = command;
                    }
                }

                if (string.IsNullOrWhiteSpace(command.Name))
                {
                    throw new InvalidOperationException(Strings.Exception_CommandsMustHaveNames);
                }

                try
                {
                    result.Add(command.Name, command);
                }
                catch (ArgumentException exception)
                {
                    // Weed out commands with the same name
                    throw new InvalidOperationException(string.Format(Strings.Exception_MultipleCommandsWithSameName, command.Name), exception);
                }
            }

            if (defaultCommand == null)
            {
                throw new InvalidOperationException(Strings.Exception_DefaultCommandRequired);
            }

            return result;
        }
    }
}
