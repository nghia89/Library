using System.Security.Claims;
using System.Linq;

namespace System.Web.Mvc
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
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