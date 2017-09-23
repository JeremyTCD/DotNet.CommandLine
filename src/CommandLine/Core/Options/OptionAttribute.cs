// Copyright (c) JeremyTCD. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JeremyTCD.DotNet.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OptionAttribute : ArgumentAttribute
    {
        private string _shortNameResourceName;
        private string _longNameResourceName;
        private string _descriptionResourceName;
        private string _shortName;
        private string _longName;
        private string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="shortNameResourceName"></param>
        /// <param name="longNameResourceName"></param>
        /// <param name="descriptionResourceName"></param>
        public OptionAttribute(Type resourceType = null, string shortNameResourceName = null, string longNameResourceName = null, string descriptionResourceName = null)
            : base(resourceType)
        {
            _shortNameResourceName = shortNameResourceName;
            _longNameResourceName = longNameResourceName;
            _descriptionResourceName = descriptionResourceName;
        }

        public string ShortName
        {
            get
            {
                return _shortName ?? (_shortName = TryGetResource(_shortNameResourceName));
            }

            set
            {
                _shortName = value;
            }
        }

        public string LongName
        {
            get
            {
                return _longName ?? (_longName = TryGetResource(_longNameResourceName));
            }

            set
            {
                _longName = value;
            }
        }

        public string Description
        {
            get
            {
                return _description ?? (_description = TryGetResource(_descriptionResourceName));
            }

            set
            {
                _description = value;
            }
        }
    }
}
