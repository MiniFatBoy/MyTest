using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestModel
{
    [Serializable]
    public class ProcessInfo
    {

        public int ProceID { get; set; }
        public string ProceName { get; set; }
        public string ExecPath { get; set; }
        public string CommandLine { get; set; }

        public string Arguments
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CommandLine))
                {
                    return CommandLine.Substring(CommandLine.LastIndexOf('"')+2);
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
