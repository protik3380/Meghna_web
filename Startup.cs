using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFreshStore.Startup))]
namespace EFreshStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
        }


    }
}
