// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application. Exposes the entry method for the command line application,
    /// <see cref="Run(string[])"/>. Additionally, serves as the dependency injection root for the library.
    /// </summary>
    public interface ICommandLineApp
    {
        /// <summary>
        /// Entry method for the command line application. Returns an exit code.
        /// </summary>
        /// <param name="args">Raw command line arguments.</param>
        /// <returns>Exit code.</returns>
        int Run(string[] args);
    }
}
