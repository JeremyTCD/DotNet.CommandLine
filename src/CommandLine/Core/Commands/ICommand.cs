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
        /// <value>
        /// Compared with command line arguments to select the command to run.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the command's description.
        /// </summary>
        /// <value>Used in help tips</value>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the command is the default command.
        /// </summary>
        /// <value>True if it is the default command; otherwise, false.</value>
        bool IsDefault { get; }

        /// <summary>
        /// Runs command.
        /// </summary>
        /// <param name="parseResult">Result of parsing command line arguments.</param>
        /// <param name="commandLineAppContext">Context of executing command line application.</param>
        /// <returns>Exit code.</returns>
        int Run(ParseResult parseResult, CommandLineAppContext commandLineAppContext);
    }
}
