// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <inheritdoc/>
    public class OptionFactory : IOptionFactory
    {
        /// <inheritdoc/>
        public virtual IOption TryCreate(PropertyInfo propertyInfo)
        {
            OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

            if (optionAttribute == null)
            {
                return null;
            }

            return CreateCore(propertyInfo, optionAttribute);
        }

        /// <inheritdoc/>
        public virtual IOption Create(PropertyInfo propertyInfo)
        {
            OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();

            if (optionAttribute == null)
            {
                // TODO message
                throw new ArgumentException(string.Format(Strings.ArgumentException_PropertyInfoMustHaveOptionAttribute, propertyInfo.Name));
            }

            return CreateCore(propertyInfo, optionAttribute);
        }

        internal virtual Option CreateCore(PropertyInfo propertyInfo, OptionAttribute optionAttribute)
        {
            if (string.IsNullOrWhiteSpace(optionAttribute.ShortName) && string.IsNullOrWhiteSpace(optionAttribute.LongName))
            {
                throw new InvalidOperationException(string.Format(Strings.Exception_OptionAttributeMustHaveName, propertyInfo.Name));
            }

            return new Option(propertyInfo, optionAttribute.ShortName, optionAttribute.LongName, optionAttribute.Description);
        }
    }
}
