
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using TestModel;

namespace DevelopHelpers
{
    /// <summary>
    /// 监管进程
    /// </summary>
    public class ProceWatcher:IDisposable
    {
        /* 需要引用 System.Management*/
        #region 字段
        private string[] _superviseeNames;
        #endregion

        #region 属性
        #endregion

        /// <summary>
        /// 进程监管
        /// </summary>
        /// <param name="superviseeNames">被监管的进程名(例"PipeDemo.exe")</param>
        public ProceWatcher(string[] superviseeNames)
        {
            _superviseeNames = superviseeNames;
        }


        #region 方法

        /// <summary>
        /// 开始监控
        /// </summary>
        public void Start()
        {
            if (_superviseeNames != null)
            {
                OutputProcessList(_superviseeNames);
                foreach (var superviseeName in _superviseeNames)
                {
                    Process[] processList = Process.GetProcessesByName(superviseeName.Remove(superviseeName.Length-4));

                    if (processList.Length > 0)
                    {
                        foreach (var process in processList)
                        {
                            process.Exited += ProcessExited;
                            process.EnableRaisingEvents = true;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 进程结束时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessExited(object sender, EventArgs e)
        {
            CheckProcessIsLive();
        }

        /// <summary>
        /// 输出进程列表
        /// </summary>
        /// <param name="processname"></param>
        void OutputProcessList(string[] processname)
        {
            StringBuilder s = new StringBuilder();
            List<ProcessInfo> processinfoList = new List<ProcessInfo>();
            foreach (var item in processname)
            {
              processinfoList.AddRange(GetProcessInfoByWMI(item));
            }

            XmlSerializationHelper.SerializeToXml(processinfoList, "process.xml");
        }

        /// <summary>
        /// 检查进程的活动状态（如果进程死掉则重启）
        /// </summary>
        void CheckProcessIsLive()
        {
            ///读取本地进程列表
            List<ProcessInfo> localprocesslist = new List<ProcessInfo>();
            localprocesslist = XmlSerializationHelper.DeSerializeFromXml<List<ProcessInfo>>("process.xml",true);

            ///校验进程
            List <Process> processlist = new List<Process>();
            foreach (var item in _superviseeNames)
            {
                processlist.AddRange(Process.GetProcessesByName(item.Remove(item.Length - 4)).ToList());
            }
                
            foreach (var item in localprocesslist)
            {
                Process process = processlist.FirstOrDefault(p => p.Id == item.ProceID);
                if (process == null)
                {
                    Process newprocess =  Process.Start(item.ExecPath, item.Arguments);
                    newprocess.Exited += ProcessExited;
                    newprocess.EnableRaisingEvents = true;
                }
            }
            ///重启后更新进程列表数据
            OutputProcessList(_superviseeNames);

        }

        /// <summary>
        ///通过WMI获取进程信息
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns></returns>
        private static List<ProcessInfo> GetProcessInfoByWMI(string processName)
        {
            List<ProcessInfo> results = new List<ProcessInfo>();

            string wmiQuery = string.Format("select *  from Win32_Process where Name='{0}'", processName);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery))
            {
                using (ManagementObjectCollection retObjectCollection = searcher.Get())

                {
                    foreach (ManagementObject retObject in retObjectCollection)

                    {
                        ProcessInfo info = new ProcessInfo();
                        info.ProceName = retObject.GetPropertyValue("Name").ToString();
                        info.ProceID = Convert.ToInt32(retObject.GetPropertyValue("ProcessId").ToString());
                        info.CommandLine = retObject.GetPropertyValue("CommandLine").ToString();
                        info.ExecPath = retObject.GetPropertyValue("ExecutablePath").ToString();
                        results.Add(info);
                    }
                }
            }
            return results;

        }

        public void Dispose()
        {
           
        }

        #endregion
    }
}
