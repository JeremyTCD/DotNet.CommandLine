
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// App context aware command line printer. 
    /// </summary>
    public class AppPrinter : IAppPrinter
    {
        private readonly AppOptions _appOptions;
        private readonly IOptionsFactory _optionsFactory;
        private readonly CommandSet _commandSet;
        private readonly StringBuilder _stringBuilder;

        /// <summary>
        /// Creates an <see cref="AppPrinter"/> instance.
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="optionsFactory"></param>
        public AppPrinter(CommandSet commandSet, AppOptions appOptions, IOptionsFactory optionsFactory)
        {
            _commandSet = commandSet;
            _appOptions = appOptions;
            _optionsFactory = optionsFactory;
            _stringBuilder = new StringBuilder();
        }

        public virtual IAppPrinter Clear()
        {
            _stringBuilder.Clear();

            return this;
        }

        public virtual IAppPrinter Print()
        {
            Console.Write(_stringBuilder.ToString());

            return this;
        }

        public virtual IAppPrinter AppendHeader()
        {
            _stringBuilder.Append(string.Format(Strings.Printer_Header, _appOptions.FullName, _appOptions.Version));

            return this;
        }

        public virtual IAppPrinter AppendAppHelp(string rowPrefix = null, int columnGap = 2)
        {
            // Usage
            AppendUsage("[command options]", "[command]");
            _stringBuilder.AppendLine();
            AppendUsage("[options]", string.Empty);

            // Commands
            IEnumerable<ICommand> nonDefaultCommands = _commandSet.
                Values.
                Where(c => !c.IsDefault);
            if (nonDefaultCommands.Count() > 0)
            {
                string[][] commandDescriptions = nonDefaultCommands.
                    Select(c => new string[] { c.Name, c.Description }).
                    ToArray();

                _stringBuilder.
                    AppendLine().
                    AppendLine().
                    Append($"Commands:").
                    AppendLine();
                AppendRows(commandDescriptions, columnGap, "    ");
            }

            // Default command options 
            IEnumerable<Option> defaultCommandOptions = _optionsFactory.CreateFromCommand(_commandSet.DefaultCommand);
            if (defaultCommandOptions.Count() > 0)
            {
                string[][] optionDescriptions = defaultCommandOptions.
                    Select(o => new string[] { GetOptionNames(o), o.Description }).
                    ToArray();

                _stringBuilder.
                    AppendLine().
                    AppendLine().
                    Append($"Options:").
                    AppendLine();
                AppendRows(optionDescriptions, columnGap, "    ");
            }

            // Get help tip
            _stringBuilder.
                AppendLine().
                AppendLine();
            AppendGetHelpTip("a command", "[command]");

            return this;
        }

        public virtual IAppPrinter AppendGetHelpTip(string targetPosValue, string commandPosValue = null)
        {
            _stringBuilder.
                Append(string.Format(Strings.Printer_GetHelpTip,
                    _appOptions.ExecutableName,
                    GetNormalizedPosValue(commandPosValue),
                    targetPosValue));

            return this;
        }

        public virtual IAppPrinter AppendUsage(string optionsPosValue, string commandPosValue = null)
        {
            _stringBuilder.
                Append(string.Format(Strings.Printer_Usage,
                    _appOptions.ExecutableName,
                    GetNormalizedPosValue(commandPosValue),
                    optionsPosValue));

            return this;
        }

        public virtual IAppPrinter AppendDescription(string description)
        {
            _stringBuilder.
                Append(string.Format(Strings.Printer_Description, description));

            return this;
        }

        /// <summary>
        /// Appends command help
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="rowPrefix"></param>
        /// <param name="columnGap"></param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no command with name <paramref name="commandName"/> exists.
        /// </exception>
        public virtual IAppPrinter AppendCommandHelp(string commandName, string rowPrefix = null, int columnGap = 2)
        {
            if (!_commandSet.TryGetValue(commandName, out ICommand command))
            {
                throw new InvalidOperationException(string.Format(Strings.Exception_CommandDoesNotExist, commandName));
            }

            // Description
            if (!string.IsNullOrWhiteSpace(command.Description))
            {
                AppendDescription(command.Description);
            }

            // Usage
            _stringBuilder.
                AppendLine().
                AppendLine();
            AppendUsage("[options]", commandName);

            // Options
            IEnumerable<Option> options = _optionsFactory.CreateFromCommand(command);
            if (options.Count() > 0)
            {
                string[][] optionDescriptions = options.
                    Select(o => new string[] { GetOptionNames(o), o.Description }).
                    ToArray();

                _stringBuilder.
                    AppendLine().
                    AppendLine().
                    Append($"Options:").
                    AppendLine();
                AppendRows(optionDescriptions, columnGap, "    ");
            }

            return this;
        }

        public virtual IAppPrinter AppendParseException(ParseException parseException)
        {
            string innerMostMessage = parseException.Message;
            Exception innerException = parseException.InnerException;

            while (innerException != null)
            {
                if (innerException is ParseException)
                {
                    innerMostMessage = innerException.Message;
                }

                innerException = innerException.InnerException;
            }

            _stringBuilder.Append(innerMostMessage);

            return this;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        #region Helpers
        /// <summary>
        /// Creates a string containing <paramref name="option"/>'s names. If it has more than one name, the names
        /// are separated by a |.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        internal string GetOptionNames(Option option)
        {
            List<string> names = new List<string>();

            if (!string.IsNullOrWhiteSpace(option.ShortName))
            {
                names.Add($"-{option.ShortName}");
            }
            if (!string.IsNullOrWhiteSpace(option.LongName))
            {
                names.Add($"-{option.LongName}");
            }

            return string.Join("|", names);
        }

        /// <summary>
        /// If <paramref name="posValue"/> is not null or empty, appends a space and returns result. Otherwise,
        /// returns an empty string.
        /// </summary>
        /// <param name="posValue"></param>
        /// <returns><see cref="string"/></returns>
        internal string GetNormalizedPosValue(string posValue)
        {
            if (string.IsNullOrEmpty(posValue))
            {
                return string.Empty;
            }

            return posValue + " ";
        }

        /// <summary>
        /// Identifies the longest value in each column. Then, iterates through rows, appending each row to <paramref name="stringBuilder"/>. While doing so,
        /// values are padded so that their padded widths are <paramref name="columnGap"/> greater than the width of the longest value in their columns.
        /// </summary>
        /// <param name="rows">Array of rows where each row is a <see cref="string[]"/>. All rows are assumed to have the same number of
        /// columns as the first row.</param>
        /// <param name="stringBuilder"></param>
        internal void AppendRows(string[][] rows, int columnGap, string rowPrefix)
        {
            int numRows = rows.Length;
            int numColumns = rows[0].Length;
            int[] columnWidths = new int[numColumns];

            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    string value = rows[y][x];
                    if (value.Length > columnWidths[x])
                    {
                        columnWidths[x] = value.Length;
                    }
                }
            }

            for (int y = 0; y < numRows; y++)
            {
                _stringBuilder.Append(rowPrefix ?? string.Empty);
                for (int x = 0; x < numColumns; x++)
                {
                    _stringBuilder.Append(rows[y][x].PadRight(columnWidths[x] + (x == numColumns - 1 ? 0 : columnGap)));
                }

                if (y != numRows - 1)
                {
                    _stringBuilder.Append(Environment.NewLine);
                }
            }
        }
        #endregion
    }
}
