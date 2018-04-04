using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DevelopHelpers
{
    /// <summary>
    /// Xml序列化帮助类
    /// </summary>
    public static class XmlSerializationHelper
    {
        /// <summary>
        ///  序列化为XML文件
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="obj">序列化对象</param>
        /// <param name="path">路径</param>
        public static void SerializeToXml<T>(T obj, string path)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path, false, UTF8Encoding.UTF8);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(writer, obj);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
            }
        }
        /// <summary>
        /// 序列化为XML文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T obj)
        {
            StringBuilder builder = new StringBuilder();
            TextWriter writer = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                writer = new StringWriter(builder);
                xs.Serialize(writer, obj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                writer.Close();
            }
            return builder.ToString();
        }

        /// <summary>
        /// 反序列化XML文件或文本
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="data">xml文件路径或者xml文本</param>
        /// <param name="isPath">是否是路径</param>
        /// <returns></returns>
        public static T DeSerializeToXml<T>(string data, bool isPath = true)
        {
            T result = default(T);
            if (isPath)
            {
                if (! File.Exists(data))
                {
                    throw new Exception("文件不存在！");
                }
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(data, UTF8Encoding.UTF8);
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    result = (T)xs.Deserialize(reader);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                TextReader reader = null;
                try
                {
                    StringBuilder buffer = new StringBuilder();
                    buffer.Append(data);
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    reader = new StringReader(buffer.ToString());
                    result = (T)xs.Deserialize(reader);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            return result;

        }
    }
}
