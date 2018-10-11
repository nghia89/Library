using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Helpers;
using System.Threading.Tasks;
using static BiTech.Library.Helpers.Tool;
using System.IO;
using Aspose.Cells;
using System.Collections;
using BiTech.Library.Common;

namespace BiTech.Library.Controllers
{
    public class NghiepVuController : BaseController
    {
        // GET: NghiepVu
        public ActionResult Index(KeySearchViewModel KeySearch, int? page)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _DDCLogic.getDDCByKeySearch(KeySearch);
            var list_getall = _DDCLogic.GetAllDDC();
            List<DDC> lst = new List<DDC>();
            foreach (var item in list)
            {
                DDC ks = new DDC()
                {
                    Id = item.Id,
                    MaDDC = item.MaDDC,
                    Ten = item.Ten,
                    CreateDateTime = item.CreateDateTime
                };
                lst.Add(ks);
            }

            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";
            List<string> temp = new List<string>();
            temp.AddRange(list_getall.Select(_ => _.Ten).ToList());
            temp.AddRange(list_getall.Select(_ => _.MaDDC).ToList());
            ViewBag.list_search = temp;

            //Sắp xếp

            if (KeySearch.SapXep == "1" || KeySearch.SapXep == null || KeySearch.SapXep == "")
                lst = lst.OrderBy(_ => _.MaDDC).ToList();
            if (KeySearch.SapXep == "11")
                lst = lst.OrderByDescending(_ => _.MaDDC).ToList();
            if (KeySearch.SapXep == "2")
                lst = lst.OrderBy(_ => _.Ten).ToList();
            if (KeySearch.SapXep == "22")
                lst = lst.OrderByDescending(_ => _.Ten).ToList();

            return View(lst);
        }
    }
}