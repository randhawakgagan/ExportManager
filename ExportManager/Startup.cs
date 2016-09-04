using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExportManager.Startup))]
namespace ExportManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
