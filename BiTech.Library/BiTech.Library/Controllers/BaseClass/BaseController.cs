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
            
            try
            {
                SSOUserDataModel loadData = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>((User.Identity as System.Web.Security.FormsIdentity).Ticket.UserData);

                if (loadData.MyApps.Keys.Contains(AppCode))
                {
                    //data.MyApps[AppCode].Licence
                    //else
                    //{
                    //    return null;
                    //}
                    data = loadData;
                }
            }
            catch { }

            #region Demo

            data = new SSOUserDataModel();
            data.Id = "123";
            data.MyApps.Add(AppCode, new SSOUserAppModel()
            {
                AppName = "Quản lý thư viện",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "BiTechLibraryDB"
            });

            #endregion

            return data;
        }
    }
}