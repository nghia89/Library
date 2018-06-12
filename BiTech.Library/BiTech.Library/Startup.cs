using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BiTech.Library.Startup))]
namespace BiTech.Library
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
