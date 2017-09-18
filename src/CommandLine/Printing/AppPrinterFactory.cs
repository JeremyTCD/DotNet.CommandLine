namespace JeremyTCD.DotNet.CommandLine
{
    public class AppPrinterFactory : IAppPrinterFactory
    {
        private readonly IOptionsFactory _optionsFactory;

        public AppPrinterFactory(IOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        public IAppPrinter Create(CommandSet commandSet, AppOptions appOptions)
        {
            return new AppPrinter(commandSet, appOptions, _optionsFactory);
        }
    }
}
