namespace JeremyTCD.DotNet.CommandLine
{
    public class ParseResult
    {
        public readonly ParseException ParseException;
        public readonly Command Command;
        public readonly object Model;

        public ParseResult(ParseException parseException, Command command, object model)
        {
            ParseException = parseException;
            Command = command;
            Model = model;
        }
    }
}