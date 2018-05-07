using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace HubServer.Models
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> Listeners = new ConcurrentDictionary<string, string>();


        public override Task OnConnected()
        {
            var clientId = Context.ConnectionId;
            var clientName = Context.Request.QueryString["ClientName"];
            Listeners.AddOrUpdate(clientId, clientName, (k, v) => clientName);
            var message = string.Format("{0} 用户:{1}加入聊天室", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), clientName);
            Clients.All.UserJoined(message);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var clientId = Context.ConnectionId;
            var clientName = Context.Request.QueryString["ClientName"];
            var message = string.Format("{0} 用户:{1}离开聊天室", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), clientName);
            Clients.All.UserJoined(message);
            Listeners.TryRemove(clientId, out clientName);
            return base.OnDisconnected(stopCalled);
        }

        [HubMethodName("SendAll")]
        public void SendAll(string message)
        {
            var clientId = Context.ConnectionId;
            var clientName = Listeners[clientId];
            var msg = string.Format("{0}-{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientName, message);
            Clients.All.ReceiveMessage(msg);
        }

        [HubMethodName("SendOne")]
        public void SendOne(string toUserId,string message)
        {
            var senderName = Listeners[Context.ConnectionId];
            var recevierName = Listeners[toUserId];
            var sendMsg = string.Format("{0}-{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), recevierName, message);
            var receiveMsg = string.Format("{0}-{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), senderName, message);
            Clients.Caller.ReceiveMessage(sendMsg);
            Clients.Client(toUserId).ReceiveMessage(receiveMsg);
        }
    }
}