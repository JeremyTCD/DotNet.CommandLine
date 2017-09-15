namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContext
    {
        public readonly CommandSet CommandSet;
        public readonly AppOptions AppOptions;

        public AppContext(CommandSet commandSet, AppOptions appOptions)
        {
            CommandSet = commandSet;
            AppOptions = appOptions;
        }
    }
}
