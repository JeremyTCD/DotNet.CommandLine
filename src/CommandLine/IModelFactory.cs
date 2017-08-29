using System;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IModelFactory
    {
        object Create(Arguments arguments, Type modelType);
    }
}