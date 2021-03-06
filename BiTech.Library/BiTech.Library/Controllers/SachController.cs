﻿using Aspose.Cells;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Common;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using MARC4J.Net.MARC;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static BiTech.Library.Helpers.Tool;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class SachController : BaseController
    {
        SachCommon sachCommon;
        XuLyChuoi xuLyChuoi;
        public SachController()
        {
            sachCommon = new SachCommon();
            xuLyChuoi = new XuLyChuoi();
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
        }
        public ActionResult Index(KeySearchViewModel KeySearch, int? page)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ListBooksModel model = new ListBooksModel();

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;

            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            ViewBag.theLoaiSach_selected = KeySearch.TheLoaiSach ?? " ";
            ViewBag.tacGia_selected = KeySearch.TenTacGia ?? " ";
            ViewBag.NXB_selected = KeySearch.TenNXB ?? " ";
            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";

            KeySearch.Keyword = sachCommon.GetInfo(KeySearch.Keyword);

            var list = _SachLogic.getPageSach(KeySearch);
            ViewBag.number = list.Count();

            foreach (var item in list)
            {
                var listTG = _SachTacGiaLogic.getListById(item.Id);

                string tenTG = "";
                foreach (var item2 in listTG)
                {
                    tenTG += _TacGiaLogic.GetByIdTG(item2.IdTacGia)?.TenTacGia + ", " ?? "";
                }
                tenTG = tenTG.Length == 0 ? "--" : tenTG.Substring(0, tenTG.Length - 2);

                // cập nhật model số lượng còn lại = sl còn lại - sl trong trạng thái không mượn được         
                //var numKhongMuonDuoc =  MuonSachController.GetSoLuongSach(item.Id, userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
                //item.SoLuongConLai = item.SoLuongConLai - numKhongMuonDuoc;

                BookView book = new BookView(item);
                book.TenSach = book.SachDTO.TenSach;
                book.MaKiemSoat = book.SachDTO.MaKiemSoat;
                book.CreateDateTime = book.SachDTO.CreateDateTime;
                book.NamXuatBan = book.SachDTO.NamXuatBan;
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = tenTG;

                model.Books.Add(book);
            }

            //Sắp xếp

            if (KeySearch.SapXep == "1")
                model.Books = model.Books.OrderBy(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "11")
                model.Books = model.Books.OrderByDescending(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "2")
                model.Books = model.Books.OrderBy(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "22")
                model.Books = model.Books.OrderByDescending(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "3")
                model.Books = model.Books.OrderBy(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "33")
                model.Books = model.Books.OrderByDescending(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "4")
                model.Books = model.Books.OrderBy(_ => _.NamXuatBan).ToList();
            if (KeySearch.SapXep == "44")
                model.Books = model.Books.OrderByDescending(_ => _.NamXuatBan).ToList();

            return View(model.Books.ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult _PartSlAndTT()
        {
            return PartialView();
        }

        public ActionResult Create()
        {
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TrangThaiSachLogic _trangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var modelTT = _trangThaiSachLogic.GetAll();
            ViewBag.TT = modelTT;

            //var idTG = _TacGiaLogic.GetAllTacGia();
            //ViewBag.IdTacGia = idTG;

            SachUploadModel model = new SachUploadModel();
            model.Languages = _LanguageLogic.GetAll();

            ViewBag.Message = TempData["ThemSachMsg"] = "";
            return View(model.SachDTO);
        }

        [HttpPost]
        public ActionResult Create(SachUploadModel model)
        {
            var a = ViewData["LstTTS"];
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ViewBag.Message = TempData["ThemSachMsg"] = "";
            TrangThaiSachLogic _trangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            if (ModelState.IsValid)
            {
                SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SachTheLoaiLogic _SachTheLoaiLogic = new SachTheLoaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

                if (model.SachDTO.IdBoSuuTap == null)
                {
                    var nameId = _BoSuuTapLogic.GetName("sach");
                    if (nameId != null)
                        model.SachDTO.IdBoSuuTap = nameId.Id;
                }

                var TenSachKhongDau = ConvertToUnSign.ConvertName(model.SachDTO.TenSach);
                model.SachDTO.TenSachKhongDau = TenSachKhongDau;
                string id = _SachLogic.ThemSach(model.SachDTO);

                if (id.Length > 0)
                {
                    string failTG = "";
                    foreach (var tg in model.ListTacGiaJson)
                    {
                        var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                        string tgId = "";

                        if (string.IsNullOrEmpty(item.Id))
                        {
                            tgId = _TacGiaLogic.Insert(new TacGia() { TenTacGia = item.TenTacGia, MoTa = "", QuocTich = "" });
                        }
                        else
                        {
                            tgId = _TacGiaLogic.GetById(item.Id)?.Id ?? "";
                        }

                        if (!string.IsNullOrEmpty(tgId))
                        {
                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                            {
                                IdSach = id,
                                IdTacGia = tgId
                            });
                        }
                        else
                        {
                            failTG += item.TenTacGia + ", ";
                        }
                    }

                    string failTL = "";
                    foreach (var tg in model.ListTheLoaiJson)
                    {
                        var item = JsonConvert.DeserializeObject<TheLoaiSachViewModels>(tg);
                        string tlId = "";

                        if (string.IsNullOrEmpty(item.Id))
                        {
                            tlId = _TheLoaiSachLogic.Insert(new TheLoaiSach() { TenTheLoai = item.TenTheLoai, MoTa = "" });
                        }
                        else
                        {
                            tlId = _TheLoaiSachLogic.getById(item.Id)?.Id ?? "";
                        }

                        if (!string.IsNullOrEmpty(tlId))
                        {
                            _SachTheLoaiLogic.ThemSachTheLoai(new SachTheLoai()
                            {
                                IdSach = id,
                                IdTheLoai = tlId
                            });
                        }
                        else
                        {
                            failTL += item.TenTheLoai + ", ";
                        }
                    }

                    #region nghia: convertSachMarcXml
                    var book = _SachLogic.GetById(id);
                    IRecord record = MarcFactory.Instance.NewRecord();
                    IDataField dataField = null;

                    record.AddVariableField(MarcFactory.Instance.NewControlField("001", book.MaKiemSoat));

                    dataField = MarcFactory.Instance.NewDataField("020", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.ISBN));
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', model.SachDTO.GiaBia));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("041", '0', '#');
                    var getById = _LanguageLogic.GetById(model.SachDTO.IdNgonNgu);
                    if (getById != null)
                    {
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', getById.Ten));
                        record.AddVariableField(dataField);
                    }

                    dataField = MarcFactory.Instance.NewDataField("250", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.TaiBan));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("300", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.SoTrang));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("520", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.TomTat));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("100", '1', '#');
                    var TenTG = "";
                    foreach (var tg in model.ListTacGiaJson)
                    {
                        var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                        if (tg.Count() < 1)
                        {
                            TenTG += item.TenTacGia;
                        }
                        else
                        {
                            TenTG += item.TenTacGia + ",";
                        }

                    }
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', TenTG.Trim(',')));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("245", '1', '0');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.TenSach));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("260", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', model.SachDTO.NamXuatBan));

                    var getNameNXB = _NhaXuatBanLogic.getById(model.SachDTO.IdNhaXuatBan);
                    //var GetNamNXB= _NhaXuatBanLogic.getById
                    if (getNameNXB != null)
                    {
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('b', (getNameNXB.Ten != null) ? getNameNXB.Ten : ""));
                    }
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("082", '0', '4');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.DDC));
                    record.AddVariableField(dataField);

                    dataField = MarcFactory.Instance.NewDataField("044", '#', '#');
                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', model.SachDTO.XuatXu));
                    record.AddVariableField(dataField);

                    var recordXml = record.GetDataFields();
                    var ControlField = record.GetControlFields();
                    for (int i = 0; i < ControlField.Count; i++)
                    {
                        book.MARC21 = "=" + ControlField[i].ToString();
                        book.MARC21 += "\n";
                    }

                    for (int i = 0; i < recordXml.Count; i++)
                    {
                        book.MARC21 += "=" + recordXml[i] + "  ";
                        book.MARC21 += "\n";
                    }
                    _SachLogic.Update(book);

                    #endregion

                    if (model.FileImageCover != null)
                    {
                        try
                        {
                            string physicalWebRootPath = Server.MapPath("~/");
                            string uploadFolder = Path.Combine(GetUploadFolder(UploadFolder.BookCovers, _SubDomain), id);
                            string uploadFolderThumbail = Path.Combine(GetUploadFolder(UploadFolder.BookCovers, _SubDomain), id);

                            var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, "cover" + Path.GetExtension(model.FileImageCover.FileName));
                            var ThumbnailName = Path.Combine(physicalWebRootPath, uploadFolder, "thumbnail" + Path.GetExtension(model.FileImageCover.FileName));

                            string location = Path.GetDirectoryName(uploadFileName);

                            if (!Directory.Exists(location))
                            {
                                Directory.CreateDirectory(location);
                            }

                            #region nghia
                            model.FileImageCover.SaveAs(uploadFileName);
                            int imageSize = 400;

                            var image = Image.FromFile(uploadFileName);

                            int thumbnailSize = imageSize;
                            int newWidth;
                            int newHeight;

                            if (image.Height < thumbnailSize && image.Width < thumbnailSize)
                            {
                                newWidth = image.Width;
                                newHeight = image.Height;
                            }
                            else
                            {
                                if (image.Width > image.Height)
                                {
                                    newWidth = thumbnailSize;
                                    newHeight = image.Height * thumbnailSize / image.Width;
                                }
                                else
                                {
                                    newWidth = image.Width * thumbnailSize / image.Height;
                                    newHeight = thumbnailSize;
                                }
                            }

                            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);

                            thumbnailGraph.DrawImage(image, imageRectangle);

                            thumbnailBitmap.Save(ThumbnailName, image.RawFormat);
                            thumbnailGraph.Dispose();
                            thumbnailBitmap.Dispose();
                            image.Dispose();//ngắt tiến trình giải phóng bộ nhớ
                            System.IO.File.Delete(uploadFileName);

                            book.LinkBiaSach = ThumbnailName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                            _SachLogic.Update(book);
                            #endregion
                        }
                        catch { }
                    }

                    // Lưu mã QR
                    Sach sach = _SachLogic.GetBookById(id);
                    try
                    {
                        string physicalWebRootPath = Server.MapPath("/");
                        Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sach, null, _SubDomain);
                        if (temp != null)
                        {
                            sach.QRlink = temp.QRlink;
                            sach.QRData = temp.QRData;
                            _SachLogic.Update(sach);
                        }
                    }
                    catch { }

                    if (failTG.Length > 0)
                    {
                        failTG = failTG.Substring(0, failTG.Length - 2);
                        TempData["ThemSachMsg"] = string.Format("Chú ý: Chọn tác giả {0} thất bại, vui lòng cập nhật sau.", failTG);
                    }

                    if (failTL.Length > 0)
                    {
                        failTL = failTL.Substring(0, failTL.Length - 2);
                        TempData["ThemSachMsg"] = string.Format("Chú ý: Chọn thể loại {0} thất bại, vui lòng cập nhật sau.", failTL);
                    }

                    //Tạo phiếu nhập - VINH
                    bool nhapSach = false;
                    if (model.ListTTSach != null)
                    {
                        foreach (var item in model.ListTTSach)
                        {
                            if (item.SoLuong > 0)
                            {
                                nhapSach = true;
                                break;
                            }
                        }
                        if (model.ListTTSach != null && nhapSach)
                        {
                            PhieuNhapSach pns = new PhieuNhapSach()
                            {
                                GhiChu = model.GhiChuPhieuNhap,
                                IdUserAdmin = _UserAccessInfo.Id,
                                UserName = _UserAccessInfo.UserName
                            };

                            string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns); //Insert phieu nhap

                            foreach (var item in model.ListTTSach)
                            {
                                if (item.SoLuong > 0)
                                {
                                    //Chi tiet phieu nhap
                                    ChiTietNhapSach ctns = new ChiTietNhapSach()
                                    {
                                        IdPhieuNhap = idPhieuNhap,
                                        IdSach = model.SachDTO.Id,
                                        SoLuong = item.SoLuong,
                                        CreateDateTime = DateTime.Now,
                                        IdTinhtrang = item.IdTrangThai,
                                    };
                                    _ChiTietNhapSachLogic.Insert(ctns);

                                    ////Insert vào bảng SachCaBiet từng cuốn một
                                    int STTHienTai = _SachLogic.GetByID_IsDeleteFalse(id).STTMaCB; //Lấy STTCB của đầu sách hiện tại
                                    SachCaBiet scb = new SachCaBiet()
                                    {
                                        IdSach = id,
                                        IdTrangThai = item.IdTrangThai,
                                        TenSach = _SachLogic.GetByID_IsDeleteFalse(id).TenSach,
                                    };
                                    for (int i = 0; i < item.SoLuong; i++)
                                    {
                                        scb.MaKSCB = sach.MaKiemSoat + "." + (++STTHienTai);
                                        _SachCaBietLogic.Insert(scb);
                                    }

                                    ///Lưu mã QR
                                    var lstSachCB = _SachCaBietLogic.GetListCaBietFromIdSach(id);
                                    try
                                    {
                                        string physicalWebRootPath = Server.MapPath("/");
                                        foreach (var itemS in lstSachCB)
                                        {
                                            SachCaBiet temp = sachCommon.LuuMaVachSach_SachCaBiet(physicalWebRootPath, itemS, null, _SubDomain);
                                            if (temp != null)
                                            {
                                                itemS.QRlink = temp.QRlink;
                                                itemS.QRData = temp.QRData;
                                                _SachCaBietLogic.Update(itemS);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    //Update bảng Sach 
                                    //Cập nhật STT Cá biệt cho mỗi đầu sách  
                                    var modelSach = _SachLogic.GetBook_NonDelete_ByMKS(sach.MaKiemSoat);
                                    modelSach.SoLuongTong++;
                                    modelSach.SoLuongConLai++;
                                    _SachLogic.Update(modelSach);
                                }
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                TempData["ThemSachMsg"] = "Thêm sách thất bại";
            }
            model.Languages = _LanguageLogic.GetAll();

            var modelTT = _trangThaiSachLogic.GetAll();
            ViewBag.TT = modelTT;
            return View(model);
        }

        public ActionResult CreateSuccess()
        {
            ViewBag.Message = TempData["ThemSachMsg"];

            return View();
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach sachDTO = _SachLogic.GetById(id);
            if (sachDTO == null)
            {
                return RedirectToAction("Index");
            }

            var sltts = _SachCaBietLogic.GetListCaBietFromIdSach(id);
            ViewBag.SlTTsach = sltts;

            var idTG = _TacGiaLogic.GetAllTacGia();
            ViewBag.IdTacGia = idTG;

            // cập nhật model số lượng còn lại = sl còn lại - sl trong trạng thái không mượn được
            //var numKhongMuonDuoc = MuonSachController.GetSoLuongSach(sachDTO.Id, userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
            //sachDTO.SoLuongConLai = sachDTO.SoLuongConLai - numKhongMuonDuoc;

            SachUploadModel model = new SachUploadModel(sachDTO);
            model.Languages = _LanguageLogic.GetAll();
            ViewBag.TLS = model.SachDTO.IdTheLoai;
            ViewBag.NXB = model.SachDTO.IdNhaXuatBan;
            ViewBag.KeSach = model.SachDTO.IdKeSach;
            ViewBag.NgonNgu = model.SachDTO.IdNgonNgu;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SachUploadModel model)
        {
            if (ModelState.IsValid)
            {
                SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SachTheLoaiLogic _SachTheLoaiLogic = new SachTheLoaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

                Sach sach = _SachLogic.GetBookById(model.SachDTO.Id);
                var TenSachKhongDau = ConvertToUnSign.ConvertName(model.SachDTO.TenSach);
                if (sach != null)
                {
                    sach.Id = model.SachDTO.Id;
                    sach.DDC = model.SachDTO.DDC;
                    sach.TenSach = model.SachDTO.TenSach;
                    sach.TenSachKhongDau = TenSachKhongDau;
                    sach.ISBN = model.SachDTO.ISBN;
                    sach.IdTheLoai = model.SachDTO.IdTheLoai;
                    sach.IdNhaXuatBan = model.SachDTO.IdNhaXuatBan;
                    sach.IdKeSach = model.SachDTO.IdKeSach;
                    sach.SoTrang = model.SachDTO.SoTrang;
                    sach.IdNgonNgu = model.SachDTO.IdNgonNgu;
                    sach.NamXuatBan = model.SachDTO.NamXuatBan;
                    sach.GiaBia = model.SachDTO.GiaBia;
                    sach.PhiMuonSach = model.SachDTO.PhiMuonSach;
                    sach.XuatXu = model.SachDTO.XuatXu;
                    sach.NguoiBienDich = model.SachDTO.NguoiBienDich;
                    sach.TaiBan = model.SachDTO.TaiBan;
                    sach.TaiLieuDinhKem = model.SachDTO.TaiLieuDinhKem;
                    sach.ISSN = model.SachDTO.ISSN;
                    sach.LLC = model.SachDTO.LLC;
                    sach.TomTat = model.SachDTO.TomTat;

                    string failTG = "";
                    if (_SachTacGiaLogic.DeleteAllTacGiaByidSach(sach.Id))
                    {
                        foreach (var tg in model.ListTacGiaJson)
                        {
                            var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                            string tgId = "";

                            if (!string.IsNullOrEmpty(item.TenTacGia))
                            {
                                TacGia tg_temp = _TacGiaLogic.GetByTenTacGia(item.TenTacGia);
                                if (tg_temp != null)
                                {
                                    tgId = tg_temp.Id;
                                }
                                else
                                {

                                    tgId = _TacGiaLogic.Insert(new TacGia() { TenTacGia = item.TenTacGia, MoTa = "", QuocTich = "" });
                                }
                            }

                            if (!string.IsNullOrEmpty(tgId))
                            {
                                _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                                {
                                    IdSach = sach.Id,
                                    IdTacGia = tgId
                                });
                            }
                            else
                            {
                                failTG += item.TenTacGia + ", ";
                            }
                        }
                    }

                    string failTL = "";
                    if (_SachTheLoaiLogic.DeleteAllTheLoaiByidSach(sach.Id))
                    {
                        foreach (var tg in model.ListTheLoaiJson)
                        {
                            var item = JsonConvert.DeserializeObject<TheLoaiSachViewModels>(tg);
                            string tgId = "";

                            if (!string.IsNullOrEmpty(item.TenTheLoai))
                            {
                                TheLoaiSach tg_temp = _TheLoaiSachLogic.GetByTenTheLoai(item.TenTheLoai);
                                if (tg_temp != null)
                                {
                                    tgId = tg_temp.Id;
                                }
                                else
                                {

                                    tgId = _TheLoaiSachLogic.Insert(new TheLoaiSach() { TenTheLoai = item.TenTheLoai, MoTa = "" });
                                }
                            }

                            if (!string.IsNullOrEmpty(tgId))
                            {
                                _SachTheLoaiLogic.ThemSachTheLoai(new SachTheLoai()
                                {
                                    IdSach = sach.Id,
                                    IdTheLoai = tgId
                                });
                            }
                            else
                            {
                                failTL += item.TenTheLoai + ", ";
                            }
                        }
                    }

                }



                if (model.FileImageCover != null)
                {
                    try
                    {
                        string physicalWebRootPath = Server.MapPath("~/");
                        string uploadFolder = Path.Combine(GetUploadFolder(UploadFolder.BookCovers, _SubDomain), model.SachDTO.Id);

                        var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, "cover" + Path.GetExtension(model.FileImageCover.FileName));
                        var ThumbnailName = Path.Combine(physicalWebRootPath, uploadFolder, "thumbnail" + Path.GetExtension(model.FileImageCover.FileName));

                        string location = Path.GetDirectoryName(uploadFileName);

                        if (!Directory.Exists(location))
                        {
                            Directory.CreateDirectory(location);
                        }

                        #region nghia
                        model.FileImageCover.SaveAs(uploadFileName);
                        int imageSize = 400;

                        var image = Image.FromFile(uploadFileName);

                        int thumbnailSize = imageSize;
                        int newWidth;
                        int newHeight;

                        if (image.Height < thumbnailSize && image.Width < thumbnailSize)
                        {
                            newWidth = image.Width;
                            newHeight = image.Height;
                        }
                        else
                        {
                            if (image.Width > image.Height)
                            {
                                newWidth = thumbnailSize;
                                newHeight = image.Height * thumbnailSize / image.Width;
                            }
                            else
                            {
                                newWidth = image.Width * thumbnailSize / image.Height;
                                newHeight = thumbnailSize;
                            }
                        }

                        var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);

                        thumbnailGraph.DrawImage(image, imageRectangle);

                        thumbnailBitmap.Save(ThumbnailName, image.RawFormat);
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();//ngắt tiến trình giải phóng bộ nhớ
                        System.IO.File.Delete(uploadFileName);
                        sach.LinkBiaSach = ThumbnailName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                        #endregion

                        //using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                        //{
                        //    model.FileImageCover.InputStream.CopyTo(fileStream);

                        //    //var book = _SachLogic.GetById(model.SachDTO.Id);
                        //    sach.LinkBiaSach = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");

                        //}
                    }
                    catch { }
                }

                #region Tai

                try
                {
                    // cập nhật QR
                    string physicalWebRootPath = Server.MapPath("/");
                    //string uploadFolder = GetUploadFolder(UploadFolder.QRCodeUser, _SubDomain);
                    string imageName = null;
                    if (sach.QRlink != null)
                        imageName = sach.QRlink.Replace(@"/Upload/QRCodeBook/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                    Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sach, imageName, _SubDomain);
                    if (temp != null)
                    {
                        sach.QRlink = temp.QRlink;
                        sach.QRData = temp.QRData;
                    }
                }
                catch { }

                #endregion

                _SachLogic.Update(sach);

                return RedirectToAction("Index");
            }

            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            model.Languages = _LanguageLogic.GetAll();
            ViewBag.TLS = model.SachDTO.IdTheLoai;
            ViewBag.NXB = model.SachDTO.IdNhaXuatBan;
            return View(model);
        }

        public JsonResult GetByFindId(string Id)
        {
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            JsonResult result = new JsonResult();
            result.Data = null;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var model = _SachCaBietLogic.GetListCaBietFromIdSach(Id);
            List<SoLuongTrangThaiSachVM> list = new List<SoLuongTrangThaiSachVM>();
            foreach (var i in model)
            {
                SoLuongTrangThaiSachVM vm = new SoLuongTrangThaiSachVM()
                {
                    Id = i.Id,
                    IdSach = i.IdSach,
                    IdTrangThai = i.IdTrangThai,
                    TrangThai = _TrangThaiSachLogic.getById(i.IdTrangThai).TenTT,
                };
                if (i.MaKSCB == "" && i.MaCaBienCu != "")
                    vm.MaCaBiet = i.MaCaBienCu;
                else
                    vm.MaCaBiet = i.MaKSCB;
                list.Add(vm);
            }
            result.Data = list;
            return result;
        }

        [HttpPost]
        public JsonResult EditSaveChange(SoLuongSachTrangThai vm, string txtIdttCategory)
        {            
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            bool update = false;
            if (!string.IsNullOrEmpty(vm.IdTrangThai))
            {
                var sachCaBiet = _SachCaBietLogic.GetIdSachFromMaCaBiet(vm.IdSach);
                sachCaBiet.IdTrangThai = vm.IdTrangThai;
                update = _SachCaBietLogic.Update(sachCaBiet);
            }            
            return Json(update, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load data to fix status
        /// </summary>
        /// <param name="Id">id bang sach ca biet</param>
        /// <returns></returns>
        public JsonResult GetById(string Id)
        {
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var model = _SachCaBietLogic.getById(Id);

            SoLuongTrangThaiSachVM vm = new SoLuongTrangThaiSachVM();
            vm = new SoLuongTrangThaiSachVM()
            {
                Id = model.Id,
                IdSach = model.IdSach,
                IdTrangThai = model.IdTrangThai,
                TrangThai = _TrangThaiSachLogic.getById(model.IdTrangThai).TenTT,
            };
            if (model.MaKSCB == "" && model.MaCaBienCu != "")
                vm.MaCaBiet = model.MaCaBienCu;
            else
                vm.MaCaBiet = model.MaKSCB;
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllTT(string id)
        {
            TrangThaiSachLogic _trangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var model = _trangThaiSachLogic.GetAllTT(id);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                DeleteByIdSach(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }

        }

        [HttpPost]
        public ActionResult DeleteForever(string id)
        {
            //SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            //Sach s = _SachLogic.GetById(id);
            // todo Xoa het ca hoi lien quan
            //_SachLogic.XoaSach(s.Id);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMany(KeySearchViewModel KeySearch, int? page)
        {

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ListBooksModel model = new ListBooksModel();

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.paged = page;
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;

            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            ViewBag.theLoaiSach_selected = KeySearch.TheLoaiSach ?? " ";
            ViewBag.tacGia_selected = KeySearch.TenTacGia ?? " ";
            ViewBag.NXB_selected = KeySearch.TenNXB ?? " ";
            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";

            var list = _SachLogic.getPageSach(KeySearch);
            ViewBag.number = list.Count();

            foreach (var item in list)
            {
                var listTG = _SachTacGiaLogic.getListById(item.Id);

                string tenTG = "";
                foreach (var item2 in listTG)
                {
                    tenTG += _TacGiaLogic.GetByIdTG(item2.IdTacGia)?.TenTacGia + ", " ?? "";
                }
                tenTG = tenTG.Length == 0 ? "--" : tenTG.Substring(0, tenTG.Length - 2);

                BookView book = new BookView(item);
                book.TenSach = book.SachDTO.TenSach;
                book.MaKiemSoat = book.SachDTO.MaKiemSoat;
                book.CreateDateTime = book.SachDTO.CreateDateTime;
                book.NamXuatBan = book.SachDTO.NamXuatBan;
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = tenTG;

                model.Books.Add(book);
            }

            //Sắp xếp
            if (KeySearch.SapXep == "1")
                model.Books = model.Books.OrderBy(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "11")
                model.Books = model.Books.OrderByDescending(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "2")
                model.Books = model.Books.OrderBy(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "22")
                model.Books = model.Books.OrderByDescending(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "3")
                model.Books = model.Books.OrderBy(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "33")
                model.Books = model.Books.OrderByDescending(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "4")
                model.Books = model.Books.OrderBy(_ => _.NamXuatBan).ToList();
            if (KeySearch.SapXep == "44")
                model.Books = model.Books.OrderByDescending(_ => _.NamXuatBan).ToList();

            return View(model.Books.ToPagedList(pageNumber, pageSize));


        }

        [HttpPost]
        public ActionResult DeleteMany(List<string> chon, string paging, string pageSize, string colRowCount)
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
                        DeleteByIdSach(item);
                    }
                }

                //return RedirectToAction("DeleteMany");
                return RedirectToAction("DeleteMany", "Sach", new
                {
                    page = paging
                });
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }

        }

        public JsonResult ListName(string q)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var data = _SachLogic.ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        // Popup PartialView

        /// <summary>
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemTheLoaiGui()
        {
            TheLoaiSachLogic _TheLoaiSachLogic =
                new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach(true);
            return PartialView("_NhapLoaiSach");
        }

        public ActionResult RequestThemTheKeSach()
        {
            return PartialView("_NhapkeSach");
        }

        public ActionResult RequestLanguage()
        {
            return PartialView("_Language");
        }

        /// <summary>
        /// Giao diện thêm nhà xuất bản
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemNhaXuatBan()
        {
            return PartialView("_NhapNhaXuatBan");
        }

        public ActionResult ThemTacGia()
        {
            return PartialView("_ThemTacGia");
        }

        #region Tai 
        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PreviewImport(HttpPostedFileBase file)
        {
            if (file != null)
            {
                // Chỉ chấp nhận file *.xls, *.xlsx
                if (Path.GetExtension(file.FileName).EndsWith(".xls") || Path.GetExtension(file.FileName).EndsWith(".xlsx"))
                {
                    var viewModel = new ImportExcelSachViewModel();
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
            #region Khai báo
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ///   PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            //   ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTheLoaiLogic _sachTheLoaiLogic = new SachTheLoaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var listAll = new List<Sach>();
            List<Sach> ListFail = new List<Sach>();
            List<Sach> ListSuccess = new List<Sach>();
            List<ArrayList> ListShow = new List<ArrayList>();
            var model = new ImportExcelSachViewModel();
            #endregion
            #region Truyền dữ liệu vào ListAll
            foreach (var item in data)
            {
                Sach sach = new Sach();
                sach.TenSach = item[1].ToString().Trim();
                sach.ISBN = item[2].ToString().Trim();

                var theLoai = item[3].ToString().Trim();
                if (!String.IsNullOrEmpty(theLoai))
                {
                    string[] tenTheLoai = theLoai.Split(new Char[] { ',', '.', '!', '\\', '/', ':', ';', '\n',
                        '_', '-', '>', '<', '^', '&', '+', '*', '~', '=','x' });
                    sach.ListTheLoai = new List<TheLoaiSach>();
                    foreach (var ten in tenTheLoai)
                    {
                        if (!String.IsNullOrEmpty(ten.Trim()))
                            sach.ListTheLoai.Add(new TheLoaiSach() { TenTheLoai = ten.Trim() });
                    }
                }
                sach.DDC = item[4].ToString().Trim();
                var tacGia = item[5].ToString().Trim();
                if (!String.IsNullOrEmpty(tacGia))
                {
                    string[] tenTacGia = tacGia.Split(new Char[] { ',', '.', '!', '\\', '/', ':', ';', '\n',
                        '_','-', '>', '<', '^', '&', '+', '*', '~', '=','x' });
                    sach.listTacGia = new List<TacGia>();
                    foreach (var ten in tenTacGia)
                    {
                        if (!String.IsNullOrEmpty(ten.Trim()))
                            sach.listTacGia.Add(new TacGia() { TenTacGia = ten.Trim() });
                    }
                }
                sach.IdNhaXuatBan = item[6].ToString().Trim();
                sach.IdKeSach = item[7].ToString().Trim();
                sach.SoTrang = item[8].ToString().Trim();
                sach.IdNgonNgu = item[9].ToString().Trim();
                sach.NamXuatBan = item[10].ToString().Trim();
                sach.GiaBia = item[11].ToString().Trim();
                sach.PhiMuonSach = item[12].ToString().Trim();
                sach.XuatXu = item[13].ToString().Trim();
                sach.NguoiBienDich = item[14].ToString().Trim();
                sach.TaiBan = item[15].ToString().Trim();
                sach.TaiLieuDinhKem = item[16].ToString().Trim();
                sach.TomTat = item[17].ToString().Trim();
                listAll.Add(sach);
            }
            #endregion
            #region Gán thông báo cho từng loại lỗi
            if (listAll != null)
            {
                foreach (var item in listAll)
                {
                    // Tên sách
                    if (String.IsNullOrEmpty(item.TenSach.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Tên sách\"");
                    }
                    // TheLoai
                    if (item.ListTheLoai.Count == 0)
                    {
                        item.ListError.Add("Rỗng ô nhập \"Thể loại sách\"");
                    }
                    // Mã DDC
                    if (!String.IsNullOrEmpty(item.DDC.Trim()))
                    {
                        var machs = System.Text.RegularExpressions.Regex.Match(item.DDC.Trim(), @"^\d{3}$");
                        if (machs.Length != 3 && machs != null)
                        {
                            item.ListError.Add("\"Mã DDC\" không hợp lệ");
                            item.ErrorDDC = true;
                        }
                    }
                    // TacGia
                    if (item.listTacGia.Count == 0)
                    {
                        item.ListError.Add("Rỗng ô nhập \"Tác giả\"");
                    }
                    // NhaXuatBan
                    if (String.IsNullOrEmpty(item.IdNhaXuatBan.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Nhà xuất bản\"");
                    }
                    // SoTrang
                    if (String.IsNullOrEmpty(item.SoTrang.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Số trang\"");
                    }
                    // NgonNgu
                    if (String.IsNullOrEmpty(item.IdNgonNgu.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Ngôn ngữ\"");
                    }
                    // NamXuatBan
                    if (String.IsNullOrEmpty(item.NamXuatBan.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Năm xuất bản\"");
                    }
                    // PhiMuonSach
                    if (String.IsNullOrEmpty(item.PhiMuonSach.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Phí mượn sách\"");
                    }
                    // TomTat
                    if (String.IsNullOrEmpty(item.TomTat.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Tóm tắt\"");
                    }
                    // Lưu vào CSDL Sách không bị lỗi 
                    if (item.ListError.Count == 0)
                    {
                        #region Lưu sách
                        #region Linh tinh                       
                        // Kệ sách
                        // string nameKS = sachCommon.ChuanHoaChuoi(item.IdKeSach);// Chuẩn hóa tên Kệ Sách
                        string nameKS = item.IdKeSach.Trim();
                        var keSach = _keSachLogic.GetByTenKeSach(nameKS);
                        if (keSach != null)
                        {
                            item.IdKeSach = keSach.Id;
                        }
                        else
                        {
                            var id = _keSachLogic.Add(new KeSach() { TenKe = nameKS });
                            item.IdKeSach = id;
                        }

                        // Nhà xuất bản
                        string nameNXB = xuLyChuoi.ChuanHoaChuoi(item.IdNhaXuatBan);// Chuẩn hóa tên Nhà Xuất Bản
                        var nxb = _NhaXuatBanLogic.GetByTenNXB(nameNXB);
                        if (nxb != null)
                        {
                            item.IdNhaXuatBan = nxb.Id;
                        }
                        else
                        {
                            var id = _NhaXuatBanLogic.ThemNXB(new NhaXuatBan() { Ten = nameNXB });
                            item.IdNhaXuatBan = id;
                        }

                        // Ngôn ngữ
                        string nameNN = xuLyChuoi.ChuanHoaChuoi(item.IdNgonNgu);// Chuẩn hóa tên Ngôn Ngữ
                        var ngonNgu = _LanguageLogic.GetByTenNgonNgu(nameNN);
                        if (ngonNgu != null)
                        {
                            item.IdNgonNgu = ngonNgu.Id;
                        }
                        else
                        {
                            var id = _LanguageLogic.InsertNew(new Language() { Ten = nameNN });
                            item.IdNgonNgu = id;
                        }
                        #endregion
                        // Thêm Sách  
                        var idSach = _SachLogic.ThemSach(item);
                        if (idSach.Length > 0)
                        {
                            // Lưu tác giả 
                            foreach (var tg in item.listTacGia)
                            {
                                tg.TenTacGia = xuLyChuoi.ChuanHoaChuoi(tg.TenTacGia);// Chuẩn hóa tên Tác Giả
                                string idTG = null;
                                var tacGia = _TacGiaLogic.GetByTenTacGia(tg.TenTacGia);
                                if (tacGia != null)
                                {
                                    idTG = tacGia.Id;
                                }
                                else
                                {
                                    // Thêm mới nếu tác giả không tồn tại
                                    idTG = _TacGiaLogic.Insert(tg);
                                }
                                _SachTacGiaLogic.ThemSachTacGia(new SachTacGia() { IdTacGia = idTG, IdSach = idSach });
                            }
                            // Lưu thể loại sách                                                                             
                            foreach (var tl in item.ListTheLoai)
                            {
                                tl.TenTheLoai = xuLyChuoi.ChuanHoaChuoi(tl.TenTheLoai);// Chuẩn hóa tên Thể Loại
                                string idTL = null;
                                var theLoaiSach = _TheLoaiSachLogic.GetByTenTheLoai(tl.TenTheLoai);
                                if (theLoaiSach != null)
                                {
                                    idTL = theLoaiSach.Id;
                                }
                                else
                                {
                                    // Thêm mới nếu thể loại sách không tồn tại
                                    idTL = _TheLoaiSachLogic.Insert(tl);
                                }
                                _sachTheLoaiLogic.ThemSachTheLoai(new SachTheLoai() { IdTheLoai = idTL, IdSach = idSach });
                            }
                            // Lưu mã vạch
                            string physicalWebRootPath = Server.MapPath("/");
                            Sach sachUpdate = _SachLogic.GetBookById(idSach);
                            Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sachUpdate, null, _SubDomain);
                            if (temp != null)
                            {
                                sachUpdate.QRlink = temp.QRlink;
                                sachUpdate.QRData = temp.QRData;
                                _SachLogic.Update(sachUpdate);
                            }
                        }
                        #endregion
                        #region Tạo Marc 21
                        var book = _SachLogic.GetById(idSach);
                        IRecord record = MarcFactory.Instance.NewRecord();
                        IDataField dataField = null;

                        record.AddVariableField(MarcFactory.Instance.NewControlField("001", book.MaKiemSoat));

                        dataField = MarcFactory.Instance.NewDataField("020", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.ISBN));
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', item.GiaBia));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("041", '0', '#');
                        var getById = _LanguageLogic.GetById(item.IdNgonNgu);
                        if (getById != null)
                        {
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', getById.Ten));
                            record.AddVariableField(dataField);
                        }

                        dataField = MarcFactory.Instance.NewDataField("250", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.TaiBan));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("300", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.SoTrang));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("520", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.TomTat));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("100", '1', '#');

                        var TenTG = "";
                        foreach (var tg in item.listTacGia)
                        {
                            TenTG += tg.TenTacGia + ", ";
                        }

                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', TenTG.Trim(',')));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("245", '1', '0');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.TenSach));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("260", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', item.NamXuatBan));

                        var getNameNXB = _NhaXuatBanLogic.getById(item.IdNhaXuatBan);
                        if (getNameNXB != null)
                        {
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('b', (getNameNXB.Ten != null) ? getNameNXB.Ten : ""));
                        }
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("082", '0', '4');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.DDC));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("044", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', item.XuatXu));
                        record.AddVariableField(dataField);

                        var recordXml = record.GetDataFields();
                        var ControlField = record.GetControlFields();
                        for (int i = 0; i < ControlField.Count; i++)
                        {
                            book.MARC21 = "=" + ControlField[i].ToString();
                            book.MARC21 += "\n";
                        }

                        for (int i = 0; i < recordXml.Count; i++)
                        {
                            book.MARC21 += "=" + recordXml[i] + "  ";
                            book.MARC21 += "\n";
                        }
                        _SachLogic.Update(book);
                        #endregion
                        ListSuccess.Add(item);
                    }
                    else
                        ListFail.Add(item);
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
                    ws.Cells["B1"].PutValue("Tên sách");
                    ws.Cells["B1"].SetStyle(style);
                    ws.Cells["C1"].PutValue("Mã ISBN");
                    ws.Cells["C1"].SetStyle(style);
                    ws.Cells["D1"].PutValue("Thể loại sách");
                    ws.Cells["D1"].SetStyle(style);
                    ws.Cells["E1"].PutValue("Mã DDC");
                    ws.Cells["E1"].SetStyle(style);
                    ws.Cells["F1"].PutValue("Tác giả");
                    ws.Cells["F1"].SetStyle(style);
                    ws.Cells["G1"].PutValue("Nhà xuất bản");
                    ws.Cells["G1"].SetStyle(style);
                    ws.Cells["H1"].PutValue("Kệ sách");
                    ws.Cells["H1"].SetStyle(style);
                    ws.Cells["I1"].PutValue("Số trang");
                    ws.Cells["I1"].SetStyle(style);
                    ws.Cells["J1"].PutValue("Ngôn ngữ");
                    ws.Cells["J1"].SetStyle(style);
                    ws.Cells["K1"].PutValue("Năm xuất bản");
                    ws.Cells["K1"].SetStyle(style);
                    ws.Cells["L1"].PutValue("Giá bìa");
                    ws.Cells["L1"].SetStyle(style);
                    ws.Cells["M1"].PutValue("Phí mượn");
                    ws.Cells["M1"].SetStyle(style);
                    ws.Cells["N1"].PutValue("Nước xuất xứ");
                    ws.Cells["N1"].SetStyle(style);
                    ws.Cells["O1"].PutValue("Người biên dịch");
                    ws.Cells["O1"].SetStyle(style);
                    ws.Cells["P1"].PutValue("Lần xuất bản");
                    ws.Cells["P1"].SetStyle(style);
                    ws.Cells["Q1"].PutValue("Tài liệu đính kèm");
                    ws.Cells["Q1"].SetStyle(style);
                    ws.Cells["R1"].PutValue("Tóm tắt");
                    ws.Cells["R1"].SetStyle(style);
                    // Import data             
                    int firstRow = 1;
                    int firstColumn = 0;
                    int stt = 1;
                    foreach (var item in ListFail)
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add(stt);
                        arrList.Add(item.TenSach);
                        arrList.Add(item.ISBN);
                        // Danh sách tên Thể Loại
                        string nameCategory = null;
                        foreach (var name in item.ListTheLoai)
                        {
                            nameCategory += name.TenTheLoai + ", ";
                        }
                        if (nameCategory != null && nameCategory.Equals(", ") == false)
                            arrList.Add(nameCategory);
                        else
                            arrList.Add("");
                        arrList.Add(item.DDC);
                        // Danh sách tên tác giả
                        string nameAuthor = null;
                        foreach (var name in item.listTacGia)
                        {
                            nameAuthor += name.TenTacGia + ", ";
                        }
                        if (nameAuthor != null && nameAuthor.Equals(", ") == false)
                            arrList.Add(nameAuthor);
                        else
                            arrList.Add("");
                        arrList.Add(item.IdNhaXuatBan);
                        arrList.Add(item.IdKeSach);
                        arrList.Add(item.SoTrang);
                        arrList.Add(item.IdNgonNgu);
                        arrList.Add(item.NamXuatBan);
                        arrList.Add(item.GiaBia);
                        arrList.Add(item.PhiMuonSach);
                        arrList.Add(item.XuatXu);
                        arrList.Add(item.NguoiBienDich);
                        arrList.Add(item.TaiBan);
                        arrList.Add(item.TaiLieuDinhKem);
                        arrList.Add(item.TomTat);
                        // Danh sách thông báo lỗi
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
                        for (int i = firstColumn; i < firstColumn + 18; i++)
                        {
                            ws.Cells[firstRow, i].SetStyle(styleData);
                        }
                        if (String.IsNullOrEmpty(item.TenSach.Trim()))
                            ws.Cells[firstRow, firstColumn + 1].SetStyle(styleError);

                        if (String.IsNullOrEmpty(nameCategory) || nameCategory.Equals(", "))
                            ws.Cells[firstRow, firstColumn + 3].SetStyle(styleError);

                        if (item.ErrorDDC == true)
                        {
                            ws.Cells[firstRow, firstColumn + 4].SetStyle(styleError);
                        }

                        if (String.IsNullOrEmpty(nameAuthor) || nameAuthor.Equals(", "))
                            ws.Cells[firstRow, firstColumn + 5].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.IdNhaXuatBan.Trim()))
                            ws.Cells[firstRow, firstColumn + 6].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.SoTrang.Trim()))
                            ws.Cells[firstRow, firstColumn + 8].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.IdNgonNgu.Trim()))
                            ws.Cells[firstRow, firstColumn + 9].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.NamXuatBan.Trim()))
                            ws.Cells[firstRow, firstColumn + 10].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.PhiMuonSach.Trim()))
                            ws.Cells[firstRow, firstColumn + 12].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.TomTat.Trim()))
                            ws.Cells[firstRow, firstColumn + 17].SetStyle(styleError);
                        // K lưu lý do lỗi vào file Excel, chỉ xuất lên table
                        item.Error = errorExcel;
                        arrList.Add(errorExcel);
                        ListShow.Add(arrList);
                        firstRow++;
                        stt++;
                    }
                    ws.AutoFitColumns();
                    // Save
                    string fileName = "DsSachBiLoi.xlsx";
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

        #endregion

        //- Xuất QR
        public ActionResult XuatQR()
        {
            var _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var lstIdSach = (List<IDSach>)TempData["lstMS"];
            var _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            string fileName = string.Concat("QR_Word" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".docx");

            var folderReport = Tool.GetUploadFolder(UploadFolder.ReportsWordQR, _SubDomain);
            
            string physicalWebRootPath = Server.MapPath("~/");

            string filePath = Path.Combine(physicalWebRootPath, folderReport); //System.Web.HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fullPath = Path.Combine(filePath, fileName);

            List<SachCaBiet> lstSachCB = new List<SachCaBiet>();
            foreach (var item in lstIdSach)
            {
                lstSachCB.AddRange(_SachCaBietLogic.GetListCaBietFromIdSach(item.Id));
            }
            if (lstSachCB.Count != 0)
            {
                string linkMau = null;
                linkMau = "/Content/MauWord/QRBook_Template.docx";
                if (string.IsNullOrEmpty(linkMau))
                {
                }
                ExcelManager wordExport = new ExcelManager();
                wordExport.ExportQRToWord(linkMau, lstSachCB, fullPath);
                byte[] filedata = System.IO.File.ReadAllBytes(fullPath);
                string contentType = MimeMapping.GetMimeMapping(fullPath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(filedata, contentType);
            }
            else
            {
                TempData["NonItem"] = "Đầu sách hiện chưa có sách trong kho!";
                return RedirectToAction("XuatQR_Mutil", "Sach");
            }
        }

        //- Thêm sách ajax
        public ActionResult ThemAjax(SachViewModels model)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach sach = new Sach()
            {
                TenSach = model.TenSach,
                IdTheLoai = model.IdTheLoai,
                IdNhaXuatBan = model.IdNhaXuatBan,
                SoTrang = model.SoTrang.ToString(),
                IdNgonNgu = model.IdNgonNgu,
                NamXuatBan = model.NamSanXuat,
                TomTat = model.TomTat,
                CreateDateTime = DateTime.Now,
            };
            string id = _SachLogic.ThemSach(sach);
            string failTG = "";
            foreach (var tg in model.ListTacGiaJson)
            {
                var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                string tgId = "";

                if (string.IsNullOrEmpty(item.Id))
                {
                    tgId = _TacGiaLogic.Insert(new TacGia() { TenTacGia = item.TenTacGia, MoTa = "", QuocTich = "" });
                }
                else
                {
                    tgId = _TacGiaLogic.GetById(item.Id)?.Id ?? "";
                }

                if (!string.IsNullOrEmpty(tgId))
                {
                    _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                    {
                        IdSach = id,
                        IdTacGia = tgId
                    });
                }
                else
                {
                    failTG += item.TenTacGia + ", ";
                }
            }

            return Json(true);
        }


        #region Phong
        /// <summary>
        /// Xoá sách bằng idSach
        /// </summary>
        /// <param name="idSach"></param>
        private void DeleteByIdSach(string idSach)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietXuatSachLogic _ChiTietXuatSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach s = _SachLogic.GetById(idSach);
            //GetList: thông tin mượn sách by idSach (1)
            List<ThongTinMuonSach> list_TTMS = _ThongTinMuonSachLogic.GetAllbyIdSach(s.Id);
            //GetList: chi tiết nhập sách by idSach (2)
            List<ChiTietNhapSach> list_CTNS = _ChiTietNhapSachLogic.GetAllChiTietByIdSach(s.Id);
            //GetList: chi tiết xuất sách by idSach (3)
            List<ChiTietXuatSach> list_CTXS = _ChiTietXuatSachLogic.GetAllChiTietByIdSach(s.Id);

            RemoveFileFromServer(s.LinkBiaSach);
            RemoveFileFromServer(s.QRlink, false);

            //if: (1)(2)(3) === 0 
            if ((list_TTMS.Count() + list_CTNS.Count() + list_CTXS.Count()) == 0)
            {
                //Xoá row table sách - xoá thật
                _SachLogic.XoaSach(s.Id);
            }
            else
            {
                //update IsDeleted = true
                s.IsDeleted = true;
                _SachLogic.Update(s);
            }


            //Xoá row table số lượng sách trạng thái by idSach
            _SoLuongSachTrangThaiLogic.DeleteByIdSach(s.Id);
            //Xoá row table sách tác giả by idSach
            _SachTacGiaLogic.DeleteAllTacGiaByidSach(s.Id);
        }

        /// <summary>
        /// Xoa image khoi server
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool RemoveFileFromServer(string path, bool isDeleteFolderParent = true)
        {
            var fullPath = Request.MapPath(path);
            if (!System.IO.File.Exists(fullPath)) return false;

            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(fullPath);
                if (isDeleteFolderParent)
                    DeleteFolderParent(fullPath);
                return true;
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Xoa folder chua file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool DeleteFolderParent(string path)
        {
            var pathFolder = Directory.GetParent(path).FullName;
            DirectoryInfo attachments_AR = new DirectoryInfo(pathFolder);
            EmptyFolder(attachments_AR);
            Directory.Delete(pathFolder);
            return false;
        }

        /// <summary>
        /// Xoa het cac file trong folder
        /// </summary>
        /// <param name="directory"></param>
        private void EmptyFolder(DirectoryInfo directory)
        {

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                EmptyFolder(subdirectory);
                subdirectory.Delete();
            }

        }



        #endregion

        #region Vinh Xuat QR sách - chọn nhiều
        public ActionResult XuatQR_Mutil(KeySearchViewModel KeySearch, int? page)
        {
            var nonItem = TempData["NonItem"];
            if (nonItem != null)
                ViewBag.NonItem = nonItem;
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            if (Session["CheckBook"] == null)
                Session["CheckBook"] = new List<IDSach>();
            ViewBag.container = (List<IDSach>)Session["CheckBook"];

            ListBooksModel model = new ListBooksModel();
            var list = new List<Sach>();
            list = _SachLogic.getPageSach(KeySearch);
            if (list.Count == 0)
            {
                list = _SachLogic.GetAll_NonDelete();
                ViewBag.SearchFail = "Chưa tìm được kết quả phù hợp!";
            }
            else
                ViewBag.SearchFail = "";
            //Kiem tra sach ca biet da co hay chua
            var lstTemp = new List<Sach>(list);
            foreach (var item in lstTemp)
            {
                var temp = _SachCaBietLogic.GetListCaBietFromIdSach(item.Id);
                if (temp.Count == 0)
                    list.Remove(item);
            }

            ViewBag.number = list.Count();

            foreach (var item in list)
            {
                var listTG = _SachTacGiaLogic.getListById(item.Id);

                string tenTG = "";
                foreach (var item2 in listTG)
                {
                    tenTG += _TacGiaLogic.GetByIdTG(item2.IdTacGia)?.TenTacGia + ", " ?? "";
                }
                tenTG = tenTG.Length == 0 ? "--" : tenTG.Substring(0, tenTG.Length - 2);

                BookView book = new BookView(item);
                book.Ten_TacGia = tenTG;

                model.Books.Add(book);
            }
            return View(model.Books.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult XuatQR_Mutil()
        {
            try
            {
                var lstUserChecked = (List<IDSach>)Session["CheckBook"];

                TempData["lstMS"] = lstUserChecked;
                if (lstUserChecked.Count != 0)
                    return RedirectToAction("XuatQR", "Sach");
                return RedirectToAction("XuatQR_Mutil", "Sach");
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        public JsonResult AddList(string Id)
        {
            if (Session["CheckBook"] == null)//nếu null mới được khởi tạo
                Session["CheckBook"] = new List<IDSach>();

            var container = (List<IDSach>)Session["CheckBook"];

            if (container == null)
            {
                container = new List<IDSach>();
            }
            if (!container.Any(x => x.Id == Id))
            {
                IDSach newItem = new IDSach();
                newItem.Id = Id;
                container.Add(newItem);
            }

            Session["CheckBook"] = container;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(string Id)
        {
            if (Session["CheckBook"] == null)//nếu null mới được khởi tạo
                Session["CheckBook"] = new List<IDSach>();

            var container = (List<IDSach>)Session["CheckBook"];
            if (container != null)
            {
                container.RemoveAll(x => x.Id == Id);
                Session["CheckBook"] = container;
            }
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session["CheckBook"] = new List<IDSach>();
            return Json(new
            {
                status = true
            });
        }
        #endregion
    }
}