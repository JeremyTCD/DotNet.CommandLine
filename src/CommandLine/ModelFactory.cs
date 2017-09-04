using JeremyTCD.DotNetCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JeremyTCD.DotNet.CommandLine
{
    public class ModelFactory : IModelFactory
    {
        private IActivatorService _activatorService { get; }
        private IEnumerable<IMapper> _mappers { get; }

        /// <summary>
        /// Creates a <see cref="ModelFactory"/> instance.
        /// </summary>
        /// <param name="activatorService"></param>
        /// <param name="mappers"></param>
        public ModelFactory(IActivatorService activatorService, IEnumerable<IMapper> mappers)
        {
            _activatorService = activatorService;
            _mappers = mappers;
        }

        /// <summary>
        /// Creates an instance of the type that <paramref name="command"/> maps to.
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
        public object Create(Arguments arguments, Command command)
        {
            object result = _activatorService.CreateInstance(command.ModelType);

            foreach (KeyValuePair<string, string> optionArg in arguments.OptionArgs)
            {
                Option option = command.
                    Options.
                    SingleOrDefault(o => o.ShortName == optionArg.Key || o.LongName == optionArg.Key);

                if (option == null)
                {
                    throw new ParseException(string.Format(Strings.Exception_OptionDoesNotExist, optionArg.Key));
                }

                bool propertySet = false;

                try
                {
                    foreach (IMapper mapper in _mappers)
                    {
                        if (mapper.TryMap(option.PropertyInfo, optionArg.Value, result))
                        {
                            propertySet = true;
                            break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new ParseException(string.Format(Strings.Exception_InvalidOptionValue, optionArg.Value, optionArg.Key), exception);
                }

                if (!propertySet)
                {
                    throw new ParseException(string.Format(Strings.Exception_InvalidOptionValue, optionArg.Value, optionArg.Key));
                }
            }

            return result;
        }
    }
}
