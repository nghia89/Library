﻿using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            ViewBag._SubDomain = _SubDomain = Tool.GetSubDomain(Request.Url);
            _UserAccessInfo = GetAccessInfo();

            if (_UserAccessInfo == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                            { "Controller", "Error" },
                            { "Action", "NotFound" }
                        });
            }

            base.OnActionExecuting(filterContext);
        }

        protected UserAccessInfo GetAccessInfo()
        {

            var accessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));
            var accessInfo = accessInfoLogic.GetBySubDomain(_SubDomain);

            if (accessInfo == null || !Tool.CheckAccessEndDate(accessInfo))
                return null;

            ViewBag.WebHeader = accessInfo.WebHeader;
            ViewBag.IsDebuging = false;

            var userAccess = new UserAccessInfo();
            userAccess.DatabaseName = accessInfo.DataBaseName;

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
                        if (loadData.MyApps.Keys.Contains(_AppCode))
                        {
                            // todo if(CheckLicence( loadData.MyApps[_AppCode].Licence) == true) {

                            userAccess.Id = loadData.Id;
                            userAccess.UserName = loadData.UserName;
                            userAccess.FullName = loadData.FullName;
                            userAccess.Avatar = loadData.Avatar;
                            userAccess.WorkPlaceId = loadData.WorkPlaceId;
                            userAccess.Role = loadData.Role;
                        }
                    }
                }
                catch { }
            }

#if DEBUG
            if (userAccess.Id.Length == 0)
            {
                userAccess.Id = "debug_id";
                userAccess.UserName = "DebugUser";
                userAccess.FullName = "Debug User";
                userAccess.WorkPlaceId = "";
                userAccess.Role = "";
                userAccess.Avatar = @"\Content\Images\userUnauth.jpg";

                ViewBag.SSOFullName = userAccess.FullName;
                ViewBag.Avatar = userAccess.Avatar;

                ViewBag.IsDebuging = true;
            }
#endif
            return userAccess;
        }
    }
}