using System;
using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandSetFactory
    {
        CommandSet CreateFromTypes(IEnumerable<Type> types);
    }
}
