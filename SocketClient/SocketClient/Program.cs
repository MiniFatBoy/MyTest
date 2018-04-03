using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        private static byte[] result = new byte[1024];
        static void Main(string[] args)
        {
            string address = "10.176.113.76";
            using (StreamReader reader =new StreamReader(@"E:\本机ip.txt") )
            {
                address = reader.ReadLine();
            }
            if (string.IsNullOrEmpty(address))
            {
                return;
                

            }
            IPAddress ip = IPAddress.Parse(address);

            Socket sokertClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sokertClient.Connect(new IPEndPoint(ip, 8080));
                if (sokertClient.Connected)
                {
                    Console.WriteLine("连接成功！");
                }
                else
                {
                    Console.WriteLine("未连接！");
                }
            }
            catch (Exception ex )
            {

                Console.WriteLine(ex);
                return;
            }

            int receiveLenth = sokertClient.Receive(result);
            Console.WriteLine("star");
            while (true)
            {
                try
                {
                    string sendMessage = Console.ReadLine();
                    sokertClient.Send(Encoding.UTF8.GetBytes(sendMessage));//传送信息
                }
                catch (Exception)
                {

                    sokertClient.Shutdown(SocketShutdown.Both);
                    Console.Read();
                    break;
                }
            }
            Console.WriteLine("over");
            Console.ReadLine();
        }
    }
}
