using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BiTech.Library.Controllers.BaseClass
{
    public abstract class BaseController : Controller
    {
        protected string _AppCode = Tool.GetConfiguration("AppCode");
        protected UserAccessInfo _UserAccessInfo;
        protected string _SubDomain;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag._SubDomain = _SubDomain = GetSubDomain(Request.Url);
            _UserAccessInfo = LoadAccessInfo();

            if (_UserAccessInfo == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                            { "Controller", "Error" },
                            { "Action", "NotFound" }
                        });
            }
            else
            {
                ViewBag.WebHeader = _UserAccessInfo.WebHeader;
                ViewBag._SubDomainAccessPermission = _UserAccessInfo.SubDomainAccessPermission;
                ViewBag.SSOFullName = _UserAccessInfo.FullName;
                ViewBag.Avatar = _UserAccessInfo.Avatar;
            }

            base.OnActionExecuting(filterContext);
        }

        protected UserAccessInfo LoadAccessInfo()
        {
            #region MyRegion

            //var accessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));
            //var accessInfo = accessInfoLogic.GetBySubDomain(_SubDomain);

            //if (accessInfo == null || !CheckAccessEndDate(accessInfo))
            //    return null;


            //var userAccess = new UserAccessInfo();
            //userAccess.DatabaseName = accessInfo.DataBaseName;
            //userAccess.WebHeader = accessInfo.WebHeader;

            //if (Request.IsAuthenticated)
            //{
            //    try
            //    {
            //        SSOUserDataModel loadData = null;
            //        var claimSSO = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "SSOUserDataModel").Select(c => c.Value);
            //        foreach (var c in claimSSO)
            //        {
            //            try
            //            {
            //                loadData = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>(c);
            //                break;
            //            }
            //            catch { }
            //        }

            //        if (loadData != null)
            //        {
            //            if (loadData.MyApps.Keys.Contains(_AppCode))
            //            {
            //                // todo if(CheckLicence( loadData.MyApps[_AppCode].Licence) == true) {

            //                userAccess.Id = loadData.Id;
            //                userAccess.UserName = loadData.UserName;
            //                userAccess.FullName = loadData.FullName;
            //                userAccess.Avatar = loadData.Avatar;
            //                userAccess.WorkPlaceId = loadData.WorkPlaceId;
            //                userAccess.Role = loadData.Role;
            //                userAccess.SubDomainAccessPermission = userAccess.DatabaseName.StartsWith(loadData.MyApps[_AppCode].DatabaseName);

            //                //CheckSubDomainAccessAuth(userAccess.WorkPlaceId, Tool.GetConfiguration("StoreSite"), Tool.GetConfiguration("AppCode"));                            
            //            }
            //        }
            //    }
            //    catch { }
            //}
            #endregion

            UserAccessInfo userAccess = (UserAccessInfo)HttpContext.Items["UserAccessInfo"];

            if (userAccess == null)
                userAccess = GetUserAccessInfo(_SubDomain, _AppCode, Request, User);
            else
                HttpContext.Items.Remove("UserAccessInfo");
            ViewBag.IsDebuging = false;

#if DEBUG
            if (userAccess != null && userAccess.Id.Length == 0)
            {
                userAccess.Id = "debug_id";
                userAccess.UserName = "DebugUser";
                userAccess.FullName = "Debug User SoHCM";
                userAccess.WorkPlaceId = "";
                userAccess.Role = "";
                userAccess.Avatar = @"\Content\Images\userUnauth.jpg";
                userAccess.SubDomainAccessPermission = _SubDomain == "thuviensohcm";
                ViewBag.IsDebuging = true;
            }
#endif
            return userAccess;
        }

        internal static UserAccessInfo GetUserAccessInfo(string subDomain, string appCode, HttpRequestBase Request, System.Security.Principal.IPrincipal User)
        {
            var accessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));
            var accessInfo = accessInfoLogic.GetBySubDomain(subDomain);

            if (accessInfo == null || !CheckAccessEndDate(accessInfo))
                return null;

            var userAccess = new UserAccessInfo();
            userAccess.DatabaseName = accessInfo.DataBaseName;
            userAccess.WebHeader = accessInfo.WebHeader;

            if (Request.IsAuthenticated)
            {
                try
                {
                    SSOUserDataModel loadData = null;
                    var claimSSO = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "SSOUserDataModel").Select(c => c.Value);
                    foreach (var c in claimSSO)
                    {
                        try
                        {
                            loadData = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>(c);
                            break;
                        }
                        catch { }
                    }

                    if (loadData != null)
                    {
                        if (loadData.MyApps.Keys.Contains(appCode))
                        {
                            // todo if(CheckLicence( loadData.MyApps[_AppCode].Licence) == true) {

                            userAccess.Id = loadData.Id;
                            userAccess.UserName = loadData.UserName;
                            userAccess.FullName = loadData.FullName;
                            userAccess.Avatar = loadData.Avatar;
                            userAccess.WorkPlaceId = loadData.WorkPlaceId;
                            userAccess.Role = loadData.Role;
                            userAccess.SubDomainAccessPermission = PermissionToAccessSubDomain(userAccess.DatabaseName, loadData.MyApps[appCode].DatabaseName);

                            //CheckSubDomainAccessAuth(userAccess.WorkPlaceId, Tool.GetConfiguration("StoreSite"), Tool.GetConfiguration("AppCode"));                            
                        }
                    }
                }
                catch { }
            }
            return userAccess;
        }

        internal static string GetSubDomain(Uri url, bool withoutBlib = true)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;

                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);

                    if (withoutBlib)
                    {
                        if (host.Length > 5)
                            return host.Substring(0, index - 5); //".blib".Length = 5
                        else
                            return "";
                    }
                    return host.Substring(0, index);
                }
            }
            return null;
        }

        internal static bool CheckAccessEndDate(AccessInfo info)
        {
            if (info == null)
                return false;

            if (info.IsActivePeriod)
            {
                if (info.EndDate == null)
                    return false;
                return info.EndDate > DateTime.Now;
            }

            return true;
        }

        internal static bool PermissionToAccessSubDomain(string dbName1, string dbName2)
        {
            return dbName1.StartsWith(dbName2);
        }

        internal bool CheckSubDomainAccessAuth(string wpId, string site, string appCode)
        {
            var list = new StoreCom().GetChildWorkPlace(wpId, site, appCode);
            foreach (var i in list)
            {
                if (i.Site == _SubDomain)
                {
                    return true;
                }
            }
            return false;
        }
    }
}