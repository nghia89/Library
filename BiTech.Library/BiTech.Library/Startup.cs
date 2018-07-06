using Microsoft.Owin;
using Mongo.Migration.Services.Initializers;
using Owin;

[assembly: OwinStartupAttribute(typeof(BiTech.Library.Startup))]
namespace BiTech.Library
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            MongoMigration.Initialize();
            //ConfigureAuth(app);
        }
    }
}
