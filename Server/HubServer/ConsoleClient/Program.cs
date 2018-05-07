using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnection("http://localhost:58515/signalr", "clientName=Payne", false);
            //connection.Received += data => { Console.WriteLine(data); };
            //connection.StateChanged += state => { Console.WriteLine("{0}:{1}", state.NewState, state.OldState); };
            var proxy = connection.CreateHubProxy("Chat");
            connection.Start().Wait();
            proxy.On("ReceiveMessage", data => Console.WriteLine(data));
            proxy.On("UserJoined", data => Console.WriteLine(data));
            while (true)
            {
                
                var input = Console.ReadLine();
                proxy.Invoke("SendAll", input);
            }
        }
    }
}
