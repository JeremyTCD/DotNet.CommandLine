// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class ArgumentsFactory : IArgumentsFactory
    {
        /// <summary>
        /// Creates an <see cref="Arguments"/> instance from a string array. This function serves as an early filter for
        /// malformed arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>
        /// <see cref="Arguments"/>
        /// </returns>
        /// <exception cref="ParseException">
        /// Thrown if array contains null or whitespace elements.
        /// </exception>
        /// <exception cref="ParseException">
        /// Thrown if an element in <paramref name="args"/> other than the first element has the format of a command.
        /// </exception>
        public virtual Arguments CreateFromArray(string[] args)
        {
            int numArgs = args.Length;
            string commandName = null;
            Dictionary<string, string> options = new Dictionary<string, string>();

            for (int i = 0; i < numArgs; i++)
            {
                string arg = args[i];
                bool isMalformed = false;

                if (string.IsNullOrWhiteSpace(arg))
                {
                    isMalformed = true;
                }
                else
                {
                    if (arg.StartsWith("-"))
                    {
                        string[] split = arg.Split('=');

                        options.Add(split[0].Remove(0, 1), split.Length > 1 ? split[1] : null);
                    }
                    else if (i == 0)
                    {
                        commandName = arg;
                    }
                    else
                    {
                        isMalformed = true;
                    }
                }

                if (isMalformed)
                {
                    throw new ParseException(string.Format(Strings.ParseException_MalformedArguments, string.Join(" ", args)));
                }
            }

            return new Arguments(commandName, options);
        }
    }
}
