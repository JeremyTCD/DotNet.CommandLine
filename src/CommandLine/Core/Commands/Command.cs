namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsDefault { get; }

        [Option(typeof(Strings), nameof(Strings.OptionShortName_Help), nameof(Strings.OptionLongName_Help), nameof(Strings.OptionDescription_Help))]
        public bool Help { get; }

        public virtual int Run(ParseResult parseResult, AppContext appContext)
        {
            if(parseResult.ParseException != null){
                appContext.
                    AppPrinter.
                    AppendParseException(parseResult.ParseException).
                    AppendGetHelpTip(IsDefault ? "this application" : "this command", IsDefault ? null : Name).
                    Print();

                return 0;
            }

            if (Help)
            {
                if (IsDefault)
                {
                    appContext.
                        AppPrinter.
                        AppendAppHelp();
                }
                else
                {
                    appContext.
                        AppPrinter.
                        AppendCommandHelp(Name);
                }

                appContext.
                    AppPrinter.
                    Print();

                return 1;
            }

            return RunCommand(parseResult, appContext);
        }

        public abstract int RunCommand(ParseResult parseResult, AppContext appContext);
    }
}
