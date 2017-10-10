// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Represents a command line application's options.
    /// </summary>
    public class CommandLineAppOptions
    {
        /// <summary>
        /// Gets or sets the name of the command line application executable.
        /// </summary>
        public string ExecutableName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the command line application.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value representing the command line application's version.
        /// </summary>
        public string Version { get; set; }
    }
}
