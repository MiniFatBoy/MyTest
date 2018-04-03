using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using LogHelper;

namespace SocketServer
{
    class Program
    {
        private static byte[] result = new byte[1024];

        private static int myPoint =8080;

        static Socket sokertServer;
        static void Main(string[] args)
        {
            try
            {
                IPAddress ip = IPAddress.Parse("10.176.114.99");

                using (var stream = new StreamWriter(@"E:\本机ip.txt"))
                {
                    stream.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
                }

                sokertServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                sokertServer.Bind(new IPEndPoint(ip, myPoint));

                sokertServer.Listen(10);

                Console.WriteLine("创建{0}成功！", sokertServer.LocalEndPoint.ToString());

                Thread myThread = new Thread(ListenClientConnect);
                myThread.Start();
                Console.Read();
            }
            catch (Exception ex ) 
            {
               string ss = LogHelper.LogerHelper.GetMethodInfo();
                LogerHelper.WriteLog(ex, "", ss);
            }
            
           
        }

        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocker = sokertServer.Accept();
                clientSocker.Send(Encoding.UTF8.GetBytes("server say hello!"));
                Thread receivethread = new Thread(ReceiveMessage);
                receivethread.Start(clientSocker);
            }
        }

        private static void ReceiveMessage(object clientSocker)
        {
            Socket myClientSocker = (Socket)clientSocker;

            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocker.Receive(result);
                    Console.WriteLine("client say : {0}", Encoding.UTF8.GetString(result, 0, receiveNumber));

                }
                catch (Exception)
                {

                    Console.WriteLine(" A client break");
                    myClientSocker.Shutdown(SocketShutdown.Both);
                    break;
                }
            }
        }
    }
}
