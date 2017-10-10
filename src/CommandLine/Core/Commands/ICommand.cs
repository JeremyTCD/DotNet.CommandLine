// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the command's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command's description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the command is the default command.
        bool IsDefault { get; }

        /// <summary>
        /// Runs the command.
        /// </summary>
        /// <param name="parseResult">The result from parsing command line arguments.</param>
        /// <param name="commandLineAppContext">The command line application's context.</param>
        /// <returns>Exit code.</returns>
        int Run(IParseResult parseResult, ICommandLineAppContext commandLineAppContext);
    }
}
