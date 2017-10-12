// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public class ParseResult : IParseResult
    {
        public ParseResult(ParseException parseException, ICommand command)
        {
            ParseException = parseException;
            Command = command;
        }

        public virtual ParseException ParseException { get; }

        public virtual ICommand Command { get; }
    }
}