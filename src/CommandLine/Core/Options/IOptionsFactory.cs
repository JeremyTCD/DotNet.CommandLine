// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a factory that creates <see cref="List{T}"/> of <see cref="Option"/>s.
    /// </summary>
    public interface IOptionsFactory
    {
        /// <summary>
        /// Creates a <see cref="List{T}"/> of <see cref="Option"/>s from an <see cref="ICommand"/>. Each <see cref="Option"/> is created
        /// from a property that has an <see cref="OptionAttribute"/>. This function serves as an early filter for invalid options.
        /// </summary>
        /// <param name="command"><see cref="ICommand"/> to create list of <see cref="Option"/>s from.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="Option"/>s. </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an <see cref="OptionAttribute"/> has neither a long name or a short name.
        /// </exception>
        List<Option> CreateFromCommand(ICommand command);
    }
}