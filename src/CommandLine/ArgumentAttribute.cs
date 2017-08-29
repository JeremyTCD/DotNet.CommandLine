using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class ArgumentAttribute : Attribute
    {
        public Type ResourceType { get; set; }

        public string GetResource(string resourceName)
        {
            if (ResourceType == null)
            {
                throw new InvalidOperationException(Strings.Exception_, GetType().Name);
            }

            if (resourceName == null)
            {
                throw new InvalidOperationException(Strings.Exception_, GetType().Name);

            }

            PropertyInfo propertyInfo = ResourceType.GetTypeInfo().GetDeclaredProperty(resourceName);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(Strings.Exception_, GetType().Name);
            }

            if (!propertyInfo.GetMethod.IsStatic)
            {

            }

            return (string) propertyInfo.GetValue(null, null);
        }
    }
}
