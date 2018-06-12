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
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<SSOUserDataModel>((User.Identity as System.Web.Security.FormsIdentity).Ticket.UserData);

                if (data.MyApps.Keys.Contains(AppCode))
                {
                    //data.MyApps[AppCode].Licence
                    //else
                    //{
                    //    return null;
                    //}
                    return data;
                }
            }
            catch { }

            return data;
        }
    }
}