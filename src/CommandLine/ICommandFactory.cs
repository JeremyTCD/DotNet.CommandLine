using System;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandFactory
    {
        Command CreateFromAttribute(CommandAttribute commandAttribute, Type commandModelType);
    }
}