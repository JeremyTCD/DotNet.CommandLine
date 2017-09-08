namespace JeremyTCD.DotNet.CommandLine
{
    public class ParseResult
    {
        public readonly ParseException ParseException;
        public readonly ICommand Command;
        public readonly CommandSet CommandSet;

        public ParseResult(ParseException parseException, ICommand command, CommandSet commandSet)
        {
            ParseException = parseException;
            Command = command;
            CommandSet = commandSet;
        }
    }
}