using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRExample.NetFramework.API.App_Start.Startup))]

namespace SignalRExample.NetFramework.API.App_Start
{
    public class Startup
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        public static void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //app.MapSignalR();

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR("/question-hub", hubConfiguration);
        }
    }
}