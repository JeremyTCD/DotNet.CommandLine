namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandLineTool
    {
        int Run(string[] args, PrinterOptions printerOptions);
    }
}
