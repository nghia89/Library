using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Configuration;

namespace BiTech.Library
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = ".XMyCookieNameCustomer",
                CookieDomain = ".bitechco.test",
                LoginPath = new PathString("/Account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = context =>
                    {
                        context.Response.Redirect(ConfigurationManager.AppSettings["LoginUrl"]?.ToString() ?? "www.bitechco.com");
                    },
                    OnResponseSignIn = context =>
                    {
                        context.Properties.AllowRefresh = true;
                        context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14);
                    }
                }
            });
        }
    }
}