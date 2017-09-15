namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppPrinterFactory
    {
        IAppPrinter Create(AppContext appContext);
    }
}
