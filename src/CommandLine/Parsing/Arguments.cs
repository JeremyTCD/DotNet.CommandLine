// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Convenience type. Representation of an array of strings passed as command line arguments.
    /// </summary>
    public class Arguments
    {
        public string CommandName { get; }
        public Dictionary<string, string> OptionArgs { get; }
        
        /// <summary>
        /// Creates an <see cref="Arguments"/> instance.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="options"></param>
        public Arguments(string commandName, Dictionary<string, string> options)
        {
            OptionArgs = options;
            CommandName = commandName;
        }
    }
}
