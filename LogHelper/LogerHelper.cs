using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LogHelper
{
    public static class LogerHelper
    {

        /// <summary>
        /// 将异常打印到LOG文件
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="LogAddress">日志文件地址</param>
        /// <param name="Ex_MetchodName">引发异常的方法名称</param>
        public static void WriteLog(Exception ex, string LogAddress,string Ex_MetchodName)
        {
            //如果日志文件路径LogAddress为空，则默认在Debug目录下新建 YYYY-mm-dd-Log.log文件
            if (LogAddress == "")
            {
                LogAddress = Environment.CurrentDirectory + '\\' +
                   DateTime.Now.Year + '-' +
                   DateTime.Now.Month + '-' +
                   DateTime.Now.Day + "-Log.log";
            }
            //把异常信息输出到文件
            StreamWriter fs = new StreamWriter(LogAddress, true);
            fs.WriteLine("当前时间：" + DateTime.Now.ToString());
            fs.WriteLine("异常信息：" + ex.Message);
            fs.WriteLine("异常对象：" + ex.Source);
            fs.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
            fs.WriteLine("触发方法：" + ex.TargetSite);
            fs.WriteLine();
            fs.Close();
        }


        /// <summary>
        /// 获取当前正在执行的代码所在的函数(方法)的名字和参数等信息
        /// </summary>
        /// <returns></returns>
        public static string GetMethodInfo()
        {
            var method = new StackFrame(1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            var property = (
            from p in method.DeclaringType.GetProperties(
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic)
            where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
            select p).FirstOrDefault();
            var classname = method.DeclaringType.Name;//所在的类；
            var namespeacename = method.DeclaringType.Namespace;//所在的程序集；
            string funname= property == null ? method.Name : property.Name;
            return "命名空间：" + namespeacename + "；  所在的类：" + classname + "；   方法名称：" + funname;
        }

    }
}
