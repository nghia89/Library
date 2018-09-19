using BiTech.Library.Areas.Models;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BiTech.Library.Areas.Controllers
{
    public class AppCenterController : ApiController
    {
        public AccessInfoLogic _AccessInfoLogic;

        public AppCenterController()
        {
            _AccessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("StoreConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));
        }

        [HttpGet]
        public dynamic Test()
        {
            return "OK";
        }

        // POST: AppCenter/UpdateAccessInfo
        [HttpPost]
        public IHttpActionResult UpdateAccessInfo([FromBody] CustomerAccessInfo info)
        {
            if (CheckConfirmKey(info.ConfirmKey))
            {
                if (CheckInfoData(info))
                {
                    var rs = _AccessInfoLogic.Update(new DTO.AccessInfo()
                    {
                        IdWorkplace = info.IdWorkplace,
                        DataBaseName = info.DataBaseName,
                        EndDate = info.EndDate,
                        IsActivePeriod = info.IsActivePeriod,
                        WebHeader = info.WebHeader,
                        WebSubDomain = info.WebSubDomain.Trim().ToLower()
                    });

                    HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, rs ? "updated" : "update fail");
                    return ResponseMessage(response2);
                }
            }
            
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, "invalid info");
            return ResponseMessage(response);
        }

        private bool CheckConfirmKey(string key)
        {
            // todo
            return key == "132";
        }

        private bool CheckInfoData(CustomerAccessInfo info)
        {
            return info.IdWorkplace.Length == 24 && info.WebSubDomain.Length > 0 && info.DataBaseName.Length > 0 && info.IsActivePeriod ? info.EndDate > DateTime.Now : true;
        }
    }
}
