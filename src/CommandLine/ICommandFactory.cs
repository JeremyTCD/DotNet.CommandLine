using System;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandFactory
    {
        Command TryCreateFromType(Type modelType);
    }
}