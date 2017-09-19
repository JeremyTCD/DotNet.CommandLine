using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineApp : ICommandLineApp
    {
        private readonly IParser _parser;
        private readonly ICommandSetFactory _commandSetFactory;
        private readonly IEnumerable<ICommand> _commands;
        private readonly AppOptions _appOptions;
        private readonly IAppContextFactory _appContextFactory;

        /// <summary>
        /// Creates a <see cref="CommandLineApp"/> instance.
        /// </summary>
        /// <param name="commandSetFactory"></param>
        /// <param name="parser"></param>
        /// <param name="printer"></param>
        /// <param name="environmentService"></param>
        /// <param name="commands"></param>
        public CommandLineApp(IParser parser, ICommandSetFactory commandSetFactory, IAppContextFactory appContextFactory, IEnumerable<ICommand> commands,
            IOptions<AppOptions> optionsAccessor)
        {
            _appOptions = optionsAccessor.Value;
            _commands = commands;
            _parser = parser;
            _appContextFactory = appContextFactory;
            _commandSetFactory = commandSetFactory;
        }

        /// <summary>
        /// Parses <paramref name="args"/>, creating a <see cref="ParseResult"/> instance. 
        /// If <see cref="ParseResult"/> instance has an <see cref="ICommand"/> instance, calls <see cref="ICommand.Run(ParseResult, IAppPrinter)"/> and returns 
        /// its return value. Otherwise, calls <see cref="ICommand.Run(ParseResult, IAppPrinter)"/> on the default command and returns its return value.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="printerOptions"></param>
        /// <returns>
        /// <see cref="int"/>
        /// </returns>
        public int Run(string[] args)
        {
            CommandSet commandSet = _commandSetFactory.CreateFromCommands(_commands);
            AppContext appContext = _appContextFactory.Create(commandSet, _appOptions);
            ParseResult result = _parser.Parse(args, commandSet);

            if(result.Command == null)
            {
                return commandSet.DefaultCommand.Run(result, appContext);
            }

            return result.Command.Run(result, appContext);
        }
    }
}
