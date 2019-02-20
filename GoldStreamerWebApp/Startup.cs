using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoldStreamerWebApp.Startup))]
namespace GoldStreamerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //Map Price viewer Hub
            app.MapSignalR();
        }
    }
}
