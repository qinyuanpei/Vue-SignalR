using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(HubServer.Startup))]

namespace HubServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
             {
                //启用跨域
                map.UseCors(CorsOptions.AllowAll);

                //启用SignalR
                var config = new HubConfiguration()
                 {
                     EnableJSONP = true,
                     EnableDetailedErrors = true,
                     EnableJavaScriptProxies = true
                 };
                 map.RunSignalR(config);
             });
        }
    }
}
