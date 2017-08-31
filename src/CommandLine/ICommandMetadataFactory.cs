﻿using System;

namespace JeremyTCD.DotNet.CommandLine
{
    public interface ICommandMetadataFactory
    {
        CommandMetadata CreateFromAttribute(CommandAttribute commandAttribute, Type commandModelType);
    }
}