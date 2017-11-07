// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a factory that creates <see cref="IOption"/>s.
    /// </summary>
    public interface IOptionFactory
    {
        /// <summary>
        /// Creates an <see cref="IOption"/> from the <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The <see cref="PropertyInfo"/> used to create an <see cref="IOption"/>. An <see cref="ArgumentException"/> is thrown if
        /// it does not contain an <see cref="OptionAttribute"/>.
        /// </param>
        /// <returns>
        /// The new <see cref="IOption"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="propertyInfo"/> has no <see cref="OptionAttribute"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="PropertyInfo"/>'s <see cref="OptionAttribute"/> has neither a long name nor a short name.
        /// </exception>
        IOption Create(PropertyInfo propertyInfo);

        /// <summary>
        /// Creates an <see cref="IOption"/> from the <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">
        /// The <see cref="PropertyInfo"/> used to create an <see cref="IOption"/>. null is returned if it does not contain an <see cref="OptionAttribute"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="IOption"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="PropertyInfo"/>'s <see cref="OptionAttribute"/> has neither a long name nor a short name.
        /// </exception>
        IOption TryCreate(PropertyInfo propertyInfo);
    }
}