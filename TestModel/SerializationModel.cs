using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TestModel
{
    [Serializable]
    public class SerializationModel
    {
        public string name { get; set; }

        public string passWord { get; set; }
    }
}
