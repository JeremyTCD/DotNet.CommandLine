namespace JeremyTCD.DotNet.CommandLine
{
    public class ArgumentsFactory : IArgumentsFactory
    {
        public Arguments CreateFromArray(string[] args)
        {
            Arguments result = new Arguments();
            foreach(string arg in args)
            {
                if (arg.StartsWith("--") || arg.StartsWith("-"))
                {
                    result.Options.Add(arg);
                }
                else if(result.Verb == null)
                {
                    result.Verb = arg;
                }
                else
                {
                    // TODO verb already specified, throw
                }
            }

            return result;
        }
    }
}
