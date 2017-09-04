using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IOptionFactory
    {
        Option TryCreateFromPropertyInfo(PropertyInfo propertyInfo);
    }
}