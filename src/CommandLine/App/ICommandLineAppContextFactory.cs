// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a factory that creates <see cref="CommandLineAppContext"/> instances.
    /// </summary>
    public interface ICommandLineAppContextFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CommandLineAppContext"/> class.
        /// </summary>
        /// <param name="commandDictionary">The <see cref="CommandLineAppContext"/> instance's <see cref="CommandDictionary"/></param>
        /// <param name="appOptions">The <see cref="CommandLineAppContext"/> instance's <see cref="CommandLineAppOptions"/></param>
        /// <returns><see cref="CommandLineAppContext"/></returns>
        CommandLineAppContext Create(CommandDictionary commandDictionary, CommandLineAppOptions appOptions);
    }
}
