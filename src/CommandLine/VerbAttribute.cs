using System;

namespace JeremyTCD.DotNet.CommandLine
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class VerbAttribute : ArgumentAttribute
    {
        private string _nameResourceName;
        private string _descriptionResourceName;
        private string _name;
        private string _description;

        public string Name
        {
            get
            {
                return _name ?? (_name = GetResource(_nameResourceName));
            }
            set
            {
                _name = value;
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
        public VerbAttribute(string nameResourceName = null, string descriptionResourceName = null)
        {
            _nameResourceName = nameResourceName;
            _descriptionResourceName = descriptionResourceName;
        }
    }
}
