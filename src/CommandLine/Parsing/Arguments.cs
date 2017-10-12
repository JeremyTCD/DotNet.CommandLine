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
        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        /// <param name="commandName">test</param>
        /// <param name="options">testa</param>
        public Arguments(string commandName, Dictionary<string, string> options)
        {
            OptionArgs = options;
            CommandName = commandName;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string CommandName { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Dictionary<string, string> OptionArgs { get; }
    }
}
