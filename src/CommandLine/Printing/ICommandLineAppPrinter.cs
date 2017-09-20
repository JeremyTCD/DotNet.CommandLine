// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandLineAppPrinter
    {
        ICommandLineAppPrinter Clear();

        ICommandLineAppPrinter Print();

        ICommandLineAppPrinter AppendLine();

        ICommandLineAppPrinter AppendHeader();

        ICommandLineAppPrinter AppendAppHelp(string rowPrefix = null, int columnGap = 2);

        ICommandLineAppPrinter AppendCommandHelp(string commandName, string rowPrefix = null, int columnGap = 2);

        ICommandLineAppPrinter AppendGetHelpTip(string targetPosValuestring, string commandPosValue = null);

        ICommandLineAppPrinter AppendUsage(string optionsPosValue, string commandPosValue = null);

        ICommandLineAppPrinter AppendParseException(ParseException parseException);
    }
}
