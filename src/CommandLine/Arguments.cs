using System.Collections.Generic;

namespace JeremyTCD.DotNet.CommandLine
{
    public class Arguments
    {
        public string Verb { get; set; }
        public List<string> Options { get; }
        
        public Arguments()
        {
            Options = new List<string>();
        }
    }
}
