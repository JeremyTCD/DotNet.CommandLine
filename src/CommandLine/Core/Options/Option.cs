// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Option
    {
        internal Option(PropertyInfo propertyInfo, string shortName, string longName, string description)
        {
            PropertyInfo = propertyInfo;
            ShortName = shortName;
            LongName = longName;
            Description = description;
        }

        public virtual PropertyInfo PropertyInfo { get; }

        public virtual string ShortName { get; }

        public virtual string LongName { get; }

        public virtual string Description { get; }
    }
}
