﻿using BiTech.Library.Areas.Models;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(Role.CustomerUser, Role.CustomerAdmin)]
    public class ThongKe2Controller : BaseController
    {
        private AccessInfoLogic _AccessInfoLogic = new AccessInfoLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("BLibDatabaseName"));

        // GET: ThongKe2
        public ActionResult Index()
        {
            ViewBag.wpid = _UserAccessInfo.WorkPlaceId;

            return View();
        }

        public async Task<JsonResult> GetSubDomainList(string wpid)
        {
            var rs = await new StoreCom().GetChildWorkPlace(wpid, Tool.GetConfiguration("StoreSite"), Tool.GetConfiguration("AppCode"));
            if (rs != null)
            {
                SubDomainPacket pk = new SubDomainPacket();

                foreach (var i in rs)
                {
                    switch (i.Type)
                    {
                        case ECustomerWorkplaceType.PhongGDDT:
                            pk.ListPhng.Add(i);
                            break;
                        case ECustomerWorkplaceType.TruongCap3:
                            pk.ListCap3.Add(i);
                            break;
                        case ECustomerWorkplaceType.TTGDTX:
                            pk.ListTTGD.Add(i);
                            break;
                        case ECustomerWorkplaceType.TruongCap2:
                            pk.ListCap2.Add(i);
                            break;
                        case ECustomerWorkplaceType.TruongCap1:
                            pk.ListCap1.Add(i);
                            break;
                        case ECustomerWorkplaceType.MamnonMauGiao:
                            pk.ListMNMG.Add(i);
                            break;
                    }
                }

                return Json(new
                {
                    data = pk,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = "",
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatForDomain(string site)
        {
            var accessInfo = _AccessInfoLogic.GetBySubDomain(site);

            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), accessInfo.DataBaseName);

            return Json(new
            {
                data = "okok",
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}