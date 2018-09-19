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
using Aspose.Cells;
using System.Threading.Tasks;
using System.Collections;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class HocSinhController : BaseController
    {
        ThanhVienCommon thanhVienCommon;
        XuLyChuoi xuLyChuoi;
        public HocSinhController()
        {
            xuLyChuoi = new XuLyChuoi();
            thanhVienCommon = new ThanhVienCommon();
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
        }

        // GET: User
        public ActionResult Index()
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            UserViewModel model = new UserViewModel();
            List<ThanhVien> listThanhVien = _ThanhVienLogic.GetAllHS();
            int i = 0;
            model.ListAll = new string[listThanhVien.Count * 2];
            foreach (var item in listThanhVien)
            {
                model.ListAll[i] = item.Ten;
                model.ListAll[i + 1] = item.MaSoThanhVien;
                i += 2;
            }
            model.ListThanhVien = listThanhVien;
            ViewBag.ThongBao = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<ThanhVien> listAll = _ThanhVienLogic.GetAllHS();
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
                ViewBag.SearchFail = "Chưa tìm được kết quả phù hợp!";
                ViewBag.ThongBao = true;
            }
            return View(model);
        }

        public PartialViewResult _PartialUser(int? page, string IdUser, List<ThanhVien> list)
        {
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            return PartialView(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _CreateUser()
        {
            UserViewModel model = new UserViewModel();
            model.TemptNgaySinh = "--/--/----";
            return View(model);
        }

        [HttpPost]
        public ActionResult _CreateUser(UserViewModel viewModel)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            BarCodeQRManager barcode = new BarCodeQRManager();
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
                LoaiTK = "hs"
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
                try
                {
                    // Lưu hình chân dung     
                    if (viewModel.HinhChanDung != null)
                    {
                        temp = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, tv, null, viewModel.HinhChanDung, _SubDomain);
                        tv.HinhChanDung = temp.HinhChanDung;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ImageUnsuccess = "Lưu hình chân dung thất bại" + ex.Message;
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                try
                {
                    // Lưu mã vạch
                    temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null, _SubDomain);
                    if (temp != null)
                    {
                        tv.QRLink = temp.QRLink;
                        tv.QRData = temp.QRData;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.QRUnsuccess = "Tạo mã QR thất bại" + ex.Message;
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
            return RedirectToAction("Index", "HocSinh");
        }
        //Get
        public ActionResult _Edit(string id)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
            ViewBag.HinhChanDung = tv.HinhChanDung;
            return View(model);
        }

        [HttpPost]
        public ActionResult _Edit(EditUserViewModel viewModel)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var thanhVien = _ThanhVienLogic.GetById(viewModel.Id);
            // thông tin cho phép cập nhật            
            thanhVien.LopHoc = viewModel.LopHoc;
            thanhVien.NienKhoa = viewModel.NienKhoa;
            thanhVien.Ten = viewModel.Ten;
            thanhVien.DiaChi = viewModel.DiaChi;
            thanhVien.GioiTinh = viewModel.GioiTinh;
            thanhVien.NgaySinh = viewModel.NgaySinh;
            thanhVien.SDT = viewModel.SDT;
            viewModel.LinkAvatar = thanhVien.HinhChanDung;


            //viewModel.LinkAvatar = "";
            //if (viewModel.HinhChanDung != null && Tool.IsImage(viewModel.HinhChanDung))
            //{
            //    viewModel.LinkAvatar = viewModel.HinhChanDung.FileName;
            //}
            if (viewModel.HinhChanDung != null)
            {
                try
                {
                    // Cập nhật Hình Chân Dung
                    string physicalWebRootPath = Server.MapPath("/");
                    string imageName = null;
                    if (thanhVien.HinhChanDung != null)
                        imageName = thanhVien.HinhChanDung.Replace(@"/Upload/AvatarUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");

                    ThanhVien tempt = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, thanhVien, imageName, viewModel.HinhChanDung, _SubDomain);
                    if (tempt != null)
                    {
                        viewModel.LinkAvatar = thanhVien.HinhChanDung = tempt.HinhChanDung;
                    }
                }
                catch { }
            }

            try
            {
                // cập nhật QR
                string physicalWebRootPath = Server.MapPath("/");
                //string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);
                string imageName = null;
                if (thanhVien.QRLink != null)
                    imageName = thanhVien.QRLink.Replace(@"/Upload/QRCodeUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                ThanhVien temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, thanhVien, imageName, _SubDomain);
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
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
                return RedirectToAction("Index", "HocSinh");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }

        // Ajax mở khóa tk
        public JsonResult ActiveMember(string id)
        {
            //#region  Lấy thông tin người dùng
            //var userdata = GetUserData();
            //if (userdata == null)
            //    return Json(new
            //    {
            //        data = "Unauthorize",
            //        status = false
            //    }, JsonRequestBehavior.AllowGet);
            //#endregion

            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ThanhVien thanhVien = _ThanhVienLogic.GetById(id);
            if (thanhVien == null)
            {
                return Json(new
                {
                    data = "Member is not found",
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            if (thanhVien.TrangThai == EUser.DeActive)
            {
                // model.TenTrangThai = "Đã bị khóa";
                thanhVien.TrangThai = EUser.Active;
                bool result = _ThanhVienLogic.Update(thanhVien);
                if (result == true)
                {
                    return Json(new
                    {
                        data = "Member account is activated",
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    data = "Fail to active member account",
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = "Member account is activated",
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        // Ajax khóa tk
        public JsonResult DeactiveMember(string id)
        {
            //#region  Lấy thông tin người dùng
            //var userdata = GetUserData();
            //if (userdata == null)
            //    return Json(new
            //    {
            //        data = "Unauthorize",
            //        status = false
            //    }, JsonRequestBehavior.AllowGet);
            //#endregion

            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ThanhVien thanhVien = _ThanhVienLogic.GetById(id);
            if (thanhVien == null)
            {
                return Json(new
                {
                    data = "Member is not found",
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            if (thanhVien.TrangThai == EUser.Active)
            {
                // model.TenTrangThai = "Đã bị khóa";
                thanhVien.TrangThai = EUser.DeActive;
                bool result = _ThanhVienLogic.Update(thanhVien);
                if (result == true)
                {
                    return Json(new
                    {
                        data = "Member account is deactivated",
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    data = "Fail to deactive member account",
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = "Member account is deactivated",
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string idUser)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
                LinkAvatar = thanhVien.HinhChanDung
            };
            return View(model);
        }

        public ActionResult ChangePassword(string idUser)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ThanhVien thanhVien = _ThanhVienLogic.GetById(model.Id);
            if (thanhVien.Password != null)
            {
                if (thanhVien.Password.Equals(model.OldPassword) == true)
                {
                    if (model.NewPassword.Equals(model.ConfirmPassword) == true)
                    {
                        thanhVien.Password = model.NewPassword;
                        if (_ThanhVienLogic.Update(thanhVien))
                        {
                            TempData["Sussces"] = "Đổi mật khẩu thành công!";
                            return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
                        }
                    }
                    TempData["Error"] = "Nhập lại mật khẩu không khớp nhau!";
                    return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
                }
                TempData["Error"] = "Mật khẩu hiện tại không đúng!";
                return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
            }
            return View(model);
        }

        public ActionResult ExportWord(string idTV)
        {
            ViewBag.IdTV = idTV;
            return View();
        }

        [HttpPost]
        public ActionResult ExportWord(UserViewModel model)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinThuVienLogic _thongTinThuVienLogic = new ThongTinThuVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<string> lstHeader = new List<string>();
            lstHeader.Add(_thongTinThuVienLogic.GetTheHeader1());//tên Phòng, sở
            lstHeader.Add(_thongTinThuVienLogic.GetTheHeader2());//tên trường
            ExcelManager excelManager = new ExcelManager();
            var listTV = _ThanhVienLogic.GetAllHS();
            string fileName = "MauTheHS.docx";
            if (model.LinkWord != null)
            {
                string physicalWebRootPath = Server.MapPath("/");
                string uploadForder = GetUploadFolder(UploadFolder.FileWord, _SubDomain);
                
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

                    excelManager.ExportWord(sourceDir, listTV, fileName, lstHeader);

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
            return RedirectToAction("Index", "HocSinh");
        }

        public ActionResult MauThe(string mauThe, string idTV)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinThuVienLogic _thongTinThuVienLogic = new ThongTinThuVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<string> lstHeader = new List<string>();
            lstHeader.Add(_thongTinThuVienLogic.GetTheHeader1());//tên Phòng, sở
            lstHeader.Add(_thongTinThuVienLogic.GetTheHeader2());//tên trường
            if (mauThe == null)
                return RedirectToAction("NotFound", "Error");
            ExcelManager excelManager = new ExcelManager();

            List<ThanhVien> listTV = new List<ThanhVien>();
            if (string.IsNullOrEmpty(idTV))
                listTV = _ThanhVienLogic.GetAllHS();
            else
            {
                var tv = _ThanhVienLogic.GetByMaSoThanhVien(idTV);
                listTV.Add(tv);
            }
            string linkMau = null;
            string fileName = "MauTheHS.docx";
            if (mauThe.Equals("mau1") == true)
            {
                linkMau = "/Content/MauWord/Mau1-HS.docx";
                fileName = "MauTheHS-Mau1.docx";
            }
            else if ((mauThe.Equals("mau2") == true))
            {
                linkMau = "/Content/MauWord/Mau2-HS.docx";
                fileName = "MauTheHS-Mau2.docx";
            }
            excelManager.ExportWord(linkMau, listTV, fileName, lstHeader);
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

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PreviewImport(HttpPostedFileBase file)
        {
            if (file != null)
            {
                // Chỉ chấp nhận file *.xls, *.xlsx
                if (Path.GetExtension(file.FileName).EndsWith(".xls") || Path.GetExtension(file.FileName).EndsWith(".xlsx"))
                {
                    var viewModel = new ImportExcelTVViewModel();
                    // Đường dẫn để lưu nội dung file Excel
                    string uploadFolder = GetUploadFolder(UploadFolder.FileExcel, _SubDomain);
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
                        // Số dòng, đầu tiên chứ dữ liệu
                        int firstRow = workSheet.Cells.FirstCell.Row + 1;
                        int firstColumn = workSheet.Cells.FirstCell.Column;
                        // Số dòng, cột tối đa
                        var maxRows = workSheet.Cells.MaxDataRow - workSheet.Cells.MinDataRow;
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

        [HttpPost]
        public ActionResult ImportSave(List<string[]> data)
        {
            var _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var listAllHS = new List<ThanhVien>();
            List<ThanhVien> ListFail = new List<ThanhVien>();
            List<ThanhVien> ListSuccess = new List<ThanhVien>();
            List<ArrayList> ListShow = new List<ArrayList>();
            var model = new ImportExcelTVViewModel();
            foreach (var item in data)
            {
                ThanhVien tv = new ThanhVien
                {
                    Ten = item[1].ToString().Trim(),
                    UserName = item[2].ToString().Trim(),
                    MaSoThanhVien = item[3].ToString().Trim(),
                    GioiTinh = item[4].ToString().Trim(),
                    LopHoc = item[6].ToString().Trim(),
                    NienKhoa = item[7].ToString().Trim(),
                    DiaChi = item[8].ToString().Trim(),
                    SDT = item[9].ToString().Trim(),
                    LoaiTK = "hs"
                };
                #region NgaySinh
                if (!String.IsNullOrEmpty(item[5].ToString().Trim()))
                {
                    string day = item[5].ToString().Replace('/', '-').Replace('\\', '-');
                    string[] arr = day.Split('-');
                    string ngay = arr[0];
                    string thang = arr[1];
                    string nam = arr[2];
                   
                    nam = nam.Substring(0, 4);// Trường hợp có định dạng datetime, 2018 00:00:00    
                    if (ngay.Length == 1)
                    {
                        char firstChar = ngay[0];
                        if (firstChar != 0)
                        {
                            ngay = "0" + arr[0];
                        }
                    }
                    if (thang.Length == 1)
                    {
                        char firstChar = thang[0];
                        if (firstChar != 0)
                        {
                            thang = "0" + arr[1];
                        }
                    }
                    if (nam.Length == 4)
                    {
                        day = ngay + "-" + thang + "-" + nam;
                        DateTime ngaySinh = DateTime.ParseExact(day, "dd-MM-yyyy", null);
                        tv.NgaySinh = ngaySinh;
                    }
                }
                #endregion
                listAllHS.Add(tv);
            }

            if (listAllHS != null)
            {
                foreach (var item in listAllHS)
                {
                    // Tên
                    if (String.IsNullOrEmpty(item.Ten.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Họ và tên\"");
                    }
                    // UserName
                    if (String.IsNullOrEmpty(item.UserName.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Tên User\"");
                    }
                    // GioiTinh
                    if (String.IsNullOrEmpty(item.GioiTinh.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Giới tính\"");
                    }
                    // MaSoThanhVien
                    if (String.IsNullOrEmpty(item.MaSoThanhVien.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Mã học sinh\"");
                    }
                    // NgaySinh
                    if (String.IsNullOrEmpty(item.NgaySinh.ToShortDateString().Trim()) || item.NgaySinh.ToShortDateString().Equals("01/01/0001") == true)
                    {
                        item.ListError.Add("Rỗng ô nhập \"Ngày sinh\"");
                    }
                    // Trùng mã                
                    var tv = _ThanhVienLogic.GetByMaSoThanhVien(item.MaSoThanhVien.Trim());
                    if (tv != null)
                    {
                        item.ListError.Add(" Bị trùng \"Mã học sinh\"");
                        item.IsDuplicate = true;
                    }
                    //
                    if (item.ListError.Count == 0)
                        ListSuccess.Add(item);
                    else
                        ListFail.Add(item);
                }
                #region Lưu vào CSDL ds Thành Viên không bị lỗi  
                if (ListSuccess.Count > 0)
                {
                    foreach (var item in ListSuccess)
                    {
                        var thanhVien = new ThanhVien
                        {
                            Ten = xuLyChuoi.ChuanHoaChuoi(item.Ten),
                            UserName = item.UserName,
                            MaSoThanhVien = item.MaSoThanhVien,
                            LoaiTK = item.LoaiTK,
                            GioiTinh = xuLyChuoi.ChuanHoaChuoi(item.GioiTinh),
                            NgaySinh = item.NgaySinh,
                            NienKhoa = item.NienKhoa,
                            LopHoc=item.LopHoc,                            
                            DiaChi = item.DiaChi,
                            SDT = item.SDT,
                            Password = item.MaSoThanhVien,
                            TrangThai = EUser.Active
                        };
                        // Thêm thành viên,lưu mã vạch  
                        var id = _ThanhVienLogic.Insert(thanhVien);
                        ThanhVien tv = _ThanhVienLogic.GetById(id);
                        ThanhVien temp = new ThanhVien();
                        string physicalWebRootPath = Server.MapPath("/");
                        temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null, _SubDomain);
                        if (temp != null)
                        {
                            tv.QRLink = temp.QRLink;
                            tv.QRData = temp.QRData;
                            _ThanhVienLogic.Update(tv);
                        }
                    }
                }
                #endregion
                #region Tạo file excel cho ds Thành Viên bị lỗi   

                if (ListFail.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    // Tên header
                    Style style = new Style();
                    style.Pattern = BackgroundType.Solid;
                    style.ForegroundColor = System.Drawing.Color.FromArgb(139, 195, 234);
                    style.Font.Size = 20;
                    style.Font.IsBold = true;
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Style styleData = new Style();
                    styleData.Font.Size = 18;
                    styleData.Font.Name = "Times New Roman";
                    styleData.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Style styleError = new Style();
                    styleError.Pattern = BackgroundType.Solid;
                    styleError.ForegroundColor = System.Drawing.Color.LightPink;
                    styleError.Font.Size = 18;
                    styleError.Font.Name = "Times New Roman";
                    styleError.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["A1"].SetStyle(style);
                    ws.Cells["B1"].PutValue("Họ và tên");
                    ws.Cells["B1"].SetStyle(style);
                    ws.Cells["C1"].PutValue("Tên User");
                    ws.Cells["C1"].SetStyle(style);
                    ws.Cells["D1"].PutValue("Mã học sinh");
                    ws.Cells["D1"].SetStyle(style);
                    ws.Cells["E1"].PutValue("Giới tính");
                    ws.Cells["E1"].SetStyle(style);
                    ws.Cells["F1"].PutValue("Ngày sinh");
                    ws.Cells["F1"].SetStyle(style);
                    ws.Cells["G1"].PutValue("Lớp học");
                    ws.Cells["G1"].SetStyle(style);
                    ws.Cells["H1"].PutValue("Niên khóa");
                    ws.Cells["H1"].SetStyle(style);
                    ws.Cells["I1"].PutValue("Địa chỉ");
                    ws.Cells["I1"].SetStyle(style);
                    ws.Cells["J1"].PutValue("SĐT");
                    ws.Cells["J1"].SetStyle(style);
                    // ws.Cells["N4"].PutValue("Lý do");
                    // Import data             
                    int firstRow = 1;
                    int firstColumn = 0;
                    int stt = 1;
                    model.ArrRows = new bool[ListFail.Count + 1];
                    foreach (var item in ListFail)
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add(stt);
                        arrList.Add(item.Ten);
                        arrList.Add(item.UserName);
                        arrList.Add(item.MaSoThanhVien);
                        arrList.Add(item.GioiTinh);
                        if (item.NgaySinh.ToShortDateString().Equals("01/01/0001"))
                            arrList.Add("");
                        else
                            arrList.Add(item.NgaySinh.ToShortDateString());
                        arrList.Add(item.LopHoc);
                        arrList.Add(item.NienKhoa);
                        arrList.Add(item.DiaChi);
                        arrList.Add(item.SDT);
                        string errorExcel = null;
                        bool isFirst = true;// xét dấu phẩy cho chuỗi thông báo
                        foreach (var err in item.ListError)
                        {
                            if (isFirst)
                                errorExcel += err;
                            else
                                errorExcel += ", " + err;
                            isFirst = false;
                        }
                        ws.Cells.ImportArrayList(arrList, firstRow, firstColumn, false);
                        // Set style màu sắc
                        for (int i = firstColumn; i < firstColumn + 10; i++)
                        {
                            ws.Cells[firstRow, i].SetStyle(styleData);
                        }
                        if (String.IsNullOrEmpty(item.Ten.Trim()))
                            ws.Cells[firstRow, firstColumn + 1].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.UserName.Trim()))
                            ws.Cells[firstRow, firstColumn + 2].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.MaSoThanhVien.Trim()))
                            ws.Cells[firstRow, firstColumn + 3].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.GioiTinh.Trim()))
                            ws.Cells[firstRow, firstColumn + 4].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.NgaySinh.ToString().Trim()) ||
                            item.NgaySinh.ToShortDateString().Equals("01/01/0001") == true)
                            ws.Cells[firstRow, firstColumn + 5].SetStyle(styleError);
                        model.ArrRows[firstRow] = false;// khởi tạo dòng False (không bị trùng)
                        if (item.IsDuplicate == true)
                        {
                            ws.Cells[firstRow, firstColumn + 3].SetStyle(styleError);
                            model.ArrRows[firstRow] = true;
                        }
                        // K lưu vào file Excel, chỉ xuất lên table
                        arrList.Add(errorExcel);
                        ListShow.Add(arrList);
                        firstRow++;
                        stt++;
                    }
                    ws.AutoFitColumns();
                    // Save
                    string fileName = "DsHocSinhBiLoi.xlsx";
                    string physicalWebRootPath = Server.MapPath("/");
                    string uploadFolder = GetUploadFolder(UploadFolder.FileExcel, _SubDomain);
                    string uploadFileName = null;
                    uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, fileName);
                    string location = Path.GetDirectoryName(uploadFileName);
                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }
                    wb.Save(uploadFileName, SaveFormat.Xlsx);
                    model.FileName = fileName;
                    model.FilePath = uploadFileName;
                }
                #endregion
            }
            model.ListSuccess = ListSuccess;
            model.ListFail = ListFail;
            model.ListShow = ListShow;
            return View(model);
        }

        public ActionResult DowloadExcel(string filePath, string fileName)
        {
            if (fileName == null)
                return RedirectToAction("NotFound", "Error");
            // To do Download                       
            string filepath = filePath;
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
}