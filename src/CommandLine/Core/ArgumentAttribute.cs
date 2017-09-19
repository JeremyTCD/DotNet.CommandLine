// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public string TryGetResource(string resourceName)
        {
            if (_resourceType == null || resourceName == null)
            {
                return null;
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
