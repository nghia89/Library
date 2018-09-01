using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string AppCode = Tool.GetConfiguration("AppCode");
        
        public BaseController()
        {
        }

        protected virtual SSOUserDataModel GetUserData()
        {
            string subdomain = GetSubDomain(Request.Url);

            AccessInfoLogic _AccessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("StoreConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));
            var accessInfo = _AccessInfoLogic.GetBySubDomain(subdomain);
            
            if (accessInfo == null)
            {
                // return null;
            }

            if (!CheckAccessEndDate(accessInfo))
            {
                // return null;
            }

            SSOUserDataModel loadData = null;
            ViewBag.SSOFullName = "";
            try
            {           
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

                    if (loadData.MyApps.Keys.Contains(AppCode))
                    {
                        // Kiểm tra DB name để vào các đơn vị con
                        //if (accessInfo.DataBaseName.StartsWith(accessInfo.DataBaseName) && loadData.MyApps[AppCode].DatabaseName.Length > 0)
                        //{

                        //}
                        //else
                        //{

                        //}
                        
                        // check Licence

                        //var userAccess = new UserAccessInfo()
                        //{
                        //    Id = loadData.Id,
                        //    UserName = loadData.UserName,
                        //    FullName = loadData.FullName,
                        //    WorkPlaceId = loadData.WorkPlaceId,
                        //    Role = loadData.Role,
                        //    DatabaseName = accessInfo.DataBaseName,
                        //};
                        // return userAccess

                        ViewBag.SSOFullName = loadData.FullName;
                        ViewBag.SSOSchoolName = loadData.WorkPlaceName;
                        ViewBag.Avatar = loadData.Avatar;
                        return loadData;
                    }
                }
            }
            catch { }

#if DEBUG
            if (loadData == null)
            {
                SSOUserDataModel data = null;
                data = new SSOUserDataModel();
                data.Id = "123";
                data.UserName = "Admindemo";
                data.FullName = "Admin demo";
                data.WorkPlaceName = "Trường BiTech - Q. Gò Vấp";
                data.Avatar = "";

                data.MyApps.Add(AppCode, new SSOUserAppModel()
                {
                    AppName = "Quản lý thư viện",
                    ConnectionString = Tool.GetConfiguration("ConnectionString"),
                    DatabaseName = "BiTechLibraryDB"
                });
                ViewBag.SSOFullName = data.FullName;
                ViewBag.SSOSchoolName = data.WorkPlaceName;
                ViewBag.Avatar = data.Avatar;
                return data;
            }
#endif
            return loadData;
        }

        private bool CheckAccessEndDate(AccessInfo info)
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
        
        private static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;

                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }
            return null;
        }
    }
}