using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using TestModel;

namespace FuturesTrade.Common.Utils
{
    /// <summary>
    /// 比较两个列表的差异，并将差异作为列表返回
    /// </summary>
    public class ObjectComparer
    {
        /// <summary>
        /// 比较两个列表，返回差异项列表
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="oldList">初始对象列表</param>
        /// <param name="newList">新的对象列表</param>
        /// <param name="mainKeyName">主键名</param>
        /// <param name="mainPropertyName">必填的属性名</param>
        /// <param name="skipPropertyNames">忽略比较的属性名</param>
        /// <returns></returns>
        public static ObservableCollection<T> CompareList<T>(ObservableCollection<T> oldList,
           ObservableCollection<T> newList, string mainKeyName, string mainPropertyName, string[] skipPropertyNames, bool isSkipAddOrRemove = false)
        {
            if (oldList == null || newList == null)
            {
                return newList;
            }

            ObservableCollection<T> result = new ObservableCollection<T>();

            //获取T中的所有属性
            Type t = typeof(T);
            PropertyInfo[] propertyInfos = t.GetProperties();
            if (propertyInfos == null)
            {
                return newList;
            }

            //主键
            PropertyInfo mainKeyProperty = propertyInfos.FirstOrDefault(p => p.Name == mainKeyName);

            //主要的属性
            PropertyInfo mainProperty = propertyInfos.FirstOrDefault(p => p.Name == mainPropertyName);

            if (mainKeyProperty != null && mainProperty != null)
            {
                foreach (var newItem in newList)
                {
                    if (mainProperty.GetValue(newItem, null) != null)
                    {
                        var item = GetObjectByProperties<T>(oldList, newItem, new PropertyInfo[] { mainKeyProperty });

                        //将修改过的和新增的元素添加到列表
                        if (!CompareProperties(item, newItem, skipPropertyNames, isSkipAddOrRemove, 1))
                        {
                            result.Add(newItem);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 比较两个对象的值是否相同
        /// </summary>
        /// <param name="oldObject">旧对象</param>
        /// <param name="newObject">新对象</param>
        /// <param name="ignoredProperties">忽略比较的属性</param>
        /// <param name="loop">循环次数</param>
        /// <returns></returns>
        public static bool CompareProperties(object oldObject, object newObject, string[] ignoredProperties, bool isSkipAddOrRemove = false,
            int loop = 1)
        {
            //如果新对象和旧对象，至少有一个为空
            if (oldObject == null || newObject == null)
            {
                if (isSkipAddOrRemove)
                {
                    return true;
                }
                return oldObject == newObject;
            }

            //如果oldObject是值类型或者字符串类型
            if (oldObject.GetType().IsValueType || oldObject.GetType() == typeof(string))
            {
                return IsPropertyEqual(oldObject, newObject);
            }
            var properties = oldObject.GetType().GetProperties();
            if (ignoredProperties != null && ignoredProperties.Length > 0)
            {
                properties = properties.Where(p => !ignoredProperties.Contains(p.Name)).ToArray();
            }
            foreach (var property in properties)
            {
                var oldValue = property.GetValue(oldObject, null);
                var newValue = property.GetValue(newObject, null);

                //判断该属性是否为值类型
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    if (!IsPropertyEqual(oldValue, newValue))
                    {
                        return false;
                    }
                }
                //属性如果是引用类型且loop > 0，则进行递归调用
                else if (loop > 0 && !CompareProperties(oldValue, newValue, null, isSkipAddOrRemove, loop - 1))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 合并两个列表
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="newList">用户提交的明细表</param>
        /// <param name="differenceList">差异明细表</param>
        /// <param name="mainKey">T类的主键</param>
        /// <param name="mainPropertyNames">T类主要属性</param>
        /// <returns></returns>
        public static ObservableCollection<T> MergeList<T>(ObservableCollection<T> newList,
            ObservableCollection<T> differenceList, string mainKeyName, string[] mainPropertyNames)
            where T : BaseEntity
        {
            Type t = typeof(T);
            PropertyInfo[] propertyInfos = t.GetProperties();
            PropertyInfo mainKeyProperty = propertyInfos.Where(p => p.Name == mainKeyName).FirstOrDefault();
            if (mainKeyProperty == null)
            {
                return null;
            }
            List<PropertyInfo> mainProperties = new List<PropertyInfo>();
            foreach (string properyName in mainPropertyNames)
            {
                var propertyInfo = propertyInfos.Where(p => p.Name == properyName).FirstOrDefault();
                if (propertyInfo != null)
                {
                    mainProperties.Add(propertyInfo);
                }
            }

            //找出新增明细，并更新主要属性值
            foreach (var item in differenceList)
            {
                var newItem = GetObjectByProperties<T>(newList, item, mainProperties);
                if (newItem != null)
                {
                    mainKeyProperty.SetValue(newItem, mainKeyProperty.GetValue(item, null), null);
                }
            }
            return newList;
        }

        /// <summary>
        /// 属性值比较
        /// </summary>
        /// <param name="oldValue">旧属性值</param>
        /// <param name="newValue">新属性值</param>
        /// <returns></returns>
        private static bool IsPropertyEqual(object oldValue, object newValue)
        {
            if (oldValue == null || newValue == null)
            {
                return oldValue == newValue;
            }

            return oldValue.GetType() == newValue.GetType() && oldValue.ToString().Equals(newValue.ToString());
        }

        /// <summary>
        /// 从列表中找到属性相同的项
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">对象列表</param>
        /// <param name="target">目标对象</param>
        /// <param name="properties">属性列表</param>
        /// <returns></returns>
        private static T GetObjectByProperties<T>(IEnumerable<T> list, T target, IEnumerable<PropertyInfo> properties)
        {
            T result = default(T);
            foreach (var item in list)
            {
                bool isEqual = true;
                foreach (var property in properties)
                {
                    var oldValue = property.GetValue(item, null);
                    var newValue = property.GetValue(target, null);
                    if (!IsPropertyEqual(oldValue, newValue))
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }
    }
}
