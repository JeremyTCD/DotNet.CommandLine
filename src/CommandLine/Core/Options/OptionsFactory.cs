// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class OptionsFactory : IOptionsFactory
    {
        private readonly IDictionary<ICommand, List<Option>> _optionsCache = new Dictionary<ICommand, List<Option>>();

        /// <inheritdoc/>
        public virtual List<Option> CreateFromCommand(ICommand command)
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
        /// Creates an <see cref="Option"/> from a <see cref="PropertyInfo"/>. Returns a new <see cref="Option"/> if successful; otherwise, if
        /// <see cref="PropertyInfo"/> does not contain an <see cref="OptionAttribute"/>, returns null.
        /// </summary>
        /// <param name="propertyInfo">
        /// The <see cref="PropertyInfo"/> used to create an <see cref="Option"/>. Must have an <see cref="OptionAttribute"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="Option"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="PropertyInfo"/>'s <see cref="OptionAttribute"/> has neither a long name or a short name.
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
