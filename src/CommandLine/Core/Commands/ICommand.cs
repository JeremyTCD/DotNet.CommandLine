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
        /// Gets the command's name. If the name matches the command name specified in the command line arguments, the command
        /// is run. The name is also used when printing usage examples.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command's description. The description used when printing help tips.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the command is the default command. True if the command is the default command; otherwise, false.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Runs the command. Returns an exit code.
        /// </summary>
        /// <param name="parseResult">Result of parsing command line arguments.</param>
        /// <param name="commandLineAppContext">Context of executing command line application.</param>
        /// <returns>Exit code.</returns>
        int Run(ParseResult parseResult, ICommandLineAppContext commandLineAppContext);
    }
}
