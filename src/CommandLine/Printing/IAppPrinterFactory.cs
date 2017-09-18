namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppPrinterFactory
    {
        IAppPrinter Create(CommandSet commandSet, AppOptions appOptions);
    }
}
