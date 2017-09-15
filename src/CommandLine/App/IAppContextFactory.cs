namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppContextFactory
    {
        AppContext Create(CommandSet commandSet, AppOptions appOptions);
    }
}
