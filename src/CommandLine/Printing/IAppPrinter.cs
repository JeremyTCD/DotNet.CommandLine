namespace JeremyTCD.DotNet.CommandLine
{
    public interface IAppPrinter
    {
        void Clear();
        void Print();
        void AppendHeader();
        void AppendAppHelp(string rowPrefix, int columnGap);
        void AppendCommandHelp(string commandName, string rowPrefix, int columnGap);
        void AppendGetHelpTip(string commandPosValue, string targetPosValue);
        void AppendUsage(string optionsPosValue, string commandPosValue);
    }
}
