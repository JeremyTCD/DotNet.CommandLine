namespace JeremyTCD.DotNet.CommandLine
{
    public interface IArgumentsFactory
    {
        Arguments CreateFromArray(string[] args);
    }
}