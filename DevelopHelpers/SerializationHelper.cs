using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DevelopHelpers
{
    public static class SerializationHelper
    {
        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static T DeepClone<T>(T source) where T : class
        {
            T result = null;
            MemoryStream stream = new MemoryStream();
            var binary = new BinaryFormatter();
            binary.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            result =(T)binary.Deserialize(stream);
            stream.Close();
            stream.Dispose();
            return result;
        }
    }
}
