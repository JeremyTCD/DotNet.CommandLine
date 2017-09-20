// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    ///
    /// </summary>
    public class CommandLineApp : ICommandLineApp
    {
        private readonly IParser _parser;
        private readonly ICommandSetFactory _commandSetFactory;
        private readonly IEnumerable<ICommand> _commands;
        private readonly CommandLineAppOptions _appOptions;
        private readonly ICommandLineAppContextFactory _appContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineApp"/> class.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="commandSetFactory"></param>
        /// <param name="appContextFactory"></param>
        /// <param name="commands"></param>
        /// <param name="optionsAccessor"></param>
        public CommandLineApp(
            IParser parser,
            ICommandSetFactory commandSetFactory,
            ICommandLineAppContextFactory appContextFactory,
            IEnumerable<ICommand> commands,
            IOptions<CommandLineAppOptions> optionsAccessor)
        {
            _appOptions = optionsAccessor.Value;
            _commands = commands;
            _parser = parser;
            _appContextFactory = appContextFactory;
            _commandSetFactory = commandSetFactory;
        }

        /// <summary>
        /// Parses <paramref name="args"/>, creating a <see cref="ParseResult"/> instance.
        /// If <see cref="ParseResult"/> instance has an <see cref="ICommand"/> instance, calls <see cref="ICommand.Run(ParseResult, ICommandLineAppPrinter)"/> and returns
        /// its return value. Otherwise, calls <see cref="ICommand.Run(ParseResult, ICommandLineAppPrinter)"/> on the default command and returns its return value.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>
        /// <see cref="int"/>
        /// </returns>
        public int Run(string[] args)
        {
            CommandSet commandSet = _commandSetFactory.CreateFromCommands(_commands);
            CommandLineAppContext appContext = _appContextFactory.Create(commandSet, _appOptions);
            ParseResult result = _parser.Parse(args, commandSet);

            if (result.Command == null)
            {
                return commandSet.DefaultCommand.Run(result, appContext);
            }

            return result.Command.Run(result, appContext);
        }
    }
}
