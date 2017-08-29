using JeremyTCD.DotNetCore.Utils;
using System;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine.src
{
    public class ModelFactory : IModelFactory
    {
        private IActivatorService _activatorService { get; }

        public ModelFactory(IActivatorService activatorService)
        {
            _activatorService = activatorService;
        }

        public object Create(Arguments arguments, Type modelType)
        {
            object result = _activatorService.CreateInstance(modelType);

            foreach(PropertyInfo propertyInfo in result.GetType().GetRuntimeProperties())
            {
                OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();
                if(optionAttribute != null)
                {
                    // check mode
                    // if required, extract value(s) from arguments
                }
            }

            return result;
        }
    }
}
