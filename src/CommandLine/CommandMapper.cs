using System;
using System.Collections.Generic;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandMapper : ICommandMapper
    {
        private readonly IEnumerable<IMapper> _mappers;
        private readonly IOptionsFactory _optionsFactory;

        /// <summary>
        /// Creates a <see cref="CommandMapper"/> instance.
        /// </summary>
        /// <param name="mappers"></param>
        /// <param name="optionFactory"></param>
        public CommandMapper(IEnumerable<IMapper> mappers, IOptionsFactory optionsFactory)
        {
            _mappers = mappers;
            _optionsFactory = optionsFactory;
        }

        /// <summary>
        /// Maps <paramref name="arguments"/> to properties in <paramref name="command"/> that have an <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="command"></param>
        /// <param name="commandOptions"></param>
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
            IEnumerable<Option> options = _optionsFactory.CreateFromCommand(command);

            foreach (KeyValuePair<string, string> optionArg in arguments.OptionArgs)
            {
                Option option = options.SingleOrDefault(o => o.ShortName == optionArg.Key || o.LongName == optionArg.Key);

                if (option == null)
                {
                    throw new ParseException(string.Format(Strings.ParseException_OptionDoesNotExist, optionArg.Key));
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
                    if (exception is ParseException)
                    {
                        throw;
                    }

                    innerException = exception;
                }

                if (!propertySet || innerException != null)
                {
                    throw new ParseException(string.Format(Strings.ParseException_InvalidOptionValue, optionArg.Value, optionArg.Key), 
                        innerException);
                }
            }
        }
    }
}
