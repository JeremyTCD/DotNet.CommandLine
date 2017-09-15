namespace JeremyTCD.DotNet.CommandLine
{
    public class AppPrinterFactory : IAppPrinterFactory
    {
        private readonly IOptionsFactory _optionsFactory;

        public AppPrinterFactory(IOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        public IAppPrinter Create(AppContext appContext)
        {
            return new AppPrinter(appContext, _optionsFactory);
        }
    }
}
