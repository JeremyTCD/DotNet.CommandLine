// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a collection of command-name/command pairs.
    /// </summary>
    public interface ICommandDictionary : IDictionary<string, ICommand>
    {
        /// <summary>
        /// Gets the command dictionaries default command.
        /// </summary>
        ICommand DefaultCommand { get; }
    }
}
