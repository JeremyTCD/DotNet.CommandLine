namespace JeremyTCD.DotNet.CommandLine
{
    public class ParseResult
    {
        public ParseException ParseException { get; internal set; }
        public Command Command { get; internal set; }
        public object Model { get; internal set; }
    }
}