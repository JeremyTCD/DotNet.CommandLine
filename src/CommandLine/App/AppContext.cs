namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContext
    {
        public readonly CommandSet CommandSet;
        public readonly AppOptions AppOptions;
        public readonly IAppPrinter AppPrinter;

        public AppContext(CommandSet commandSet, AppOptions appOptions, IAppPrinter appPrinter)
        {
            AppPrinter = appPrinter;
            CommandSet = commandSet;
            AppOptions = appOptions;
        }
    }
}
