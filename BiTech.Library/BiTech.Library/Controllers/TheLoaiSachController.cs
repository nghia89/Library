using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BiTech.Library.Helpers.Tool;

namespace BiTech.Library.Controllers
{
    public class TheLoaiSachController : BaseController
    {
        // GET: TheLoaiSach
        public ActionResult Index(int? page, string idParent)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _TheLoaiSachLogic.GetAllTheLoaiSach();
            List<TheLoaiSachViewModels> list_viewmode = new List<TheLoaiSachViewModels>();
            foreach (TheLoaiSach item in list)
            {
                TheLoaiSachViewModels Theloai = new TheLoaiSachViewModels();
                Theloai.Id = item.Id;
                Theloai.IdParent = item.IdParent;
                Theloai.TenTheLoai = item.TenTheLoai;
                if (item.IdParent != null)
                {
                    var parent = _TheLoaiSachLogic.getById(item.IdParent);
                    if (parent != null)
                        Theloai.TenTheLoaiParent = parent.TenTheLoai;
                }
                Theloai.MoTa = item.MoTa;
                Theloai.MaDDC = item.MaDDC;
                list_viewmode.Add(Theloai);
            }

            //filter
            var folder = _TheLoaiSachLogic.getById(idParent);
            if (idParent != null)
            {
                //Nếu các thể loại con của idParent
                list_viewmode = list_viewmode.Where(_ => _.IdParent == idParent).ToList();

                if (folder != null)
                {
                    ViewBag.URLBackParent = (folder.IdParent != null) ? "/TheLoaiSach?idParent=" + folder.IdParent : "/TheLoaiSach";
                }
            }
            else
            {
                //Lấy các câu không có idParent
                list_viewmode = list_viewmode.Where(_ => _.IdParent == null).ToList();
            }

            var list_viewmode_view = list_viewmode.OrderBy(_ => _.MaDDC);
            ViewBag.idParent = (idParent == null || idParent == "") ? null : idParent;
            ViewBag.url = GetViewLinkURL(folder);
            // Phân trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.paged = page;
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            ViewBag.number = list_viewmode_view.Count();
            return View(list_viewmode_view.OrderBy(c => c.MaDDC).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Them()
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();

            return View();
        }

        [HttpPost]
        public ActionResult Them(TheLoaiSach model)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TheLoaiSach TLS = new TheLoaiSach()
            {
                IdParent = model.IdParent,
                TenTheLoai = model.TenTheLoai,
                MoTa = model.MoTa,
                MaDDC = model.MaDDC
            };

            _TheLoaiSachLogic.ThemTheLoaiSach(TLS);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(TheLoaiSach model)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            try
            {
                TheLoaiSach TLS = new TheLoaiSach()
                {
                    TenTheLoai = model.TenTheLoai,
                    IdParent = model.IdParent,
                    MoTa = model.MoTa,
                    MaDDC = model.MaDDC
                };

                //todo bàn lại sau
                //Kiểm tra ddc level mấy
                //Kiểm tra ddc levet tra đã tồn tại chưa
                //Nếu không có ddc thì theo idparent

                if (check_NameChildren(TLS, model.IdParent))
                {
                    _TheLoaiSachLogic.ThemTheLoaiSach(TLS);
                    return Json(true);
                }
                else
                {
                    return Json("Tên thể loại đã tồn tại");
                }
            }
            catch { return Json(false); }
        }

