// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a factory that creates <see cref="CommandDictionary"/> instances.
    /// </summary>
    public interface ICommandDictionaryFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CommandDictionary"/> class from collection of <see cref="ICommand"/> instances.
        /// </summary>
        /// <param name="commands">The collection whose elements are used to populate the new <see cref="CommandDictionary"/>.</param>
        /// <returns><see cref="CommandDictionary"/></returns>
        CommandDictionary CreateFromCommands(IEnumerable<ICommand> commands);
    }
}
