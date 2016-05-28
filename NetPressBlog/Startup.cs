using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetPressBlog.Startup))]
namespace NetPressBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
