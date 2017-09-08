namespace JeremyTCD.DotNet.CommandLine
{
    public class ParseResult
    {
        public readonly ParseException ParseException;
        public readonly ICommand Command;

        public ParseResult(ParseException parseException, ICommand command)
        {
            ParseException = parseException;
            Command = command;
        }
    }
}