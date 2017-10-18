// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class CommandLineApp : ICommandLineApp
    {
        private readonly IParser _parser;
        private readonly ICommandDictionaryFactory _commandDictionaryFactory;
        private readonly IEnumerable<ICommand> _commands;
        private readonly CommandLineAppOptions _appOptions;
        private readonly ICommandLineAppContextFactory _appContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineApp"/> class.
        /// </summary>
        /// <param name="parser">The <see cref="CommandLineApp"/>'s <see cref="IParser"/>.</param>
        /// <param name="commandLineAppContextFactory">The <see cref="ICommandLineAppContextFactory"/> used to create the <see cref="CommandLineApp"/>'s
        /// <see cref="ICommandLineAppContext"/>.</param>
        /// <param name="commandDictionaryFactory">The <see cref="ICommandDictionaryFactory"/> used to create the <see cref="CommandLineApp"/>'s
        /// <see cref="ICommandDictionary"/>.</param>
        /// <param name="commands">The collection whose elements are used to populate the <see cref="CommandLineApp"/>'s <see cref="ICommandDictionary"/>.
        /// </param>
        /// <param name="optionsAccessor">The <see cref="CommandLineAppOptions"/> accessor.</param>
        public CommandLineApp(
            IParser parser,
            ICommandLineAppContextFactory commandLineAppContextFactory,
            ICommandDictionaryFactory commandDictionaryFactory,
            IEnumerable<ICommand> commands,
            IOptions<CommandLineAppOptions> optionsAccessor)
        {
            _parser = parser;
            _appContextFactory = commandLineAppContextFactory;
            _commandDictionaryFactory = commandDictionaryFactory;
            _commands = commands;
            _appOptions = optionsAccessor.Value;
        }

        /// <inheritdoc/>
        public virtual int Run(string[] args)
        {
            ICommandDictionary commandDictionary = _commandDictionaryFactory.CreateFromCommands(_commands);
            ICommandLineAppContext appContext = _appContextFactory.Create(commandDictionary, _appOptions);
            IParseResult result = _parser.Parse(args, commandDictionary);

            if (result.Command == null)
            {
                return commandDictionary.DefaultCommand.Run(result, appContext);
            }

            return result.Command.Run(result, appContext);
        }
    }
}
