// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JeremyTCD.DotNetCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class CollectionMapper : IMapper
    {
        private IActivatorService _activatorService { get; }

        public CollectionMapper(IActivatorService activatorService)
        {
            _activatorService = activatorService;
        }

        /// <summary>
        /// If <paramref name="value"/> is not null and the property represented by <paramref name="propertyInfo"/> is assignable to
        /// <see cref="ICollection{T}"/>, sets the corresponding property in <paramref name="command"/> to an <see cref="ICollection{T}"/> 
        /// containing the result of splitting <paramref name="value"/> using ',' as separator.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <param name="command"></param>
        /// <returns>
        /// True if property was set succesfully, false otherwise.
        /// </returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        public bool TryMap(PropertyInfo propertyInfo, string value, ICommand command)
        {
            if(value == null)
            {
                return false;
            }

            IEnumerable<Type> collectionTypes = propertyInfo.
                PropertyType.
                GetTypeInfo().
                GetInterfaces().
                Where(t => t.GetTypeInfo().IsGenericType == true && t.GetGenericTypeDefinition() == typeof(ICollection<>));

            if(collectionTypes.Count() != 1)
            {
                return false;
            }

            Type genericType = collectionTypes.
                First().
                GetGenericArguments()[0];

            object collection = _activatorService.CreateInstance(propertyInfo.PropertyType);
            MethodInfo add = propertyInfo.PropertyType.GetMethod("Add");
            foreach (string subValue in value.Split(','))
            {
                add.Invoke(collection, new[] { Convert.ChangeType(subValue, genericType) });
            }

            propertyInfo.SetValue(command, collection);

            return true;
        }
    }
}
