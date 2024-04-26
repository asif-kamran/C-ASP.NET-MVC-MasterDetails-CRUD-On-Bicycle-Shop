using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bicycle_project.Startup))]
namespace Bicycle_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
