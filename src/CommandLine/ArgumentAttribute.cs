using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class ArgumentAttribute : Attribute
    {
        protected Type _resourceType { get; set; }

        public ArgumentAttribute(Type resourceType)
        {
            _resourceType = resourceType;
        }

        public string GetResource(string resourceName)
        {
            if (_resourceType == null)
            {
                //throw new InvalidOperationException(Strings.Exception_, GetType().Name);
            }

            if (resourceName == null)
            {
                //throw new InvalidOperationException(Strings.Exception_, GetType().Name);

            }

            PropertyInfo propertyInfo = _resourceType.GetTypeInfo().GetDeclaredProperty(resourceName);

            if (propertyInfo == null)
            {
                //throw new InvalidOperationException(Strings.Exception_, GetType().Name);
            }

            if (!propertyInfo.GetMethod.IsStatic)
            {

            }

            return (string) propertyInfo.GetValue(null, null);
        }
    }
}
