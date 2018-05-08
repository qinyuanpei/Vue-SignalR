using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入ClientName:");
            var clientName = Console.ReadLine();
            var qs = string.Format("clientName={0}", clientName);
            var connection = new HubConnection("http://localhost:58515/signalr", qs, false);
            connection.Received += data =>
            {
                //Console.WriteLine(data);
            };

            var proxy = connection.CreateHubProxy("Chat");
            connection.Start().Wait();

            //注册接收消息回调
            proxy.On("ReceiveMessage", data =>
            {
                Console.WriteLine(data);
            });

            //注册用户加入回调
            proxy.On("UserJoined", data =>
            {
                Console.WriteLine(data);
            });

            //注册用户离开回调
            proxy.On("UserLeaved", data =>
            {
                Console.WriteLine(data);
            });

            while (true)
            {
                var input = Console.ReadLine();
                if(!string.IsNullOrEmpty(input))
                    proxy.Invoke("SendAll", input);
            }
        }
    }
}
