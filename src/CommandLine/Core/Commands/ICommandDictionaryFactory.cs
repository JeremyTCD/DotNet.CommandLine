// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a factory that creates an <see cref="ICommandDictionary"/>s.
    /// </summary>
    public interface ICommandDictionaryFactory
    {
        /// <summary>
        /// Creates an instance of a class that implements <see cref="ICommandDictionary"/> from a collection of <see cref="ICommand"/>s.
        /// This function serves as a filter for incompatible/invalid commands.
        /// </summary>
        /// <param name="commands">The collection whose elements are used to populate the new <see cref="ICommandDictionary"/>.</param>
        /// <returns>A new command dictionary.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if there are multiple default commands.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an <see cref="ICommand"/>'s name property is null or whitespace.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if multiple commands have the same name.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if there is no default command.
        /// </exception>
        ICommandDictionary Create(IEnumerable<ICommand> commands);
    }
}
