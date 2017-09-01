using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface IMapper
    {
        bool TryMap(PropertyInfo propertyInfo, string value, object target);
    }
}
