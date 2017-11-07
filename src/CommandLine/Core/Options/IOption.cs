using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOption
    {
        string Description { get; }

        string LongName { get; }

        PropertyInfo PropertyInfo { get; }

        string ShortName { get; }
    }
}