        [HttpGet]
        public ActionResult Sua(string idTheLoai)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(idTheLoai);
            if (TLS == null)
                return RedirectToAction("Index", "Error");
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                IdParent = TLS.IdParent,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
                MaDDC = TLS.MaDDC,
            };
            var list_root = _TheLoaiSachLogic.GetAllTheLoaiSachRoot();
            List<TheLoaiSach> ListTheLoai = new List<TheLoaiSach>();
            foreach (TheLoaiSach item in list_root)
            {
                ListTheLoai.Add(item);
                ListTheLoai.AddRange(ListTheLoaiChildren(item, _TheLoaiSachLogic));
            }

            var folder = _TheLoaiSachLogic.getById(idTheLoai);
            if (folder != null)
            {
                ViewBag.URLBackParent = (folder.IdParent != null) ? "/TheLoaiSach?idParent=" + folder.IdParent : "/TheLoaiSach";
            }

            ViewBag.ListTheLoai = ListTheLoai;
            return View(VM);

        }

        public List<TheLoaiSach> ListTheLoaiChildren(TheLoaiSach _TheLoaiSach, TheLoaiSachLogic _TheLoaiSachLogic)
        {
            List<TheLoaiSach> kq = new List<TheLoaiSach>();
            var list = _TheLoaiSachLogic.GetAllTheLoaiSachChildren(_TheLoaiSach.Id);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.TenTheLoai = " " + item.TenTheLoai;
                    kq.Add(item);
                    kq.AddRange(ListTheLoaiChildren(item, _TheLoaiSachLogic));
                }
            }
            return kq;
        }

        [HttpPost]
        public ActionResult Sua(TheLoaiSachViewModels model)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(model.Id);
            if (TLS == null)
                return RedirectToAction("Index", "Error");
            int so = (TLS.TenTheLoai == model.TenTheLoai) ? 1 : 0;
            TLS.IdParent = model.IdParent;
            TLS.TenTheLoai = model.TenTheLoai;
            TLS.MoTa = model.MoTa;
            TLS.MaDDC = model.MaDDC;
            if (check_NameChildren(TLS, model.IdParent, so))
            {
                _TheLoaiSachLogic.SuaTheLoaiSach(TLS);
                return RedirectToAction("Index", "TheLoaiSach", new { idParent = TLS.IdParent });
            }
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                IdParent = TLS.IdParent,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
                MaDDC = TLS.MaDDC,
            };

            ViewBag.URLBackParent = (VM.IdParent != null) ? "/TheLoaiSach?idParent=" + VM.IdParent : "/TheLoaiSach";
            ViewBag.error_title = "Tên thể loại được sửa đã tồn tại";
            return View(VM);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SuaAjax(string id_TL, string idParent)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            try
            {
                idParent = (idParent == "") ? null : idParent;
                string tb = "";
                if (CheckUpdate_parent(id_TL, idParent, ref tb))
                {
                    TheLoaiSach TLS = _TheLoaiSachLogic.getById(id_TL);
                    TLS.IdParent = idParent;
                    _TheLoaiSachLogic.SuaTheLoaiSach(TLS);
                    return Json(true);
                }
                else
                {
                    return Json(tb);
                }

            }
            catch { return Json(false); }
        }

        [HttpPost]
        public ActionResult Xoa(string Id)
        {
            XoaTheLoai(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteMany(List<string> chon, string paging, string pageSize, string colRowCount, string idParent)
        {
            try
            {
                if (paging != "")
                {
                    //Count max item in page
                    int kq_so1 = (int.Parse(paging) * int.Parse(pageSize)) - int.Parse(colRowCount);
                    int SlItemInPage = int.Parse(pageSize);

                    //kq_so1 >= 0 => đang ở page cuối
                    if (kq_so1 >= 0)
                    {
                        SlItemInPage = SlItemInPage - kq_so1;

                        //Nếu cái item ở trang cuối được xoá hết thì luồi 1 page
                        if (chon.Count == SlItemInPage)
                        {
                            paging = (((int.Parse(paging) - 1) == 0) ? 1 : int.Parse(paging) - 1).ToString();
                        }
                    }
                }

                if (chon != null)
                {
                    foreach (string item in chon)
                    {
                        XoaTheLoai(item);
                    }
                }

                return RedirectToAction("Index", "TheLoaiSach", new
                {
                    idParent = idParent,
                    page = paging
                });
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        /// <summary>
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemTheLoaiGui(string idParent)
        {
            ViewBag.idParent = (idParent == null || idParent == "") ? null : idParent;
            return PartialView("_NhapLoaiSach");
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(TheLoaiSachViewModels model)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ExcelManager excelManager = new ExcelManager();
            List<TheLoaiSach> listExcel = new List<TheLoaiSach>();
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
                    listExcel = excelManager.ImportTheLoaiSach(sourceDir);
                }
                foreach (var item in listExcel)
                {
                    // ktr mã DDC chưa tồn tại thì thêm
                    if (_TheLoaiSachLogic.ktrTrung(item) == false)
                    {
                        _TheLoaiSachLogic.ThemTheLoaiSach(item);
                    }
                }
                var listAll = _TheLoaiSachLogic.GetAllTheLoaiSach();

                foreach (var i in listExcel)
                {
                    foreach (var j in listAll)
                    {
                        if (i.IdParent == j.MaDDC)
                        {
                            i.IdParent = j.Id;
                            _TheLoaiSachLogic.Update(i);
                        }
                    }
                }
            }
            // return View();
            return RedirectToAction("Index", "TheLoaiSach");
        }

        #region AngularJS

        public JsonResult Get_AllTheLoaiSach() //JsonResult
        {
            TheLoaiSachLogic _TheLoaiSachLogic =
                new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _TheLoaiSachLogic.GetAllTheLoaiSach(true);
            return Json(list.OrderBy(c => c.MaDDC), JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Phong function
        /// <summary>
        /// tạo link điều hướng cho các thể loại con
        /// </summary>
        /// <param name="TLS"></param>
        /// <returns></returns>
        private string GetViewLinkURL(TheLoaiSach TLS)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            string kq = "";
            if (TLS == null)
                return kq;
            else
            {
                kq = kq + "<i class=\"arrow right_icon\"></i><a href=/TheLoaiSach?idParent=" + TLS.Id + ">" + TLS.TenTheLoai + "</a>";
                TheLoaiSach TLSParent = _TheLoaiSachLogic.getById(TLS.IdParent);
                kq = GetViewLinkURL(TLSParent) + kq;
            }
            return kq;
        }

        /// <summary>
        /// Kiểm tra idParent_TL và id_TL có hợp lệ để update id_TL.idParent không
        /// </summary>
        /// <param name="id_TL"></param>
        /// <param name="idParent_TL"></param>
        /// <returns></returns>
        private bool CheckUpdate_parent(string id_TL, string idParent_TL, ref string tb)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            #region idParent_TL có là con hoặc cháu của id_TL không

            if (idParent_TL == null)
            {

            }
            else
            {
                //idParent_TL có là con hoặc cháu của id_TL không nếu có thì return false
                var list_id_ancestors = Get_TreeFamily_ID(_TheLoaiSachLogic.getById(idParent_TL));
                //id_TL = idParent_TL return false (1 object không thể là con của chính nó)
                list_id_ancestors.Add(idParent_TL);
                if (list_id_ancestors.Where(_ => _ == id_TL).ToList().Count() > 0)
                {
                    tb = "Không thể dán vào thể loại con của thể loại đang cắt";
                    return false;
                }

            }
            #endregion

            if (!check_NameChildren(_TheLoaiSachLogic.getById(id_TL), idParent_TL))
            {
                tb = "Tên thể loại đã tồn tại";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Nếu TLS trùng tên chidren idParent
        /// </summary>
        /// <param name="TLS"></param>
        /// <param name="idParent"></param>
        /// <returns></returns>
        private bool check_NameChildren(TheLoaiSach TLS, string idParent, int dk_so = 0)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<TheLoaiSach> list_chlidren = new List<TheLoaiSach>();
            if (idParent == null)
            {
                var list_TLS = _TheLoaiSachLogic.GetAllTheLoaiSach();
                list_chlidren = list_TLS.Where(_ => _.IdParent == null).ToList();
            }
            else
            {
                list_chlidren = _TheLoaiSachLogic.GetAllTheLoaiSachChildren(idParent);

            }
            if (list_chlidren.Where(_ => _.TenTheLoai == TLS.TenTheLoai).ToList().Count() > dk_so)
                return false;
            return true;
        }

        /// <summary>
        /// Lấy danh sách id tổ tiên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_check"></param>
        /// <returns></returns>
        private List<string> Get_TreeFamily_ID(TheLoaiSach id_check)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<string> list_id = new List<string>();
            if (id_check.IdParent == null)
            {
                return list_id;
            }
            else
            {
                list_id.Add(id_check.IdParent);
                list_id.AddRange(Get_TreeFamily_ID(_TheLoaiSachLogic.getById(id_check.IdParent)));
            }
            return list_id;
        }


        private bool XoaTheLoai(string id)
        {
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var TLS = _TheLoaiSachLogic.getById(id);
            _TheLoaiSachLogic.XoaTheLoaiSach(TLS.Id);
            return true;
        }
        #endregion
    }
}