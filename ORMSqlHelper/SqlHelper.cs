using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ORMAtrributes;
using System.Reflection;
using System.Data.SqlClient;

namespace ORMSqlHelper
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class SqlHelper
    {
        private readonly static string _connectionString = ConfigurationManager.AppSettings["SqlServer"];

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int Insert<T>(T t)
        {
            int result = 0;
            Type type = typeof(T);
            string columString = "";
            string valueString = "";
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (var item in type.GetProperties())
            {
                ///除去主键、忽略元素
                if (item.IsDefined(typeof(IdentifyAttribute)) || item.IsDefined(typeof(IgnoreAttribute)))
                {
                    continue;
                }
                columString = columString + item.GetMappingColumName() + ",";
                valueString = valueString + "@" + item.GetMappingColumName() + ",";
                SqlParameter para = new SqlParameter($"@{item.GetMappingColumName()}", item.GetValue(t));
            }
            string sqlCommandString = $"INSERT INTO {type.GetMappingTableName()} ({columString.TrimEnd(',')}) VALUE ({valueString.TrimEnd(',')})";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandString, con);
                command.Parameters.AddRange(paras.ToArray());
                result = command.ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Find<T>(int id)
        {
            Type type = typeof(T);
            string columString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetMappingColumName()}] AS [{p.Name}]"));
            string sqlCommandString = $"SELECT {columString} FROM {type.GetMappingTableName()} WHERE ID = {id} ";
            object t = Activator.CreateInstance(type);//通过类型创建实例
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandString, con);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    foreach (var prop in type.GetProperties())
                    {
                        prop.SetValue(t, reader[prop.Name.ToString()]);
                    }
                }
            }
            return (T)t;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int Update<T>(T t)
        {
            int result = -1;
            Type type = typeof(T);
            string idetifyName = "";
            string sqlCommandString = $"UPDATE [{type.GetMappingTableName()}] SET ";
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (var item in type.GetProperties())
            {
                if (item.IsDefined(typeof(IdentifyAttribute)))
                {
                    continue;
                }
                SqlParameter para = new SqlParameter($"@{item.GetMappingColumName()}", item.GetValue(t));
                if (item.IsDefined(typeof(IdentifyAttribute)))
                {
                    idetifyName = item.GetMappingColumName();
                    continue;
                }
                sqlCommandString = sqlCommandString + $"{item.GetMappingColumName()} = @{item.GetMappingColumName()},";
                paras.Add(para);
            }
            sqlCommandString = sqlCommandString.TrimEnd(',') + $" WHERE {idetifyName} = @{idetifyName} ";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandString, con);
                command.Parameters.AddRange(paras.ToArray());
                result = command.ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int Delete<T>(T t)
        {
            int result = -1;
            Type type = typeof(T);
            string idetifyName = "";
            string sqlCommandString = $"DELETE FROM [{type.GetMappingTableName()}] ";
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (var item in type.GetProperties())
            {
                if (item.IsDefined(typeof(IdentifyAttribute)))
                {
                    SqlParameter para = new SqlParameter($"@{item.GetMappingColumName()}", item.GetValue(t));
                    idetifyName = item.GetMappingColumName();
                    paras.Add(para);
                }
            }
            sqlCommandString = sqlCommandString  + $" WHERE {idetifyName} = @{idetifyName} ";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommandString, con);
                command.Parameters.AddRange(paras.ToArray());
                result = command.ExecuteNonQuery();
            }
            return result;
        }
    }
}
