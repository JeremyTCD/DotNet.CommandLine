namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppPrinter
    {
        IAppPrinter Clear();
        IAppPrinter Print();
        IAppPrinter AppendHeader();
        IAppPrinter AppendAppHelp(string rowPrefix = null, int columnGap = 2);
        IAppPrinter AppendCommandHelp(string commandName, string rowPrefix = null, int columnGap = 2);
        IAppPrinter AppendGetHelpTip(string targetPosValuestring, string commandPosValue = null);
        IAppPrinter AppendUsage(string optionsPosValue, string commandPosValue = null);
        IAppPrinter AppendParseException(ParseException parseException);
    }
}
