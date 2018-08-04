using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class TraSachController : BaseController
    {
        //todo
        static public List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
        public ActionResult Index(string IdUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            //Danh sách thành viên
            List<UserViewModel> list_user = new List<UserViewModel>();
            list_user.Add(new UserViewModel() { MaSoThanhVien = "123", Ten = "Phong" });
            list_user.Add(new UserViewModel() { MaSoThanhVien = "456", Ten = "Tai" });
            list_user.Add(new UserViewModel() { MaSoThanhVien = "789", Ten = "Thinh" });
            //Danh sách sách đang mượn
            list_book.Clear();
            list_book.Add(new MuonTraSachViewModel()
            {
                MaKiemSoat = "01",
                IdUser = "123",
                TenSach = "One Piece tâp 1",
                SoLuong = "1",
                NgayMuon = (new DateTime(2018, 2, 20)).Date.ToString("dd/MM/yyyy"),
                NgayTra = (new DateTime(2018, 6, 28)).Date.ToString("dd/MM/yyyy"),
                TinhTrang = ((new DateTime(2018, 6, 28)) - DateTime.Now).TotalDays < 0
            });
            list_book.Add(new MuonTraSachViewModel() { MaKiemSoat = "02", IdUser = "123", TenSach = "One Piece tâp 2", SoLuong = "4", NgayMuon = (new DateTime(2018, 2, 20)).Date.ToString("dd/MM/yyyy"), NgayTra = (new DateTime(2018, 8, 28)).Date.ToString("dd/MM/yyyy") });
            list_book.Add(new MuonTraSachViewModel() { MaKiemSoat = "03", IdUser = "123", TenSach = "One Piece tâp 3", SoLuong = "3", NgayMuon = (new DateTime(2018, 2, 20)).Date.ToString("dd/MM/yyyy"), NgayTra = (new DateTime(2018, 8, 28)).Date.ToString("dd/MM/yyyy") });
            list_book.Add(new MuonTraSachViewModel()
            {
                MaKiemSoat = "04",
                IdUser = "456",
                TenSach = "One Piece tâp 4",
                SoLuong = "3",
                NgayMuon = (new DateTime(2018, 2, 20)).Date.ToString("dd/MM/yyyy"),
                NgayTra = (new DateTime(2018, 4, 28)).Date.ToString("dd/MM/yyyy"),
                TinhTrang = ((new DateTime(2018, 3, 28)) - DateTime.Now).TotalDays < 0
            });
            list_book.Add(new MuonTraSachViewModel()
            {
                MaKiemSoat = "05",
                IdUser = "456",
                TenSach = "One Piece tâp 5",
                SoLuong = "3",
                NgayMuon = (new DateTime(2018, 2, 20)).Date.ToString("dd/MM/yyyy"),
                NgayTra = (new DateTime(2018, 8, 28)).Date.ToString("dd/MM/yyyy"),
                TinhTrang = ((new DateTime(2018, 8, 28)) - DateTime.Now).TotalDays < 0
            });

            //model
            UserViewModel _thanhvienmodoe = null;

            //ViewBag
            ViewBag.hasUser = false; // biến kiểm tra có tồn tại IdUser không
            ViewBag.ThongBao = false; //Có hiện thị thông báo hay không
            ViewBag.ThongBaoString = ""; //Nội dung thông báo
            ViewBag.user = null; // Giữ thông tin user nếu đăng nhập thành công
            ViewBag.MaxDate = 10;

            if (IdUser == null)
            {
                //load mặc định (chưa tìm kiếm)
            }
            else if (IdUser == "" || IdUser.Trim() == "")
            {
                //không có IdUser
                ViewBag.ThongBao = true;
                ViewBag.ThongBaoString = "Bạn hãy nhập mã thành viên";
            }
            else
            {
                //Có IdUser
                //Kiểm tra IdUser có tồn tại trong database không
                //to do (khi kết hợp với code của team sẽ làm lại - hiện tại không lấy dữ liệu trong database)
                ////ThanhVien tv = _ThanhVienLogic.GetById(IdUser);
                ////if(tv != null)
                ////{
                ////}
                _thanhvienmodoe = list_user.Where(_ => _.MaSoThanhVien == IdUser).SingleOrDefault();
                if (_thanhvienmodoe != null)
                {
                    ViewBag.user = _thanhvienmodoe;
                    ViewBag.hasUser = true;
                }
                else
                {
                    ViewBag.ThongBao = true;
                    ViewBag.ThongBaoString = "Thành viên này không tồn tại";
                }

            }

            return View(list_book);
        }

        [HttpPost]
        public JsonResult GetBook_TraSach(string maSach, string IdUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            SachLogic _SachLogicLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<Sach> list = _SachLogicLogic.getAllSach();
            list.Clear();
            
            var list_temp = list_book.Where(_ => _.MaKiemSoat == maSach && _.IdUser == IdUser).ToList();

            return Json(list_temp, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetListBook_IdUser(string IdUser)
        {
            var list = list_book.Where(_ => _.IdUser == IdUser).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_item)
        {
            if (List_item.Count > 0)
            {

                foreach (MuonTraSachViewModel item in List_item)
                {
                    MuonTraSachViewModel _sachTeam = list_book.Where(_ => _.MaKiemSoat == item.MaKiemSoat).SingleOrDefault();
                    if(_sachTeam != null)
                    {
                        if(_sachTeam.SoLuong == item.SoLuong)
                        {
                            //Trả hết
                            list_book.Remove(_sachTeam);
                        }
                        else
                        {
                            //Trả theo số lượng cuốn
                            _sachTeam.SoLuong = (int.Parse(_sachTeam.SoLuong) - int.Parse(item.SoLuong)).ToString();
                        }
                    }
                }
                var list_temp = list_book.Where(_ => _.IdUser == List_item[0].IdUser).ToList();
                return Json(list_temp, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }



    }
}