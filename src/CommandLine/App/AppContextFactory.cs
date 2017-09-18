namespace JeremyTCD.DotNet.CommandLine
{
    public class AppContextFactory : IAppContextFactory
    {
        private IAppPrinterFactory _appPrinterFactory;

        public AppContextFactory(IAppPrinterFactory appPrinterFactory)
        {
            _appPrinterFactory = appPrinterFactory;
        }

        public AppContext Create(CommandSet commandSet, AppOptions appOptions)
        {
            IAppPrinter appPrinter = _appPrinterFactory.Create(commandSet, appOptions);

            return new AppContext(commandSet, appOptions, appPrinter);
        }
    }
}
