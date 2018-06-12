using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace BiTech.Library
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (Context.Request.IsAuthenticated)
            {
                FormsIdentity ident = (FormsIdentity)Context.User.Identity;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>(ident.Ticket.UserData);

                string[] arrRoles = data.Role.Split(new[] { '|' });
                Context.User = new System.Security.Principal.GenericPrincipal(ident, arrRoles);
            }
        }
    }
}
