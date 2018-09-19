using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class TrangThaiSachController : BaseController
    {
        // GET: TrangThai
        public ActionResult Index(int? page)
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            
            var lst = _TrangThaiSachLogic.GetAll();
            List<TrangThaiSachViewModels> lsttt = new List<TrangThaiSachViewModels>();
            foreach (var item in lst)
            {
                TrangThaiSachViewModels tt = new TrangThaiSachViewModels()
                {
                    Id = item.Id,
                    TenTT = item.TenTT,
                    TrangThai = item.TrangThai
                };
                lsttt.Add(tt);
            }
            // Phân trang
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lsttt.OrderBy(x => x.TenTT).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(TrangThaiSachViewModels model)
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TrangThaiSach tts = new TrangThaiSach()
            {
                TenTT = model.TenTT,
                TrangThai = model.TrangThai
            };
            _TrangThaiSachLogic.ThemTrangThai(tts);
            return RedirectToAction("Index");
        }

        public ActionResult Sua(string id)
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            if (tts == null)
                return RedirectToAction("NotFound", "Error");
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT,
                TrangThai = tts.TrangThai
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Sua(TrangThaiSachViewModels model)
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            tts.TenTT = model.TenTT;
            tts.TrangThai = model.TrangThai;
            _TrangThaiSachLogic.SuaTrangThai(tts);
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult Xoa(string Id)
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var tts = _TrangThaiSachLogic.getById(Id);
            _TrangThaiSachLogic.XoaTrangThai(tts.Id);
            return RedirectToAction("Index");
        }
        #region AngularJS

        public JsonResult Get_AllTrangThaiSach() //JsonResult
        {
            TrangThaiSachLogic _TrangThaiSachLogic =
                new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _TrangThaiSachLogic.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}