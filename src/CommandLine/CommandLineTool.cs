using JeremyTCD.DotNetCore.Utils;
using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandLineTool : ICommandLineTool
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IParser _parser;
        private readonly IPrinter _printer;
        private readonly ICommandSetFactory _commandSetFactory;

        /// <summary>
        /// Creates a <see cref="CommandLineTool"/> instance.
        /// </summary>
        /// <param name="commandSetFactory"></param>
        /// <param name="parser"></param>
        /// <param name="printer"></param>
        public CommandLineTool(IParser parser, IPrinter printer, ICommandSetFactory commandSetFactory, IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
            _parser = parser;
            _printer = printer;
            _commandSetFactory = commandSetFactory;
        }

        /// <summary>
        /// Parses <paramref name="args"/> into a model. 
        /// <para />
        /// If parsing is successful, calls <see cref="IPrinter.PrintHeader"/>. If model is assignable to <see cref="IRunnable"/>, calls <see cref="IRunnable.Run(IPrinter)"/>. 
        /// If <see cref="IRunnable.Run(IPrinter)"/> returns a non negative integer, exits process with the integer as exit code. Otherwise, returns a <see cref="ParseResult"/> 
        /// instance.
        /// <para />
        /// If parsing is unsuccessful, prints error and help messages before exiting process with exit code 1.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="modelTypes"></param>
        /// <param name="appName"></param>
        /// <param name="appVersion"></param>
        /// <returns>
        /// <see cref="ParseResult"/>
        /// </returns>
        public ParseResult Run(string[] args, IEnumerable<Type> modelTypes, string appName, string appVersion)
        {
            CommandSet commandSet = _commandSetFactory.CreateFromTypes(modelTypes);
            ParseResult result = _parser.Parse(args, commandSet);

            _printer.PrintHeader();

            if (result.ParseException != null)
            {
                HandleParseException(result);
            }
            else if (result.Model is IRunnable)
            {
                HandleRunnable(result);
            }

            return result;
        }

        internal virtual void HandleRunnable(ParseResult result)
        {
            IRunnable runnable = result.Model as IRunnable;
            int exitCode = runnable.Run(_printer);

            if (exitCode > -1)
            {
                _environmentService.Exit(exitCode);
            }
        }

        internal virtual void HandleParseException(ParseResult result)
        {
            _printer.PrintParseException(result.ParseException);

            if (result.Command != null)
            {
                // Hint for getting command specific help
                _printer.PrintGetHelpHint(result.Command);
            }
            else
            {
                _printer.PrintGetHelpHint();
            }

            _environmentService.Exit(1);
        }
    }
}
