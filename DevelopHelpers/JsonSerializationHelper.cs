using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DevelopHelpers
{
    /// <summary>
    /// Json序列化帮助类
    /// </summary>
    public  class JsonSerializationHelper
    {
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T obj)
        {
            string result = string.Empty;
            result = JsonConvert.SerializeObject(obj);
            return result;
        }

        /// <summary>
        /// 将对象序列化为json文本文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">对象</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool SerializeToJson<T>(T obj, string path)
        {
            bool result = false;
            StreamWriter writer = new StreamWriter(path, false, UTF8Encoding.UTF8);
            try
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
            }
            return result;
        }

        /// <summary>
        /// 将json字符串（或文件）转化为对象
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="isPath">是否为路径</param>
        /// <returns></returns>
        public static T DeSerializeFromJson<T>(string data, bool isPath = false)
        {
            T result = default(T);
            if (isPath)
            {
                if (!File.Exists(data))
                {
                    throw new Exception("文件不存在！");
                }
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(data, UTF8Encoding.UTF8);
                    result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
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
                result = JsonConvert.DeserializeObject<T>(data);
            }
            return result;
        }
    }
}
