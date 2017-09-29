// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application's context.
    /// </summary>
    public interface ICommandLineAppContext
    {
        /// <summary>
        /// Gets the command line application's <see cref="ICommandDictionary"/>.
        /// </summary>
        ICommandDictionary CommandDictionary { get; }

        /// <summary>
        /// Gets the command line application's <see cref="CommandLine.CommandLineAppOptions"/>.
        /// </summary>
        CommandLineAppOptions CommandLineAppOptions { get; }

        /// <summary>
        /// Gets the command line application's <see cref="ICommandLineAppPrinter"/>.
        /// </summary>
        ICommandLineAppPrinter CommandLineAppPrinter { get; }
    }
}