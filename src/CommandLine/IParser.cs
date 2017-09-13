namespace JeremyTCD.DotNet.CommandLine
{
    public interface IParser
    {
        ParseResult Parse(string[] args, ICommandSet commandSet);
    }
}
