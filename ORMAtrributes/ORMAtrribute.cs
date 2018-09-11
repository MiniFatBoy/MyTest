using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMAtrributes
{
    //ORM框架中的特性

    /// <summary>
    /// 表名映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappingTableAttribute : Attribute
    {
        public string _name = null;
        public MappingTableAttribute(string name)
        {
            _name = name;
        }

        public string GetMapingName()
        {
            return _name;
        }
    }

    /// <summary>
    /// 列名映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingColumAttribute : Attribute
    {
        public string _name = null;
        public MappingColumAttribute(string name)
        {
            _name = name;
        }

        public string GetMapingName()
        {
            return _name;
        }
    }

    /// <summary>
    /// 主键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentifyAttribute : Attribute
    {
        public bool IsIdentify { get; set; }
    }

    /// <summary>
    /// 外键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimairyKeyAttribute : Attribute
    {
        public bool IsPrimaryKey { get; set; }
    }

    /// <summary>
    /// 是否忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
        public bool IsIgnore { get; set; }
    }
}
