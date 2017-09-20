// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class OptionsFactory : IOptionsFactory
    {
        private readonly IDictionary<ICommand, List<Option>> _optionsCache = new Dictionary<ICommand, List<Option>>();

        /// <summary>
        /// Creates a <see cref="List{T}"/> of <see cref="Option"/> instances from properties in <paramref name="command"/> that have an
        /// <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// <see cref="List{Option}"/>
        /// </returns>
        public List<Option> CreateFromCommand(ICommand command)
        {
            _optionsCache.TryGetValue(command, out List<Option> result);

            if (result == null)
            {
                result = new List<Option>();
                foreach (PropertyInfo propertyInfo in command.GetType().GetProperties())
                {
                    Option option = TryCreateFromPropertyInfo(propertyInfo);
                    if (option != null)
                    {
                        result.Add(option);
                    }
                }

                _optionsCache.Add(command, result);
            }

            return result;
        }

        /// <summary>
        /// Creates an <see cref="Option"/> instance from <paramref name="propertyInfo"/>'s <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// <see cref="PropertyInfo"/> instance with an <see cref="OptionAttribute"/>
        /// </param>
        /// <returns>
        /// <see cref="Option"/> if successful, null if <paramref name="propertyInfo"/> does not contain an <see cref="OptionAttribute"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <paramref name="propertyInfo"/> instance's <see cref="OptionAttribute"/> has neither a long name or a short name.
        /// </exception>
        internal virtual Option TryCreateFromPropertyInfo(PropertyInfo propertyInfo)
        {
            OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

            if (optionAttribute == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(optionAttribute.ShortName) && string.IsNullOrWhiteSpace(optionAttribute.LongName))
            {
                throw new InvalidOperationException(string.Format(Strings.Exception_OptionAttributeMustHaveName, propertyInfo.Name));
            }

            return new Option(propertyInfo, optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
