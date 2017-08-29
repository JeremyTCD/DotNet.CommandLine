using System;

namespace JeremyTCD.DotNet.CommandLine.src
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

        public string ShortName
        {
            get
            {
                return _shortName ?? (_shortName = GetResource(_shortNameResourceName));
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
                return _longName ?? (_longName = GetResource(_longNameResourceName));
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
                return _description ?? (_description = GetResource(_descriptionResourceName));
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Creates a <see cref="VerbAttribute"/> instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public OptionAttribute(string shortNameResourceName = null, string longNameResourceName = null, string descriptionResourceName = null)
        {
            _shortNameResourceName = shortNameResourceName;
            _longNameResourceName = longNameResourceName;
            _descriptionResourceName = descriptionResourceName;
        }
    }
}
