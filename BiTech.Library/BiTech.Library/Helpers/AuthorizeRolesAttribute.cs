using System.Security.Claims;
using System.Linq;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Models;

namespace System.Web.Mvc
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        private bool _RequiredDomainAccess;

        public AuthorizeRolesAttribute(bool requiredDomainAccess, params string[] roles) : base()
        {
            _RequiredDomainAccess = requiredDomainAccess;
            Roles = string.Join(",", roles);
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (_RequiredDomainAccess)
            {
                var subDomain = BaseController.GetSubDomain(httpContext.Request.Url);
                var appCode = Tool.GetConfiguration("AppCode");
                var userAccessInfo = BaseController.GetUserAccessInfo(subDomain, appCode, httpContext.Request, httpContext.User);

                if (userAccessInfo.SubDomainAccessPermission)
                    httpContext.Items["UserAccessInfo"] = userAccessInfo;

                return userAccessInfo.SubDomainAccessPermission;
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var roles = ((ClaimsIdentity)filterContext.HttpContext.User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }

    }
}