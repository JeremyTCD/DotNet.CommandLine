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
        /// <param name="parser">The command line application's <see cref="IParser"/>.</param>
        /// <param name="commandDictionaryFactory">
        /// The <see cref="ICommandDictionaryFactory"/> used to create the command line application's <see cref="CommandDictionary"/>.
        /// </param>
        /// <param name="commands">
        /// The collection whose elements are used to populate the command line application's <see cref="CommandDictionary"/>.
        /// </param>
        /// <param name="commandLineAppContextFactory">
        /// The <see cref="ICommandLineAppContextFactory"/> used to create the command line application's <see cref="CommandLineAppContext"/>.
        /// </param>
        /// <param name="optionsAccessor">The <see cref="CommandLineAppOptions"/> accessor.</param>
        public CommandLineApp(
            IParser parser,
            ICommandDictionaryFactory commandDictionaryFactory,
            IEnumerable<ICommand> commands,
            ICommandLineAppContextFactory commandLineAppContextFactory,
            IOptions<CommandLineAppOptions> optionsAccessor)
        {
            _appOptions = optionsAccessor.Value;
            _commands = commands;
            _parser = parser;
            _appContextFactory = commandLineAppContextFactory;
            _commandDictionaryFactory = commandDictionaryFactory;
        }

        /// <inheritdoc/>
        public int Run(string[] args)
        {
            CommandDictionary commandDictionary = _commandDictionaryFactory.CreateFromCommands(_commands);
            ICommandLineAppContext appContext = _appContextFactory.Create(commandDictionary, _appOptions);
            ParseResult result = _parser.Parse(args, commandDictionary);

            if (result.Command == null)
            {
                return commandDictionary.DefaultCommand.Run(result, appContext);
            }

            ICommandLineApp test = (ICommandLineApp)this;
            return result.Command.Run(result, appContext);
        }
    }
}
