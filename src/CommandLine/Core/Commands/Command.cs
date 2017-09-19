namespace JeremyTCD.DotNet.CommandLine
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsDefault { get; }

        [Option(typeof(Strings), nameof(Strings.OptionShortName_Help), nameof(Strings.OptionLongName_Help), nameof(Strings.OptionDescription_Help))]
        public bool Help { get; set; }

        /// <summary>
        /// If <paramref name="parseResult"/> contains a <see cref="ParseException"/> instance, prints exception and a get help tip before returning 0.
        /// If <see cref="Help"/> is true, prints help and returns 1. Otherwise, calls <see cref="RunCommand(ParseResult, AppContext)"/>.
        /// </summary>
        /// <param name="parseResult"></param>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public virtual int Run(ParseResult parseResult, AppContext appContext)
        {
            appContext.
                AppPrinter.
                AppendHeader().
                AppendLine();

            if(parseResult.ParseException != null){
                appContext.
                    AppPrinter.
                    AppendParseException(parseResult.ParseException).
                    AppendLine().
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

            appContext.
                AppPrinter.
                Print();

            return RunCommand(parseResult, appContext);
        }

        public abstract int RunCommand(ParseResult parseResult, AppContext appContext);
    }
}
