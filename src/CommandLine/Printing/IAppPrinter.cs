namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppPrinter
    {
        void Clear();
        void Print();
        void AppendHeader();
        void AppendAppHelp(string rowPrefix = null, int columnGap = 2);
        void AppendCommandHelp(string commandName, string rowPrefix = null, int columnGap = 2);
        void AppendGetHelpTip(string targetPosValuestring, string commandPosValue = null);
        void AppendUsage(string optionsPosValue, string commandPosValue = null);
    }
}
