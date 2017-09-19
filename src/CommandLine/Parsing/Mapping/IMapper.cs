// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IMapper
    {
        bool TryMap(PropertyInfo propertyInfo, string value, ICommand command);
    }
}
