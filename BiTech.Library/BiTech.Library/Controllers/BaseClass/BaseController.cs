using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.SSOFullName = "";
            try
            {
                SSOUserDataModel loadData = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>((User.Identity as System.Web.Security.FormsIdentity).Ticket.UserData);

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
                    }
                }
            }
            catch { }

#if DEBUG
            if (data == null)
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