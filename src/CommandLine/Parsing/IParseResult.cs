namespace JeremyTCD.DotNet.CommandLine
{
    public interface IParseResult
    {
        ICommand Command { get; }
        ParseException ParseException { get; }
    }
}