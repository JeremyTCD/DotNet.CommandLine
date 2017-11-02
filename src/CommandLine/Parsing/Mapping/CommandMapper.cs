// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// Initializes a new instance of the <see cref="CommandMapper"/> class.
        /// </summary>
        /// <param name="mappers"></param>
        /// <param name="optionsFactory"></param>
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
        /// <exception cref="ParseException">
        /// Thrown if <paramref name="arguments"/> contains an option that does not exist.
        /// </exception>
        /// <exception cref="ParseException">
        /// Thrown if <paramref name="arguments"/> contains an invalid option value.
        /// </exception>
        public virtual void Map(IArguments arguments, ICommand command)
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
                    throw new ParseException(
                        string.Format(Strings.ParseException_InvalidOptionValue, optionArg.Value, optionArg.Key),
                        innerException);
                }
            }
        }
    }
}
