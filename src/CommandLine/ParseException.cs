﻿using System;

namespace JeremyTCD.DotNet.CommandLine
{
    /// <summary>
    /// Exception containing an error message pertaining to parsing that is suitable for end user consumption.
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// Creates a <see cref="ParseException"/> instance with a default parse error message.
        /// </summary>
        public ParseException() : base(Strings.Exception_InvalidArguments) { }

        /// <summary>
        /// Creates a <see cref="ParseException"/> instance.
        /// </summary>
        /// <param name="userMessage">Must be suitable for end user consumption.</param>
        public ParseException(string userMessage) : base(userMessage) { }

        /// <summary>
        /// Creates a <see cref="ParseException"/> instance.
        /// </summary>
        /// <param name="userMessage">Must be suitable for end user consumption.</param>
        /// <param name="innerException"></param>
        public ParseException(string userMessage, Exception innerException) : base(userMessage, innerException) { }
    }
}
