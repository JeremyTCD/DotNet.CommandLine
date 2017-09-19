// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        bool IsDefault { get; }

        int Run(ParseResult parseResult, AppContext appContext);
    }
}
