using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DevelopHelpers
{
    public class ConvertHelpers
    {
        /// <summary>
        /// 根据父类创建子类
        /// </summary>
        /// <typeparam name="T">子类</typeparam>
        /// <typeparam name="P">父类</typeparam>
        /// <param name="parent">父类实体</param>
        /// <returns>子类实体</returns>
        public static T CreateInstanceByBase<T,P>(P parent ) where T:P
        {
            Type baseType = typeof(P);
            Type type = typeof(T);
            T result =(T)Activator.CreateInstance(typeof(T), true);
            PropertyInfo[] basePropertyInfos = baseType.GetProperties();
            PropertyInfo[] PropertyInfos = type.GetProperties();
            foreach (var baseItem in basePropertyInfos)
            {
                var property = PropertyInfos.FirstOrDefault(f => f.Name == baseItem.Name);
                if (property != null)
                {
                    property.SetValue(result, baseItem.GetValue(parent, null), null);
                }
            }
            return result;         
        }
    }
}
