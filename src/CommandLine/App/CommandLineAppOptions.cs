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
        /// Gets or sets the name of the command line application executable. The executable name is used when
        /// printing usage examples.
        /// </summary>
        public string ExecutableName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the command line application. The full name is used when
        /// printing headers.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the command line application's version. The version is used when
        /// printing headers.
        /// </summary>
        public string Version { get; set; }
    }
}
