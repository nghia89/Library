using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BiTech.Library.Controllers.BaseClass;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class ThongKeController : BaseController
    {
        NghiepVuThongKe nghiepVu;
        static List<string> list_tk = new List<string>();// List chứa số sách được mượn và số người mượn sách

        public ThongKeController()
        {
            nghiepVu = new NghiepVuThongKe();
        }

        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MuonSach()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MuonSachJson(string day, int? month, int? year)
        {
            #region Khai báo
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<ThongTinMuonSach> listAll = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listMonthSelected = new List<ThongTinMuonSach>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);        
            #endregion
            if (day == null && month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            foreach (var item in listAll)
            {
                #region SẮP XẾP TRẠNG THÁI
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------             
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;
                // Ngày phải trả < ngày hiện tại và ngày trả == NULL là CHƯA TRẢ                  
                if (ngayPhaiTra < DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThaiString = ETinhTrangPhieuMuon.ChuaTra.ToString();
                    item.TenTrangThai = "Chưa trả";                  
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (ngayPhaiTra >= DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TrangThaiString = ETinhTrangPhieuMuon.GanTra.ToString();
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = ngayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra <= DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraDungHen.ToString();
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraTre.ToString();
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)ngayTraThucTe) - ngayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra > DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraDungHen.ToString();
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
                #endregion
                // --Sử lý theo Tháng Năm hoặc theo Ngày  
                if (day != null && day != "0")
                {
                    string[] arrTemp = day.Split('T');
                    day = arrTemp[0];
                    if (ngayMuon.ToString("dd/MM/yyyy") == day || ngayMuon.ToString("dd-MM-yyyy") == day)
                    {
                        item.NgayMuonTemp = item.NgayGioMuon.ToString("dd-MM-yyyy");
                        item.NgayTraTemp = item.NgayPhaiTra.ToString("dd-MM-yyyy");
                        #region Ngày trả thật tế
                        string ngayTra = null;
                        if (item.NgayTraThucTe != null)
                            ngayTra = item.NgayTraThucTe.ToString("dd-MM-yyyy");
                        if (ngayTra.Equals("01-01-0001") == false && ngayTra != null)
                        {
                            item.NgayTraThucTeTemp = ngayTra;
                            if (item.SoNgayGiaoDong != null)
                            {
                                // Trễ 
                                item.TenTrangThai = item.TenTrangThai + " - (" + (item.SoNgayGiaoDong.ToString() + " ngày)");
                            }
                            else
                            {
                                // đúng hạn
                                item.TenTrangThai = item.TenTrangThai;
                            }
                        }
                        else
                        {
                            item.NgayTraThucTeTemp = "---//---";
                            if (item.SoNgayGiaoDong != null && item.SoNgayGiaoDong < 30)
                            {
                                // Gần sắp trả
                                item.TenTrangThai = item.TenTrangThai + " - (còn " + item.SoNgayGiaoDong.ToString() + " ngày)";
                            }
                            else
                            {
                                if (item.SoNgayGiaoDong == null)
                                {
                                    // Trễ
                                    var ngayTre = DateTime.Now - item.NgayPhaiTra;
                                    item.TenTrangThai = item.TenTrangThai + " - (trễ " + ngayTre.Days + " ngày)";
                                }
                                else
                                {
                                    // Gần trả
                                    item.TenTrangThai = "Còn " + item.SoNgayGiaoDong.ToString() + " ngày";
                                }
                            }
                        }
                        #endregion
                        listMonthSelected.Add(item);
                    }
                }
                else
                {
                    if (ngayMuon.Year == year)
                    {
                        if (ngayMuon.Month == month)
                        {
                            item.NgayMuonTemp = item.NgayGioMuon.ToString("dd-MM-yyyy");
                            item.NgayTraTemp = item.NgayPhaiTra.ToString("dd-MM-yyyy");
                            #region Ngày trả thật tế
                            string ngayTra = null;
                            if (item.NgayTraThucTe != null)
                                ngayTra = item.NgayTraThucTe.ToString("dd-MM-yyyy");
                            if (ngayTra.Equals("01-01-0001") == false && ngayTra != null)
                            {
                                item.NgayTraThucTeTemp = ngayTra;
                                if (item.SoNgayGiaoDong != null)
                                {
                                    // Trễ 
                                    item.TenTrangThai = item.TenTrangThai + " - (" + (item.SoNgayGiaoDong.ToString() + " ngày)");
                                }
                                else
                                {
                                    // đúng hạn
                                    item.TenTrangThai = item.TenTrangThai;
                                }
                            }

                            else
                            {
                                item.NgayTraThucTeTemp = "---//---";
                                if (item.SoNgayGiaoDong != null && item.SoNgayGiaoDong < 30)
                                {
                                    // Gần sắp trả
                                    item.TenTrangThai = item.TenTrangThai + " - (còn " + item.SoNgayGiaoDong.ToString() + " ngày)";
                                }
                                else
                                {
                                    if (item.SoNgayGiaoDong == null)
                                    {
                                        // Trễ
                                        var ngayTre = DateTime.Now - item.NgayPhaiTra;
                                        item.TenTrangThai = item.TenTrangThai + " - (trễ " + ngayTre.Days + " ngày)";
                                    }
                                    else
                                    {
                                        // Gần trả
                                        item.TenTrangThai = "Còn " + item.SoNgayGiaoDong.ToString() + " ngày";
                                    }
                                }
                            }
                            #endregion                         
                            listMonthSelected.Add(item);
                        }
                    }
                }
            }
            // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------                   
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listMonthSelected)
                .OrderBy(x => x.TrangThai).ThenBy(x => x.NgayGioMuon).ToList();
            int i = 1;
            foreach (var item in listRutGon)
            {
                var user = _thongKeLogic.GetThanhVienByMSTV(item.idUser);
                if (user != null)
                {
                    item.TenThanhVien = user.Ten;
                }
                item.TenSach = _thongKeLogic.GetSachById(item.idSach).TenSach;
                item.STT = i.ToString();
                i++;
            }
            // Tổng ssdm và snms
            list_tk.Clear();
            list_tk.Add(listMonthSelected.Count().ToString());
            list_tk.Add(nghiepVu.DemSoNguoiMuonSach(listRutGon).ToString());
            return Json(listRutGon, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MuonSachJson_ssdm()
        {
            return Json(list_tk, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeTheoTuan(DateTime date)
        {
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            // Thống kê theo tuần          
            DateTime thisWeekStart = date.AddDays(-(int)date.DayOfWeek);
            DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            List<DateTime> listDates = new List<DateTime>();
            for (var i = 0; i < 7; i++)
            {
                listDates.Add(thisWeekStart.Date);
                thisWeekStart = thisWeekStart.AddDays(1);
            }
            BieuDoPhieuMuonViewModel model = new BieuDoPhieuMuonViewModel();
            model.thongKeTheoTuan = new List<int[]>();
            foreach (var item in listDates)
            {
                // list chứ thông tin thống kê của 1 ngày
                model.thongKeTheoTuan.Add(nghiepVu.ThongKeTheoTuan(item, Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName));
            }
            return View(model);
        }

        public ActionResult LichSuMuonSach(int? page, string idUser, int? pagePM, string day, int? month, int? year, bool MuonSach)
        {
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            // kiem tra null du lieu
            ThanhVien thanhVien = _thongKeLogic.GetThanhVienByMSTV(idUser);
            if (idUser == null || thanhVien == null || _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien) == null)
                return RedirectToAction("NotFound", "Error");
            List<Sach> listSach = new List<Sach>();
            ViewBag.TenThanhVien = thanhVien.Ten;
            var listPhieuMuon = _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien);
            if (listPhieuMuon == null)
                return RedirectToAction("NotFound", "Error");
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPhieuMuon)
            {
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;

                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (ngayPhaiTra <= DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (ngayPhaiTra > DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = ngayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // -- cong Phieu Tra vo               
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra <= DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)ngayTraThucTe) - ngayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra > DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
            }
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listPhieuMuon)
                .OrderBy(x => x.TrangThai)
                .ThenBy(x => x.NgayGioMuon).ToList();
            foreach (var item in listRutGon)
            {
                Sach sach = _thongKeLogic.GetSachById(item.idSach);
                listSach.Add(sach);
            }
            ViewBag.ListSach = listSach;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            ViewBag.Id = idUser;
            // ngay thang nam
            ViewBag.MuonSach = MuonSach;
            ViewBag.pagePM = pagePM;
            ViewBag.Day = day;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View(listRutGon.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DanhSachChuaTra(int? page, int? pagePM, string day, int? month, int? year)
        {
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<Sach> listSach = new List<Sach>();
            List<ThongTinMuonSach> listChuaTra = new List<ThongTinMuonSach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<string> listIdThanhVien = new List<string>();
            var listMuon = _thongKeLogic.GetAllTTMS();
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;           

                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null)
                {
                    // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                    if (ngayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = ngayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
            }
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listChuaTra)
                .OrderBy(x => x.TrangThai)
                .ThenBy(x => x.NgayGioMuon).ToList();
            foreach (var item in listRutGon)
            {
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienByMSTV(item.idUser));
                listIdThanhVien.Add(item.idUser);
                listSach.Add(_thongKeLogic.GetSachById(item.idSach));
            }
            // Thông tin Sách,Thành Viên
            ViewBag.ListThanhVien = listThanhVien;
            ViewBag.ListIdThanhVien = listIdThanhVien;
            ViewBag.ListSach = listSach;
            //  ViewBag.SoSachDuocMuon = soSachDuocMuon;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            // ngay thang nam
            ViewBag.Page = page;
            ViewBag.pagePM = pagePM;
            ViewBag.Day = day;
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;
            return View(listRutGon.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ThongTinDocGia(string idUser, int? page, ThongKeViewModel model)
        {
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThanhVien thanhVien = _thongKeLogic.GetThanhVienByMSTV(idUser);
            if (idUser == null || _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien) == null || thanhVien == null)
                return RedirectToAction("NotFound", "Error");
            List<ThongTinMuonSach> listChuaTra = new List<ThongTinMuonSach>();
            List<Sach> listSach = new List<Sach>();
            List<ThongTinMuonSach> listRutGon = new List<ThongTinMuonSach>();
            var listMuon = _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien);
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;        
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null)
                {
                    if (ngayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = ngayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
            }
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại           
            listRutGon = nghiepVu.RutGonDanhSach(listChuaTra)
                .OrderBy(x => x.TrangThai)
                 .ThenBy(x => x.NgayGioMuon).ToList();
            foreach (var item in listRutGon)
            {
                listSach.Add(_thongKeLogic.GetSachById(item.idSach));
            }
            model = new ThongKeViewModel
            {
                HinhChanDung = thanhVien.HinhChanDung,
                TenDocGia = thanhVien.Ten,
                DiaChi = thanhVien.DiaChi,
                SDT = thanhVien.SDT,
                TrangThaiUser = thanhVien.TrangThai,
                ListPhieuMuon = listRutGon as List<ThongTinMuonSach>,
                LopHoc = thanhVien.LopHoc,
                NienKhoa = thanhVien.NienKhoa,
                QRLink = thanhVien.QRLink,
                NgaySinh = thanhVien.NgaySinh,
                GioiTinh = thanhVien.GioiTinh,
                idUser = thanhVien.MaSoThanhVien,
                ChucVu = thanhVien.ChucVu
            };
            if (thanhVien.HinhChanDung != null)
                model.LinkAvatar = thanhVien.HinhChanDung;
            else
                model.LinkAvatar = "/Content/Images/user5.jpg";
            ViewBag.Page = page;
            ViewBag.IdThanhVien = idUser;
            ViewBag.ListSach = listSach;
            return View(model);
        }
    }
}