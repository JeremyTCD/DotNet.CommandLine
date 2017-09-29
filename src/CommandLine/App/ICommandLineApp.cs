// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application.
    /// </summary>
    public interface ICommandLineApp
    {
        /// <summary>
        /// Runs the command specified by the given command line arguments. If the command line arguments do not
        /// specify a command, runs the default command.
        /// </summary>
        /// <param name="args">The command line arguments. Can be null if there are no arguments.</param>
        /// <returns>Exit code.</returns>
        int Run(string[] args);
    }
}
