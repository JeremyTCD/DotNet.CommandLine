// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Option
    {
        public PropertyInfo PropertyInfo { get; }
        public string ShortName { get; }
        public string LongName { get; }
        public string Description { get; }

        internal Option(PropertyInfo propertyInfo, string shortName, string longName, string description)
        {
            PropertyInfo = propertyInfo;
            ShortName = shortName;
            LongName = longName;
            Description = description;
        }
    }
}
