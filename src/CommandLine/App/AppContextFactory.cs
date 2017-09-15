namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContextFactory : IAppContextFactory
    {
        public AppContext Create(CommandSet commandSet, AppOptions appOptions)
        {
            return new AppContext(commandSet, appOptions);
        }
    }
}
