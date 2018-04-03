using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueTest
{

    public class QueueInfo
    {
        public string name { get; set; }

        public string password { get; set; }
    }

    public delegate void ScanQueuedlgt();
   public class BusinessInfoHelper
    {
        public ScanQueuedlgt ScanQueue;

        public readonly static BusinessInfoHelper Instance = new BusinessInfoHelper();

        private BusinessInfoHelper() { }

        private Queue<QueueInfo> listQueue = new Queue<QueueInfo>();

        /// <summary>
        /// 将对象加入队列中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        public void AddQueue(string name, string password)
        {
            QueueInfo info = new QueueInfo();
            info.name = name;
            info.password = password;
            listQueue.Enqueue(info);
        }

        /// <summary>
        /// 启动队列
        /// </summary>
        /// <param name="ScanQueue"></param>
        public void start(ScanQueuedlgt ScanQueue)
        {
            Thread thread = new Thread(new ThreadStart(ScanQueue));
            thread.IsBackground = true;
            thread.Start();
        }

        private void threadStart()
        {
            while (true)
            {
                if (listQueue.Count!=0)
                {
                    try
                    {
                        ScanQueue();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// 要执行的方法
        /// </summary>
        private void ScanQueue1()
        {
            while (listQueue.Count>0)
            {
                try
                {
                    //从队列中取出（队列只能先取头）
                    QueueInfo info = listQueue.Dequeue();

                    ///取出来后要加入的代码
                    Console.WriteLine("账户名："+info.name+" 密码："+info.password);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
