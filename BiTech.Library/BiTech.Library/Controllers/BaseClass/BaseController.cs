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

        protected virtual SSOUserDataModel GetUserData()
        {
            SSOUserDataModel data = null;
            SSOUserDataModel loadData = null;
            ViewBag.SSOFullName = "";
            try
            {


                //SSOUserDataModel loadData = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>((User.Identity as System.Web.Security.FormsIdentity).Ticket.UserData);

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
                        // check Licence

                        //if(loadData.MyApps[AppCode].Licence == valid){
                        //{
                        //    data = loadData;
                        //    ViewBag.SSOFullName = data.FullName;
                        //}
                        //else
                        //{
                        //    return null;
                        //}

                        ViewBag.SSOFullName = loadData.FullName;
                        return loadData;
                    }
                }
            }
            catch { }

#if DEBUG
            if (data == null && loadData == null)
            {
                data = new SSOUserDataModel();
                data.Id = "123";
                data.UserName = "Admindemo";
                data.FullName = "Admin demo";

                data.MyApps.Add(AppCode, new SSOUserAppModel()
                {
                    AppName = "Quản lý thư viện",
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "BiTechLibraryDB"
                });
                ViewBag.SSOFullName = data.FullName;
            }
#endif
            return data;
        }
    }
}