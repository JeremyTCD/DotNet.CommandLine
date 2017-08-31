using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CommandMetadata
    {
        public Type CommandModelType { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsDefault { get; }

        public IEnumerable<OptionMetadata> OptionMetadata {get;}

        public CommandMetadata(Type commandModelType, bool isDefault, string name, string description, IEnumerable<OptionMetadata> optionMetadata)
        {
            CommandModelType = commandModelType;
            Name = name;
            Description = description;
            OptionMetadata = optionMetadata;
            IsDefault = isDefault;
        }
    }
}
