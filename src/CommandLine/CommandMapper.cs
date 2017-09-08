using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandMapper : ICommandMapper
    {
        private readonly IOptionFactory _optionFactory;
        private readonly IEnumerable<IMapper> _mappers;

        /// <summary>
        /// Creates a <see cref="CommandMapper"/> instance.
        /// </summary>
        /// <param name="mappers"></param>
        /// <param name="optionFactory"></param>
        public CommandMapper(IEnumerable<IMapper> mappers, IOptionFactory optionFactory)
        {
            _optionFactory = optionFactory;
            _mappers = mappers;
        }

        /// <summary>
        /// Maps <paramref name="arguments"/> to properties in <paramref name="command"/> that have an <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="command"></param>
        /// <returns>
        /// object
        /// </returns>
        /// <exception cref="ParseException">
        /// Thrown if <paramref name="arguments"/> contains an option that does not exist.
        /// </exception>
        /// <exception cref="ParseException">
        /// Thrown if <paramref name="arguments"/> contains an invalid option value.
        /// </exception>
        public void Map(Arguments arguments, ICommand command)
        {
            List<Option> options = GetOptionsFromCommand(command);

            foreach (KeyValuePair<string, string> optionArg in arguments.OptionArgs)
            {
                Option option = options.SingleOrDefault(o => o.ShortName == optionArg.Key || o.LongName == optionArg.Key);

                if (option == null)
                {
                    throw new ParseException(string.Format(Strings.Exception_OptionDoesNotExist, optionArg.Key));
                }

                bool propertySet = false;
                Exception innerException = null;

                try
                {
                    foreach (IMapper mapper in _mappers)
                    {
                        if (mapper.TryMap(option.PropertyInfo, optionArg.Value, command))
                        {
                            propertySet = true;
                            break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    innerException = exception;
                }

                if (!propertySet || innerException != null)
                {
                    throw new ParseException(string.Format(Strings.Exception_InvalidOptionValue, optionArg.Value, optionArg.Key), innerException);
                }
            }
        }

        internal virtual List<Option> GetOptionsFromCommand(ICommand command)
        {
            List<Option> result = new List<Option>();
            foreach (PropertyInfo propertyInfo in command.GetType().GetProperties())
            {
                Option option = _optionFactory.TryCreateFromPropertyInfo(propertyInfo);
                if (option != null)
                {
                    result.Add(option);
                }
            }

            return result;
        }
    }
}
