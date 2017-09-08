namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandMapper
    {
        void Map(Arguments arguments, ICommand command);
    }
}