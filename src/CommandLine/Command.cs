using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Command
    {
        public Type ModelType { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsDefault { get; }

        public IEnumerable<Option> Options {get;}

        public Command(Type commandModelType, bool isDefault, string name, string description, IEnumerable<Option> options)
        {
            ModelType = commandModelType;
            Name = name;
            Description = description;
            Options = options;
            IsDefault = isDefault;
        }
    }
}
