using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BiTech.Library.Controllers.BaseClass;
using static BiTech.Library.Helpers.Tool;
using System.Threading.Tasks;
using Aspose.Cells;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BiTech.Library.Controllers
{
    public class GiaoVienController : BaseController
    {
        ThanhVienCommon thanhVienCommon;
        public GiaoVienController()
        {
            thanhVienCommon = new ThanhVienCommon();
        }

        // GET: User
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            UserViewModel model = new UserViewModel();
            List<ThanhVien> listThanhVien = _ThanhVienLogic.GetAllGV();
            int i = 0;
            model.ListAll = new string[listThanhVien.Count * 2];
            foreach (var item in listThanhVien)
            {
                // auto complete
                model.ListAll[i] = item.Ten;
                model.ListAll[i + 1] = item.MaSoThanhVien;
                i += 2;
            }
            model.ListThanhVien = listThanhVien; // PartialView
            ViewBag.ThongBao = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion           
            List<ThanhVien> listAll = _ThanhVienLogic.GetAllGV();
            ViewBag.ThongBao = false;
            model.ListThanhVien = new List<ThanhVien>();
            int i = 0;
            model.ListAll = new string[listAll.Count * 2];
            foreach (var item in listAll)
            {
                model.ListAll[i] = item.Ten;
                model.ListAll[i + 1] = item.MaSoThanhVien;
                i += 2;
            }
            // Todo
            string strSearch = model.TextForSearch;
            var listByName = _ThanhVienLogic.GetByName(strSearch);
            var listByMSTV = _ThanhVienLogic.GetByMaSoThanhVien(strSearch);
            if (listByName.Count > 0)
                model.ListThanhVien = listByName;
            else if (listByMSTV != null)
                model.ListThanhVien = new List<ThanhVien>() { listByMSTV };
            else
            {
                model.ListThanhVien = listAll;
                ViewBag.ThongBao = true;
                ViewBag.SearchFail = "Chưa tìm được kết quả phù hợp!";
            }
            return View(model);
        }

        public PartialViewResult _PartialUser(int? page, string IdUser, List<ThanhVien> list)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            return PartialView(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _CreateUser()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion           
            UserViewModel model = new UserViewModel();
            model.TemptNgaySinh = "--/--/----";
            return View(model);
        }

        [HttpPost]
        public ActionResult _CreateUser(UserViewModel viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion          
            ThanhVien thanhVien = new ThanhVien()
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Ten = viewModel.Ten,
                MaSoThanhVien = viewModel.MaSoThanhVien,
                NgaySinh = viewModel.NgaySinh,
                GioiTinh = viewModel.GioiTinh,
                LopHoc = viewModel.LopHoc,
                DiaChi = viewModel.DiaChi,
                SDT = viewModel.SDT,
                NienKhoa = viewModel.NienKhoa,
                //IdChucVu = viewModel.IdChucVu,
                TrangThai = EUser.Active, // mac dinh la Active
                CreateDateTime = DateTime.Now,
                // Loại tài khoản 
                LoaiTK = "gv"
            };
            DateTime ngaySinh = new DateTime();
            if (viewModel.TemptNgaySinh.Equals("--/--/----") == false)
            {
                ngaySinh = DateTime.ParseExact(viewModel.TemptNgaySinh, "dd/MM/yyyy", null);
                thanhVien.NgaySinh = ngaySinh;
            }
            else
            {
                ViewBag.NullNgaySinh = "Bạn chưa chọn ngày sinh!";
                return View(viewModel);
            }
            // Kiem tra trung ma thanh vien
            var idMaThanhVien = _ThanhVienLogic.GetByMaSoThanhVien(viewModel.MaSoThanhVien);
            if (idMaThanhVien == null)
            {
                string id = _ThanhVienLogic.Insert(thanhVien);
                ThanhVien tv = _ThanhVienLogic.GetById(id);
                string physicalWebRootPath = Server.MapPath("/");
                ThanhVien temp = new ThanhVien();

                // Lưu hình chân dung 
                //viewModel.LinkAvatar = "";
                //if (viewModel.HinhChanDung != null && Tool.IsImage(viewModel.HinhChanDung))
                //{
                //    viewModel.LinkAvatar = viewModel.HinhChanDung.FileName;
                //}
                if (viewModel.HinhChanDung != null)
                {
                    try
                    {
                        temp = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, tv, null, viewModel.HinhChanDung);
                        // viewModel.LinkAvatar = temp.HinhChanDung;
                        tv.HinhChanDung = temp.HinhChanDung;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ImageUnsuccess = "Lưu hình chân dung thất bại\r\n" + ex.Message;
                        viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                        return View(viewModel);
                    }
                }
                // Lưu mã vạch
                try
                {

                    temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null);
                    if (temp != null)
                    {
                        tv.QRLink = temp.QRLink;
                        tv.QRData = temp.QRData;
                    }
                }
                catch (Exception ex)
                {
                    TempData["UnSuccess"] = "Tạo mã QR thất bại\r\n" + ex.Message;
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                if (id == null || id == "")
                {
                    //Fail
                    ViewBag.UnSuccess = "Thêm mới thất bại";
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                else
                {
                    _ThanhVienLogic.Update(tv);
                }
            }
            else
            {
                ViewBag.Duplicate = "Mã số thành viên bị trùng";
                viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                return View(viewModel);
            }
            return RedirectToAction("Index", "GiaoVien");
        }

        //Get
        public ActionResult _Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.Success = TempData["Success"];
            ViewBag.UnSucces = TempData["UnSuccess"];
            ThanhVien tv = _ThanhVienLogic.GetById(id);
            if (tv == null)
                return RedirectToAction("NotFound", "Error");
            // tv.NgaySinh.ToString("dd-mm-yyyy");

            EditUserViewModel model = new EditUserViewModel()
            {
                // thông tin cho phép cập nhật
                Ten = tv.Ten,
                DiaChi = tv.DiaChi,
                SDT = tv.SDT,
                GioiTinh = tv.GioiTinh,
                NgaySinh = tv.NgaySinh,
                LopHoc = tv.LopHoc,
                NienKhoa = tv.NienKhoa,
                LinkAvatar = tv.HinhChanDung,
                Id = tv.Id
                // thông tin không được thay đổi 

                /* UserName = tv.UserName,
                Password = tv.Password,
                MaSoThanhVien = tv.MaSoThanhVien,
                ChucVu = tv.ChucVu,
                TrangThai = tv.TrangThai, */
            };
            //model.LinkAvatar = "";
            //if (model.HinhChanDung != null && Tool.IsImage(model.HinhChanDung))
            //{
            //    model.LinkAvatar = model.HinhChanDung.FileName;
            //}
            //ViewBag.HinhChanDung = tv.HinhChanDung;
            return View(model);
        }

        [HttpPost]
        public ActionResult _Edit(EditUserViewModel viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion          
            var thanhVien = _ThanhVienLogic.GetById(viewModel.Id);
            // thông tin cho phép cập nhật            
            thanhVien.LopHoc = viewModel.LopHoc;
            thanhVien.NienKhoa = viewModel.NienKhoa;
            thanhVien.Ten = viewModel.Ten;
            thanhVien.DiaChi = viewModel.DiaChi;
            thanhVien.GioiTinh = viewModel.GioiTinh;
            thanhVien.NgaySinh = viewModel.NgaySinh;
            thanhVien.SDT = viewModel.SDT;


            if (viewModel.HinhChanDung != null)
            {
                try
                {
                    // Cập nhật Hình Chân Dung
                    string physicalWebRootPath = Server.MapPath("/");
                    string imageName = null;
                    if (thanhVien.HinhChanDung != null)
                        imageName = thanhVien.HinhChanDung.Replace(@"/Upload/AvatarUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                    ThanhVien tempt = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, thanhVien, imageName, viewModel.HinhChanDung);
                    if (tempt != null)
                    {
                        thanhVien.HinhChanDung = tempt.HinhChanDung;
                    }
                }
                catch { }
            }
            try
            {
                // Cập nhật QR
                string physicalWebRootPath = Server.MapPath("/");
                string imageName = null;
                if (thanhVien.QRLink != null)
                    imageName = thanhVien.QRLink.Replace(@"/Upload/QRCodeUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                ThanhVien temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, thanhVien, imageName);
                if (temp != null)
                {
                    thanhVien.QRLink = temp.QRLink;
                    thanhVien.QRData = temp.QRData;
                }
            }
            catch { }

            bool resultInfo = _ThanhVienLogic.Update(thanhVien);

            if (resultInfo == true)
            {
                ViewBag.UpdateSuccess = "Cập nhật thành công!";
            }
            else
            {
                ViewBag.UpdateFail = "Cập nhật không thành công!";
            }
            return View(viewModel);
        }

        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            ThanhVien thanhVien = _ThanhVienLogic.GetById(id);
            if (thanhVien == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (thanhVien.TrangThai == EUser.Active)
            {
                // model.TenTrangThai = "Đang kích Hoạt";
                thanhVien.TrangThai = EUser.DeActive;

            }
            else if (thanhVien.TrangThai == EUser.DeActive)
            {
                // model.TenTrangThai = "Đã bị khóa";
                thanhVien.TrangThai = EUser.Active;
            }
            bool result = _ThanhVienLogic.Update(thanhVien);
            if (result == true)
            {
                TempData["Success"] = "Xóa thành công";
                return RedirectToAction("Index", "GiaoVien");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }

        public ActionResult Details(string idUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThanhVien thanhVien = _ThanhVienLogic.GetById(idUser);
            if (thanhVien == null)
                return RedirectToAction("NotFound", "Error");
            UserViewModel model = new UserViewModel()
            {
                // thông tin cho phép cập nhật
                Id = thanhVien.Id,
                Ten = thanhVien.Ten,
                DiaChi = thanhVien.DiaChi,
                SDT = thanhVien.SDT,
                GioiTinh = thanhVien.GioiTinh,
                NgaySinh = thanhVien.NgaySinh,
                LopHoc = thanhVien.LopHoc,
                NienKhoa = thanhVien.NienKhoa,

                // thông tin không được thay đổi 
                UserName = thanhVien.UserName,
                Password = thanhVien.Password,
                MaSoThanhVien = thanhVien.MaSoThanhVien,
                TrangThai = thanhVien.TrangThai,
                QRLink = thanhVien.QRLink,

            };
            if (thanhVien.HinhChanDung == null)
                model.LinkAvatar = @"/Content/Images/Default.jpg";
            else
                model.LinkAvatar = thanhVien.HinhChanDung;
            return View(model);
        }

        public ActionResult ChangePassword(string idUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThanhVien thanhVien = _ThanhVienLogic.GetById(idUser);
            if (thanhVien == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.Id = idUser;
            ViewBag.Error = TempData["Error"];
            ViewBag.Sussces = TempData["Sussces"];
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            ThanhVien thanhVien = _ThanhVienLogic.GetById(model.Id);
            if (thanhVien.Password != null)
            {
                if (thanhVien.Password.Equals(model.OldPassword) == true)
                {
                    if (model.NewPassword == null)
                    {
                        return View(model);
                    }
                    else
                    {
                        if (model.NewPassword.Equals(model.ConfirmPassword) == true)
                        {
                            TempData["Sussces"] = "Đổi mật khẩu thành công!";
                            return RedirectToAction("ChangePassword", "GiaoVien", new { @idUser = model.Id });
                        }
                        TempData["Error"] = "Nhập lại mật khẩu không khớp nhau!";
                        return RedirectToAction("ChangePassword", "GiaoVien", new { @idUser = model.Id });
                    }
                }
                TempData["Error"] = "Mật khẩu hiện tại không đúng!";
                return RedirectToAction("ChangePassword", "GiaoVien", new { @idUser = model.Id });
            }
            //  ViewBag.UnSussces= TempData["UnSussces"] = "Đổi mật khẩu không thành công!";
            return View(model);
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromExcel(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            ExcelManager excelManager = new ExcelManager();
            List<ThanhVien> list = new List<ThanhVien>();
            if (model.LinkExcel != null)
            {
                string physicalWebRootPath = Server.MapPath("/");
                // Todo Excel
                list = thanhVienCommon.ImportFromExcel(physicalWebRootPath, model.LinkExcel);
                int i = 0;
                foreach (var item in list)
                {
                    // ktr trùng mã số thành viên
                    var thanhVien = _ThanhVienLogic.GetByMaSoThanhVien(item.MaSoThanhVien);
                    if (thanhVien == null)
                    {
                        // Thêm thành viên,lưu mã vạch                        
                        var id = _ThanhVienLogic.Insert(item); //insert toàn bộ,chưa ktra gv hs
                        ThanhVien tv = _ThanhVienLogic.GetById(id);
                        ThanhVien temp = new ThanhVien();
                        temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null);
                        if (temp != null)
                        {
                            tv.QRLink = temp.QRLink;
                            tv.QRData = temp.QRData;
                            _ThanhVienLogic.Update(tv);
                        }
                    }
                    else
                    {
                        ViewBag.Duplicate = "Mã thành viên bị trùng ở dòng số " + (item.RowExcel + i).ToString();
                        return View();
                    }
                    i++;
                }
            }
            return RedirectToAction("Index", "GiaoVien");
        }

        public ActionResult ExportWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportWord(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            ExcelManager excelManager = new ExcelManager();
            var listTV = _ThanhVienLogic.GetAllGV();
            string fileName = "MauTheGV.docx";
            if (model.LinkWord != null)
            {
                string uploadForder = GetUploadFolder(Helpers.UploadFolder.FileWord);
                string physicalWebRootPath = Server.MapPath("/");

                var sourceFileName = Path.Combine(physicalWebRootPath, uploadForder, model.LinkWord.FileName);

                string location = Path.GetDirectoryName(sourceFileName);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Create))
                {
                    model.LinkWord.InputStream.CopyTo(fileStream);
                    var sourceDir = fileStream.Name.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                    fileStream.Close();
                    excelManager.ExportWord(sourceDir, listTV, fileName);
                    // To do Download           
                    string filepath = @"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileWord\" + fileName;
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                    string contentType = MimeMapping.GetMimeMapping(filepath);
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,
                        Inline = true,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());
                    return File(filedata, contentType);
                }
            }
            // return View();
            return RedirectToAction("Index", "GiaoVien");
        }

        public ActionResult MauThe(string mauThe)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            if (mauThe == null)
                return RedirectToAction("NotFound", "Error");
            ExcelManager excelManager = new ExcelManager();
            var listTV = _ThanhVienLogic.GetAllGV();
            string linkMau = null;
            //DateTime today = DateTime.Today;
            //string fileName = "MauTheGV ("+ today.Day.ToString() + "-" + today.Month.ToString() + "-"+today.Year.ToString() + ")"+".docx";
            string fileName = "MauTheGV.docx";
            if (mauThe.Equals("mau1") == true)
            {
                linkMau = "/Content/MauWord/Mau1.docx";
                fileName = "MauTheGV-Mau1.docx";
            }
            else if ((mauThe.Equals("mau2") == true))
            {
                linkMau = "/Content/MauWord/Mau2.docx";
                fileName = "MauTheGV-Mau2.docx";
            }
            excelManager.ExportWord(linkMau, listTV, fileName);

            // To do Download           
            string filepath = @"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileWord\" + fileName;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }

        #region Step By Step Excel

        //public IActionResult Import()
        //{
        //    ViewBag.Title = "Tạo tài khoản từ tệp nguồn";

        //    #region Xóa file vừa tạo lần trước (nếu có)
        //    var previousExcel = HttpContext.Session.GetString("ExelFileName");
        //    if (false == string.IsNullOrEmpty(previousExcel))
        //    {
        //        var serverFilePath = Path.Combine(HostingEnvironment.WebRootPath, previousExcel);
        //        System.IO.File.Delete(serverFilePath);
        //    }
        //    #endregion

        //    #region Bảo mật
        //    #region Kiểm tra đăng nhập
        //    var loggedUserId = HttpContext.Session.GetString("UserId");
        //    if (string.IsNullOrEmpty(loggedUserId)) return RedirectToAction("Login", "User");
        //    #endregion

        //    var userData = UserLogic.GetById(loggedUserId);

        //    // Chỉ cho phép tài khoản quản lý trường được truy cập
        //    if (userData.RoleSystemName != "SchoolManager")
        //    {
        //        return RedirectToAction("PermissionNotAllowed", "Home");
        //    }
        //    #endregion

        //    // Kiểm tra quyền truy cập
        //    if (UserLogic.HasPermission(userData.Id, "AddChildUser") == false)
        //    {
        //        return RedirectToAction("PermissionNotAllowed", "Home");
        //    }

        //    var viewModel = new ImportUserviewModel();

        //    // Lấy thông tin trường.
        //    var grantedSchool = UserLogic.GetGrantedSchool(userData.Id);
        //    if (grantedSchool != null)
        //    {
        //        viewModel.SchoolName = grantedSchool.Name;

        //        // Nếu đã có thông tin trường thì lấy luôn thông tin của Phòng
        //        var department = new EntityRepository<Department>(Database.DepartmentTableName).GetById(grantedSchool.DepartmentId);
        //        if (department != null)
        //        {
        //            viewModel.DepartmentName = department.Name;
        //        }
        //    }

        //    if (grantedSchool != null)
        //    {
        //        // Lấy danh sách khối lớp theo trường của Quản lý trường
        //        var dbGradeList = new GradeLogic().GetByGrade(grantedSchool.Grade);
        //        foreach (var item in dbGradeList)
        //        {
        //            // Lấy danh sách môn học của Khối lớp này
        //            var dbSubjectList = SubjectLogic.GetByGradeId(item.Id);
        //            viewModel.GradeList.Add(new GradeViewModel
        //            {
        //                Id = item.Id,
        //                Name = item.kl_TenKhoiLop,
        //                SubjectList = dbSubjectList
        //            });
        //        }
        //    }


        //    viewModel.InsitutionName = new SettingLogic().GetByKey("InsitutionName");
        //    viewModel.PermissionList = PermissionLogic.GetChildPermission(userData.RoleSystemName);

        //    return View(viewModel);
        //}

        // Đọc dữ liệu file chứa tài khoản đã được upload

        [HttpPost]
        public async Task<ActionResult> PreviewImport(HttpPostedFileBase file)
        {
            if (file != null)
            {
                // Chỉ chấp nhận file *.xls, *.xlsx
                if (Path.GetExtension(file.FileName).EndsWith(".xls") || Path.GetExtension(file.FileName).EndsWith(".xlsx"))
                {
                    var viewModel = new ImportResultViewModel();
                    // Đường dẫn để lưu nội dung file Excel
                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.FileExcel);
                    string uploadFileName = null;
                    string physicalWebRootPath = Server.MapPath("/");
                    uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, file.FileName);
                    string location = Path.GetDirectoryName(uploadFileName);
                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }
                    // Ghi nội dung file Excel vào tệp tạm
                    using (var fileStream = new FileStream(uploadFileName, FileMode.Create))
                    {
                        // Lưu                
                        file.InputStream.CopyTo(fileStream);
                        string sourceSavePath = uploadFileName;
                        Workbook workBook = new Workbook(sourceSavePath);
                        Worksheet workSheet = workBook.Worksheets[0];
                        int firstRow = workSheet.Cells.FirstCell.Row + 1;
                        int firstColumn = workSheet.Cells.FirstCell.Column;
                        // Số dòng tối đa
                        var maxRows = workSheet.Cells.MaxDataRow - workSheet.Cells.MinDataRow;
                        // Số cột tối đa
                        var maxColumns = (workSheet.Cells.MaxDataColumn + 1) - workSheet.Cells.MinDataColumn;
                        //
                        viewModel.RawDataList = new List<string[]>();
                        // Đọc từng dòng trong Excel
                        for (int rowIndex = firstRow; rowIndex <= firstRow + maxRows; rowIndex++)
                        {
                            // Xác định dòng dữ liệu này có bị trống dữ liệu CẢ DÒNG hay không.
                            var isEmptyRow = true;
                            // Tạo từng dòng thông tin
                            var rowData = new string[maxColumns];
                            // Lấy nội dung từng cột dữ liệu trong hàng hiện tại.
                            for (int columnIndex = firstColumn; columnIndex <= firstColumn + maxColumns; columnIndex++)
                            {
                                // Đọc nội dung ô
                                var cellData = (workSheet.Cells[rowIndex, columnIndex]).Value?.ToString() ?? "";
                                if (false == string.IsNullOrEmpty(cellData))
                                {
                                    // Lấy nội dung của Ô, lưu vào bộ nhớ
                                    rowData[columnIndex - firstColumn] = cellData;
                                    // Xác định Row hiện tại không bị trống dữ liệu
                                    isEmptyRow = false;
                                }
                            }
                            #region Nếu dòng không trống thì thêm vào danh sách đã quét.
                            if (isEmptyRow == false)
                            {
                                viewModel.RawDataList.Add(rowData);
                            }
                            #endregion                            
                        }
                        workBook.Dispose();

                        // Realse
                        //releaseObject(workSheet);
                        // releaseObject(workBook);
                        // releaseObject(xlApp);
                    }
                    // Xóa file đã lưu tạm
                    System.IO.File.Delete(uploadFileName);                    
                    viewModel.TotalEntry = viewModel.RawDataList.Count;
                    return View(viewModel);
                }
                else
                {
                    return Json(new { status = "fail", message = "Tập tin không đúng định dạng của Excel, vui lòng kiểm tra lại" });
                }
            }

            return Json(new { status = "fail", message = "Quá trình Upload bị gián đoạn. Vui lòng thữ lại" });
        }

        public ActionResult RequestEditPreviewForm(string[] data, string orderNumber)
        {
            ViewBag.OrderNumber = orderNumber;
            return PartialView("_EditPreviewForm", data);
        }

        [HttpPost]
        public ActionResult ImportSave(List<string[]> data, string columnOptions)
        {
            return View();
        }
        //    #region  Lấy thông tin người dùng
        //    var userdata = GetUserData();
        //    if (userdata == null)
        //        return RedirectToAction("LogOff", "Account");
        //    var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    #endregion

        //    #region xxx
        //    ImportResultViewModel model = new ImportResultViewModel();
        //    var school = _ThanhVienLogic.GetAllGV();//new SchoolLogic().GetById(userData.SchoolId);
        //    // Chuyển dữ liệu từ JSON thành POCO List
        //    var entries = new List<ThanhVien>();
        //    var optionsSegments = columnOptions.Split('|');

        //    foreach (var item in data)
        //    {
        //        var jObject = new JObject();
        //        for (int i = 1; i <= optionsSegments.Length - 1; i++)
        //        {
        //            var columnData = item[i - 1];

        //            #region Parse dữ liệu cột Quyền chức năng
        //            if (optionsSegments[i].ToString() == "GrantedPermissionList")
        //            {
        //                var permissionArray = columnData.Split('/');

        //                string[] SystemNamePermissionList = new string[permissionArray.Length];
        //                for (int k = 0; k <= permissionArray.Length - 1; k++)
        //                {
        //                    // Loại bỏ các khoảng trống
        //                    permissionArray[k] = permissionArray[k].Trim();

        //                    // Loại bỏ dấu tiếng việt
        //                    permissionArray[k] = ConvertToUnsign1(permissionArray[k]).ToLower();

        //                    switch (permissionArray[k])
        //                    {
        //                        case "them":
        //                            SystemNamePermissionList[k] = "SubmitQuestion";
        //                            break;
        //                        case "xem":
        //                            SystemNamePermissionList[k] = "ViewOtherQuestion";
        //                            break;
        //                        case "xoa":
        //                            SystemNamePermissionList[k] = "DeleteQuestion";
        //                            break;
        //                        case "sua":
        //                            SystemNamePermissionList[k] = "EditQuestion";
        //                            break;
        //                        case "duyet":
        //                            SystemNamePermissionList[k] = "VarifyQuestion";
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                }

        //                jObject[optionsSegments[i]] = string.Join("|", SystemNamePermissionList);
        //                continue;
        //            }
        //            #endregion

        //            #region Parse dữ liệu cột Môn giảng dạy
        //            if (optionsSegments[i].ToString() == "GrantedGradeAndSubjectId")
        //            {
        //                var dataString = "";
        //                // Lấy danh sách khối lớp của Người tạo tài khoản
        //                var creatorGradeList = new GradeLogic().GetByGrade(school.Grade);
        //                #region Lấy danh sách môn học của từng khối lớp
        //                var gradeListWithSubject = new List<ViewModels.User.GradeViewModel>();
        //                foreach (var grade in creatorGradeList)
        //                {
        //                    var subjectList = SubjectLogic.GetByGradeId(grade.Id);
        //                    var gradeViewModel = new GradeViewModel
        //                    {
        //                        Id = grade.Id,
        //                        Name = grade.kl_TenKhoiLop,
        //                        SubjectList = subjectList
        //                    };

        //                    gradeListWithSubject.Add(gradeViewModel);
        //                }
        //                #endregion
        //                var dataInArray = columnData.Split('/');

        //                foreach (var element in dataInArray)
        //                {
        //                    var elementInArray = element.Split('.');

        //                    if (elementInArray.Length == 2)
        //                    {
        //                        var gradeName = elementInArray[0].ToString();
        //                        var subjectOrderNumber = int.Parse(elementInArray[1].ToString());

        //                        #region Lấy mã của khối lớp đã dc khai báo.
        //                        var grade = gradeListWithSubject.Where(_ => _.Name.IndexOf(gradeName) > -1).FirstOrDefault();
        //                        if (grade != null)
        //                        {
        //                            var subjectPositionInArray = subjectOrderNumber - 1;
        //                            // Ngăn trường hợp Index out of array
        //                            if (subjectPositionInArray > grade.SubjectList.Count)
        //                            {
        //                                continue;
        //                            }
        //                            var subject = grade.SubjectList[subjectPositionInArray];
        //                            if (subject == null)
        //                            {
        //                                continue;
        //                            }

        //                            dataString += "|" + grade.Id + "_" + subject.Id;
        //                        }
        //                        #endregion
        //                    }
        //                }

        //                jObject[optionsSegments[i]] = dataString;
        //                continue;
        //            }
        //            #endregion

        //            #region Parse dữ liệu cột quyền quản lý
        //            if (optionsSegments[i].ToString() == "RoleSystemName")
        //            {
        //                if (string.IsNullOrEmpty(columnData))
        //                {
        //                    jObject["IsSubjectHead"] = false;
        //                }
        //                else
        //                {
        //                    jObject["IsSubjectHead"] = true;
        //                }
        //                continue;
        //            }
        //            #endregion

        //            jObject[optionsSegments[i]] = columnData;
        //        }

        //        var pocoEntry = JsonConvert.DeserializeObject<ImportResultViewModel.Entry>(jObject.ToString());
        //        entries.Add(pocoEntry);
        //    }

        //    var viewModels = new ImportResultViewModel();
        //    var hasError = false;

        //    if (entries != null)
        //    {
        //        // Các Permission mặc định được gán cho tài khoản giáo viên
        //        var defaultPermissionList = new List<string>() { "ViewOtherQuestion", "SubmitQuestion", "DeleteQuestion", "EditQuestion", "VarifyQuestion" };

        //        // Lưu những số điện thoại đã được scan để xem có số nào trong file data bị trùng không
        //        var scannedPhoneNumber = new List<string>();

        //        foreach (var item in entries)
        //        {
        //            // Nếu không có set mật khẩu mặc định thì lấy SĐT làm mật khẩu
        //            var userPassword = "";
        //            if (string.IsNullOrEmpty(item.Password))
        //            {
        //                userPassword = Guid.NewGuid().ToString().Substring(0, 8);
        //            }
        //            else
        //            {
        //                userPassword = item.Password;
        //            }

        //            if (false == string.IsNullOrEmpty(item.PhoneNumber))
        //            {
        //                item.PhoneNumber = Regex.Replace(item.PhoneNumber, @"[^\d]", ""); ;
        //            }

        //            // Nếu không có Username thì lấy sdt làm username.
        //            var username = item.Username;
        //            if (string.IsNullOrEmpty(username))
        //            {
        //                username = item.PhoneNumber;
        //            }

        //            var viewModelEntry = new ImportResultViewModel.Entry
        //            {
        //                FullName = item.FullName,
        //                PhoneNumber = item.PhoneNumber,
        //                Username = username,
        //                Password = userPassword,
        //                RoleSystemName = (item.IsSubjectHead) ? "SubjectHead" : "Teacher",
        //                SchoolId = userData.SchoolId,
        //                DepartmentId = userData.DepartmentId,
        //                GrantedGradeAndSubjectId = item.GrantedGradeAndSubjectId,
        //                GrantedPermissionList = item.GrantedPermissionList,
        //                Email = item.Email,

        //            };

        //            // Kiểm tra có trùng SDT trong DB hay k
        //            if (UserLogic.IsPhoneNumberAvailable(item.PhoneNumber) == false)
        //            {
        //                viewModelEntry.IsDuplicationWithDatabase = true;

        //                // Ghi nhận tài khoản không hợp lệ
        //                hasError = true;
        //            }

        //            // Kiểm tra có trùng SDT trong file Excel hay k
        //            if (scannedPhoneNumber.Contains(item.PhoneNumber))
        //            {
        //                viewModelEntry.IsDuplicationWithFile = true;

        //                // Ghi nhận tài khoản không hợp lệ
        //                hasError = true;
        //            }

        //            // Kiểm tra địa chỉ email có hợp lệ k 
        //            if (IsValidEmail(viewModelEntry.Email) == false)
        //            {
        //                viewModelEntry.IsInvalidEmailAddress = hasError = true;
        //            }

        //            // Kiểm tra USERNAME có hợp lệ k
        //            if (IsValidUsername(viewModelEntry.Username) == false)
        //            {
        //                viewModelEntry.IsInvalidUsername = hasError = true;
        //            }

        //            viewModels.Entries.Add(viewModelEntry);

        //            scannedPhoneNumber.Add(item.PhoneNumber);
        //            viewModels.TotalValidEntry += 1;
        //        }

        //        // Nếu không có lỗi Validate gì thì tiến hành lưu vào Database luôn
        //        if (hasError == false)
        //        {
        //            foreach (var item in viewModels.Entries)
        //            {
        //                if (string.IsNullOrEmpty(item.GrantedGradeAndSubjectId))
        //                {
        //                    continue;
        //                }

        //                var GrantedGradeAndSubjectIdInArray = item.GrantedGradeAndSubjectId.Split('|');
        //                GrantedGradeAndSubjectIdInArray = GrantedGradeAndSubjectIdInArray.Skip(1).ToArray();

        //                // Lưu User vào danh sách tạm
        //                var user = new Models.User
        //                {
        //                    FullName = item.FullName,
        //                    PhoneNumber = item.PhoneNumber,
        //                    Username = item.Username,
        //                    Password = item.Password,
        //                    Email = item.Email,
        //                    RoleSystemName = item.RoleSystemName,
        //                    SchoolId = userData.SchoolId,
        //                    DepartmentId = userData.DepartmentId,
        //                    GrantedGradeAndSubjectId = GrantedGradeAndSubjectIdInArray
        //                };

        //                // Lưu vào CSDL
        //                UserLogic.Insert(user);

        //                if (item.GrantedPermissionList != null)
        //                {
        //                    foreach (var permission in item.GrantedPermissionList.Split('|'))
        //                    {
        //                        // Tránh bị edit permission trên GUI
        //                        if (defaultPermissionList.Contains(permission))
        //                        {
        //                            new EntityRepository<GrantedPermission>(Database.GrantedPermissionTableName).Insert(new GrantedPermission
        //                            {
        //                                UserId = user.Id,
        //                                PermissionSystemName = permission
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    ViewBag.HasError = hasError;
        //    ViewBag.ColumnOptions = columnOptions;

        //    #region Tạo file excel
        //    var xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //    Worksheet ws = (Worksheet)wb.Worksheets[1];

        //    // Tên bảng
        //    ws.Cells[1, 1] = "Danh sách tài khoản";
        //    ws.Range[ws.Cells[1, 1], ws.Cells[1, 6]].Merge();

        //    var range = (Range)ws.get_Range("A1", System.Type.Missing);
        //    range.EntireColumn.AutoFit();
        //    range.Font.Size = 18;
        //    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //    // Tên cột
        //    ws.Cells[2, 1] = "STT";
        //    ((Range)ws.UsedRange[2, 1]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    ws.Cells[2, 2] = "Họ tên";
        //    ((Range)ws.UsedRange[2, 2]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    ws.Cells[2, 3] = "Tài khoản";
        //    ((Range)ws.UsedRange[2, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    ws.Cells[2, 4] = "Mật khẩu";
        //    ((Range)ws.UsedRange[2, 4]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    ws.Cells[2, 5] = "Số điện thoại";
        //    ((Range)ws.UsedRange[2, 5]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    ws.Cells[2, 6] = "Email";
        //    ((Range)ws.UsedRange[2, 6]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


        //    var position = 3;
        //    var orderNumber = 1;

        //    foreach (var user in viewModels.Entries)
        //    {
        //        // Số thứ tự
        //        ws.Cells[position, 1] = orderNumber;
        //        ((Range)ws.UsedRange[position, 1]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 1]).EntireColumn.ColumnWidth = 10;
        //        ((Range)ws.UsedRange[position, 1]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //        ws.Cells[position, 2] = user.FullName;
        //        ((Range)ws.UsedRange[position, 2]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 2]).EntireColumn.ColumnWidth = 30;

        //        ws.Cells[position, 3] = user.Username;
        //        ((Range)ws.UsedRange[position, 3]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
        //        ((Range)ws.UsedRange[position, 3]).EntireColumn.ColumnWidth = 30;

        //        ws.Cells[position, 4] = user.Password;
        //        ((Range)ws.UsedRange[position, 4]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 4]).EntireColumn.ColumnWidth = 30;

        //        ws.Cells[position, 5] = user.PhoneNumber;
        //        ((Range)ws.UsedRange[position, 5]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 5]).EntireColumn.ColumnWidth = 30;
        //        ((Range)ws.UsedRange[position, 5]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //        ws.Cells[position, 6] = user.Email;
        //        ((Range)ws.UsedRange[position, 6]).ClearFormats();
        //        ((Range)ws.UsedRange[position, 6]).EntireColumn.ColumnWidth = 30;

        //        position++;
        //        orderNumber++;
        //    }

        //    // Độ rộng cột Excel fit vừa nội dung

        //    viewModels.ExcelFileName = Guid.NewGuid() + ".xlsx";

        //    var serverFilePath = Path.Combine(HostingEnvironment.WebRootPath, viewModels.ExcelFileName);

        //    wb.SaveCopyAs(serverFilePath);

        //    releaseObject(ws);
        //    releaseObject(wb);
        //    releaseObject(xlApp);
        //    #endregion

        //    HttpContext.Session.SetString("ExelFileName", serverFilePath);

        //    return View(viewModels);
        //    #endregion
        //}
        #endregion
    }

}