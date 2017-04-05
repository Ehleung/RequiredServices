using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TryIt.Startup))]
namespace TryIt
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
