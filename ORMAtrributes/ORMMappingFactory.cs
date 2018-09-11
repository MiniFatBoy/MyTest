using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ORMAtrributes
{
    /// <summary>
    /// 映射处理工厂类
    /// </summary>
    public static class ORMMappingFactory
    {
        // <summary>
        /// 获取映射表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMappingTableName(this Type type)
        {
            if (type.IsDefined(typeof(MappingTableAttribute), true))
            {
                MappingTableAttribute attribute = (MappingTableAttribute)type.GetCustomAttribute(typeof(MappingTableAttribute), true);
                return attribute.GetMapingName();
            }
            else
            {
                return type.Name;
            }
        }

        // <summary>
        /// 获取映射列名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMappingColumName(this PropertyInfo prop)
        {
            if (prop.IsDefined(typeof(MappingTableAttribute), true))
            {
                MappingTableAttribute attribute = (MappingTableAttribute)prop.GetCustomAttribute(typeof(MappingTableAttribute), true);
                return attribute.GetMapingName();
            }
            else
            {
                return prop.Name;
            }
        }

    }
}
