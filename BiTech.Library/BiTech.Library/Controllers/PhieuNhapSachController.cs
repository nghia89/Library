using BiTech.Library.BLL.DBLogic;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BiTech.Library.Controllers.BaseClass;
using static BiTech.Library.Helpers.Tool;
using System.IO;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    public class PhieuNhapSachController : BaseController
    {
        public ActionResult Index(int? page)
        {
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            var lst = _PhieuNhapSachLogic.Getall();
            List<PhieuNhapSachModels> lstpns = new List<PhieuNhapSachModels>();
            foreach (var item in lst)
            {
                PhieuNhapSachModels pns = new PhieuNhapSachModels()
                {
                    IdPhieuNhap = item.Id,
                    IdUserAdmin = item.IdUserAdmin,
                    GhiChu = item.GhiChu,
                    NgayNhap = item.CreateDateTime,
                    UserName = item.UserName
                };
                lstpns.Add(pns);
            }

            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lstpns.OrderByDescending(x => x.NgayNhap).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(string id)
        {
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic =
              new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic =
             new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var model = _ChiTietNhapSachLogic.GetAllChiTietById(id);
            var phieunhap = _PhieuNhapSachLogic.GetById(id);

            if (phieunhap == null || model == null)
                return RedirectToAction("NotFound", "Error");

            PhieuNhapSachModels pns = new PhieuNhapSachModels()
            {

                IdUserAdmin = phieunhap.IdUserAdmin,
                GhiChu = phieunhap.GhiChu,
                NgayNhap = phieunhap.CreateDateTime

            };
            List<ChiTietNhapSachViewModels> lst = new List<ChiTietNhapSachViewModels>();
            foreach (var item in model)
            {
                ChiTietNhapSachViewModels ctns = new ChiTietNhapSachViewModels();
                ctns.Id = item.Id;
                ctns.IdTinhTrang = item.IdTinhtrang;
                var TinhTrang = _TrangThaiSachLogic.getById(ctns.IdTinhTrang);
                ctns.tenTinhTrang = TinhTrang.TenTT;
                ctns.IdSach = item.IdSach;
                var TenSach = _SachLogic.GetBookById(ctns.IdSach);
                ctns.ten = TenSach.TenSach;
                ctns.soLuong = item.SoLuong;
                ctns.GhiChuDon = item.GhiChu;
                lst.Add(ctns);

            }
            ViewBag.lstctnhap = lst;

            return View(pns);
        }

        public ActionResult TaoPhieuNhapSach()
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuNhapSach(PhieuNhapSachModels model)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.UserName
                };

                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);

                if (!String.IsNullOrEmpty(idPhieuNhap))
                {
                    foreach (var json in model.listChiTietJsonString)
                    {
                        var ctModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChiTietNhapSachViewModels>(json);

                        var ctns = new ChiTietNhapSach()
                        {
                            IdPhieuNhap = idPhieuNhap,
                            IdSach = ctModel.IdSach,
                            //tenTinhTrang = ctModel.tenTinhTrang,
                            SoLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now,
                            IdTinhtrang = ctModel.IdTinhTrang,
                            GhiChu = ctModel.GhiChuDon
                        };

                        _ChiTietNhapSachLogic.Insert(ctns);
                        {
                            // update số lượng sách trạng thái
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctns.IdSach, ctModel.IdTinhTrang);
                            if (sltt != null)
                            {
                                sltt.SoLuong += ctns.SoLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = ctns.IdSach;
                                sltt.IdTrangThai = ctModel.IdTinhTrang;
                                sltt.SoLuong = ctns.SoLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }

                            // update tổng số lượng sách
                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuongTong += ctns.SoLuong;

                            // ktr neu cho muon moi cong them
                            //var tinhTrang = _TrangThaiSachLogic.getById(ctModel.IdTinhTrang);
                            //if (tinhTrang.TrangThai == true)
                            updatesl.SoLuongConLai += ctns.SoLuong;

                            _SachLogic.Update(updatesl);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ModelState.Clear();

            return View();
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromExcel(PhieuNhapSachModels model)
        {
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ExcelManager excelManager = new ExcelManager();
            List<ChiTietNhapSach> list = new List<ChiTietNhapSach>();
            if (model.LinkExcel != null)
            {
                string uploadForder = GetUploadFolder(Helpers.UploadFolder.FileExcel);
                string physicalWebRootPath = Server.MapPath("/");

                var sourceFileName = Path.Combine(physicalWebRootPath, uploadForder, model.LinkExcel.FileName);
                string location = Path.GetDirectoryName(sourceFileName);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Create))
                {
                    model.LinkExcel.InputStream.CopyTo(fileStream);
                    var sourceDir = fileStream.Name.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                    // Todo Excel
                    list = excelManager.ImportPhieuNhapSach(sourceDir);
                }
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.UserName
                };
                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);
                var listAllTTS = _TrangThaiSachLogic.GetAll();
                int i = 0;
                foreach (var item in list)
                {
                    var sach = _SachLogic.GetByMaMaKiemSoat(item.IdSach);
                    if (sach == null)
                    {
                        ViewBag.NullSach = "Mã sách không tồn tại ở dòng thứ " + (item.RowExcel + i).ToString();
                        return View();
                    }
                    i++;
                    int index = Int32.Parse(item.IdTinhtrang);
                    TrangThaiSach trangThaiSach = null;
                    if (index <= listAllTTS.Count())
                        trangThaiSach = listAllTTS[index - 1];
                    if (trangThaiSach != null && sach != null)
                    {
                        item.IdSach = sach.Id;
                        item.IdPhieuNhap = idPhieuNhap;
                        item.IdTinhtrang = trangThaiSach.Id;

                        _ChiTietNhapSachLogic.Insert(item);
                        //todo
                        {
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(sach.Id, trangThaiSach.Id);
                            if (sltt != null)
                            {
                                sltt.SoLuong += item.SoLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = sach.Id;
                                sltt.IdTrangThai = trangThaiSach.Id;
                                sltt.SoLuong = item.SoLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }

                            var updateSach = _SachLogic.GetBookById(sltt.IdSach);
                            updateSach.SoLuongTong += item.SoLuong;
                            if (trangThaiSach.TrangThai == true)
                                updateSach.SoLuongConLai += item.SoLuong;
                            _SachLogic.Update(updateSach);
                        }
                    }
                }
            }
            //return View();
            return RedirectToAction("Index", "PhieuNhapSach");
        }

        // Ajax ---

        [HttpGet]
        public JsonResult _GetBookItemById(string maKS, int soLuong, string idtrangthai, string GhiChuDon)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            if (maKS != null)
            {
                maKS = maKS.Trim();
                GhiChuDon = GhiChuDon.Trim();

                JsonResult result = new JsonResult();
                result.Data = null;
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                if (!string.IsNullOrEmpty(maKS) && !string.IsNullOrEmpty(idtrangthai) && soLuong > 0)
                {
                    var book = _SachLogic.GetByMaMaKiemSoat(maKS);

                    var tt = _TrangThaiSachLogic.getById(idtrangthai);
                    if (book != null && tt != null)
                    {
                        ChiTietNhapSachViewModels pp = new ChiTietNhapSachViewModels()
                        {
                            IdSach = book.Id,
                            ten = book.TenSach,
                            soLuong = soLuong,
                            IdTinhTrang = idtrangthai,
                            tenTinhTrang = tt.TenTT,
                            MaKiemSoat = book.MaKiemSoat,
                            GhiChuDon = GhiChuDon
                        };
                        result.Data = pp;
                    }  
                }
                return result;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Autocomplete(string a)
        {
            SachLogic _SachLogic =
               new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var ListTD = (from N in _SachLogic.getAll()
                          where N.MaKiemSoat.StartsWith(a)
                          select new { N.MaKiemSoat });


            return Json(ListTD, JsonRequestBehavior.AllowGet);
        }

        //VINH
        public JsonResult GetBookByID(string idSach)
        {
            SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach _sach = _SachLogicLogic.GetBook_NonDelete_ByMKS(new SachCommon().GetInfo(idSach));

            return Json(_sach, JsonRequestBehavior.AllowGet);
        }

		public ActionResult PreToInsert(List<SoLuongTrangThaiSachVM> lstModel)
		{
			ViewData["LstTTS"] = lstModel;
			return Json(true);
		}		
	}
}