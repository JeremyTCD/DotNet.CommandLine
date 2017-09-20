// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application. Exposes the entry method for running the command line application,
    /// <see cref="Run(string[])"/>. Additionally, serves as the dependency injection root for the library.
    /// </summary>
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

        /// <summary>
        /// Parses <paramref name="args"/>, creating a <see cref="ParseResult"/>.
        /// If the <see cref="ParseResult"/> has an <see cref="ICommand"/>, calls <see cref="ICommand.Run(ParseResult, CommandLineAppContext)"/> and returns
        /// its return value. Otherwise, calls the same method on the default command and returns its return value.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>
        /// <see cref="int"/>
        /// </returns>
        public int Run(string[] args)
        {
            CommandDictionary commandDictionary = _commandDictionaryFactory.CreateFromCommands(_commands);
            CommandLineAppContext appContext = _appContextFactory.Create(commandDictionary, _appOptions);
            ParseResult result = _parser.Parse(args, commandDictionary);

            if (result.Command == null)
            {
                return commandDictionary.DefaultCommand.Run(result, appContext);
            }

            return result.Command.Run(result, appContext);
        }
    }
}
