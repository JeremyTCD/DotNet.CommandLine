// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandMapper
    {
        void Map(IArguments arguments, ICommand command);
    }
}