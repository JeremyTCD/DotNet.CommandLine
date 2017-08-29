using JeremyTCD.DotNet.CommandLine.src;
using System;
using System.Linq;
using System.Reflection;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Parser
    {
        private IArgumentsFactory _argumentsFactory { get; }
        private IModelFactory _modelFactory { get; }

        public Parser(IArgumentsFactory argumentsFactory, IModelFactory modelFactory)
        {
            _argumentsFactory = argumentsFactory;
            _modelFactory = modelFactory;
        }

        public ParseResult Parse(string[] args, Type defaultModelType = null, params Type[] modelTypes)
        {
            Arguments arguments = _argumentsFactory.CreateFromArray(args);
            Type modelType = MatchModelType(arguments.Verb, defaultModelType, modelTypes);
            object model = _modelFactory.Create(arguments, modelType);

            // Create instance, populate properties

            // Create ParseResult
            return null;
        }

        private Type MatchModelType(string verb, Type defaultModelType, Type[] modelTypes)
        {
            if (string.IsNullOrWhiteSpace(verb))
            {
                if(defaultModelType == null)
                {
                    // No verb specified and no default specified, print main help (all commands)
                }
                return defaultModelType;
            }

            foreach(Type type in modelTypes)
            {
                VerbAttribute verbAttribute = type.GetTypeInfo().GetCustomAttribute<VerbAttribute>();

                if(verbAttribute == null)
                {
                    // throw, every class should have a verb
                }

                if(verbAttribute.Name == verb)
                {
                    return type;
                }
            }

            // Verb does not match any existing verb, print main help (all commands)
            return null;
        }
    }
}
