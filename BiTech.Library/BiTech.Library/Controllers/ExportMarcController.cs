using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Common;
using BiTech.Library.DAL.CommonConstants;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using BiTech.Library.Controllers.BaseClass;
using MARC4J.Net;
using MARC4J.Net.MARC;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class ExportMarcController : BaseController
    {

        // GET: ExportMarc
        public ActionResult Index(KeySearchViewModel KeySearch, int? page)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (Session[CommonConstants.Session] == null)//nếu null mới được khởi tạo
                Session[CommonConstants.Session] = new List<IDSach>();
            ViewBag.container = (List<IDSach>)Session[CommonConstants.Session];

            ListBooksModel model = new ListBooksModel();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

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
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_TacGia = tenTG;

                model.Books.Add(book);
            }
            //Session[CommonConstants.Session] = model;

            return View(model.Books.ToPagedList(pageNumber, pageSize));
        }
		
        [HttpGet]
        public ActionResult exportItemMarc()
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (Session[CommonConstants.Session] == null)//nếu null mới được khởi tạo
                Session[CommonConstants.Session] = new List<IDSach>();

            var ListSession = (List<IDSach>)Session[CommonConstants.Session];
            if (ListSession.Count() != 0)
            {
                string fileName = string.Concat("FileMarc_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".mrc");
                //var folderReport = "/Upload/filemarc";
                //string filePath = System.Web.HttpContext.Current.Server.MapPath(folderReport);

                var folderReport = Tool.GetUploadFolder(UploadFolder.FileMrc, _SubDomain);

                string physicalWebRootPath = Server.MapPath("~/");
                var filePath = Path.Combine(physicalWebRootPath, folderReport);

                //kiễm tra nếu chưa có thì tạo mới
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullPath = Path.Combine(filePath, fileName);

                using (var fs2 = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    using (var writer = new MarcStreamWriter(fs2, "UTF-8"))
                    {
                        foreach (var Id in ListSession)
                        {
                            var GetId = _SachLogic.GetById(Id.Id);
                            IRecord record = MarcFactory.Instance.NewRecord();
                            IDataField dataField = null;

                            record.AddVariableField(MarcFactory.Instance.NewControlField("001", (GetId.MaKiemSoat != null) ? GetId.MaKiemSoat : ""));

                            var getIdSachTG = _SachTacGiaLogic.getById(GetId.Id);
                            if (getIdSachTG != null)
                            {
                                var getByIdTG = _TacGiaLogic.GetById(getIdSachTG.IdTacGia);
                                if (getByIdTG != null)
                                {
                                    dataField = MarcFactory.Instance.NewDataField("100", '1', '#');
                                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getByIdTG.TenTacGia != null) ? getByIdTG.TenTacGia : ""));
                                    record.AddVariableField(dataField);
                                }
                                else
                                {
                                    dataField = MarcFactory.Instance.NewDataField("100", '1', '#');
                                    dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', " "));
                                    record.AddVariableField(dataField);
                                }
                            }


                            dataField = MarcFactory.Instance.NewDataField("020", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.ISBN != null) ? GetId.ISBN : " "));
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', (GetId.GiaBia != null) ? GetId.GiaBia : ""));
                            record.AddVariableField(dataField);

                            dataField = MarcFactory.Instance.NewDataField("041", '0', '#');
                            var getById = _LanguageLogic.GetById(GetId.IdNgonNgu);
                            if (getById != null)
                            {
                                dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getById.Ten != null) ? getById.Ten : ""));
                                record.AddVariableField(dataField);
                            }


                            dataField = MarcFactory.Instance.NewDataField("250", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.TaiBan != null) ? GetId.TaiBan : " "));
                            record.AddVariableField(dataField);

                            dataField = MarcFactory.Instance.NewDataField("300", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.SoTrang != null) ? GetId.SoTrang : ""));
                            record.AddVariableField(dataField);

                            dataField = MarcFactory.Instance.NewDataField("520", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.TomTat != null) ? GetId.TomTat : ""));
                            record.AddVariableField(dataField);

                            dataField = MarcFactory.Instance.NewDataField("245", '1', '0');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.TenSach != null) ? GetId.TenSach : ""));
                            record.AddVariableField(dataField);


                            dataField = MarcFactory.Instance.NewDataField("260", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', (GetId.NamXuatBan != null) ? GetId.NamXuatBan : ""));

                            var getNameNXB = _NhaXuatBanLogic.getById(GetId.IdNhaXuatBan);
                            //var GetNamNXB= _NhaXuatBanLogic.getById
                            if (getNameNXB != null)
                            {
                                dataField.AddSubfield(MarcFactory.Instance.NewSubfield('b', (getNameNXB.Ten != null) ? getNameNXB.Ten : ""));
                            }
                            record.AddVariableField(dataField);


                            if (GetId.DDC != null)
                            {
                                dataField = MarcFactory.Instance.NewDataField("082", '0', '4');
                                dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.DDC != null) ? GetId.DDC : ""));
                                record.AddVariableField(dataField);
                            }


                            dataField = MarcFactory.Instance.NewDataField("044", '#', '#');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.XuatXu != null) ? GetId.XuatXu : ""));
                            record.AddVariableField(dataField);

                            //dataField = MarcFactory.Instance.NewDataField("500", '#', '#');
                            //dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (GetId.TomTat != null) ? GetId.TomTat : ""));
                            //record.AddVariableField(dataField);


                            writer.Write(record);
                        }
                    }

                }

                string filename = fileName;
                string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderReport, filename);
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                System.IO.File.Delete(filepath);
					
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(filedata, contentType);
            }
            else
            {
                TempData["error"] = "addErrorBlock";

            }

            return RedirectToAction("Index", "ExportMarc");
        }


        //exportAllMarc
        [HttpGet]
        public ActionResult exportAllMarc(HttpRequestMessage request)
        {

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);


            var query = _SachLogic.getAll();
            string fileName = string.Concat("FileMarc_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".mrc");
            var folderReport = "/Reports";
            string fileUrl = $"{Request.Url.Scheme}://{Request.Url.Host}:64002/Reports/{fileName}";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            using (var fs2 = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (var writer = new MarcStreamWriter(fs2, "UTF-8"))
                {


                    foreach (var i in query)
                    {
                        IRecord record = MarcFactory.Instance.NewRecord();
                        IDataField dataField = null;

                        record.AddVariableField(MarcFactory.Instance.NewControlField("001", i.MaKiemSoat));

                        var getIdSachTG = _SachTacGiaLogic.getById(i.Id);
                        if (getIdSachTG != null)
                        {
                            var getByIdTG = _TacGiaLogic.GetById(getIdSachTG.IdTacGia);
                            if (getByIdTG != null)
                            {
                                dataField = MarcFactory.Instance.NewDataField("100", '1', '#');
                                dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getByIdTG.TenTacGia != null) ? getByIdTG.TenTacGia : ""));
                                record.AddVariableField(dataField);
                            }
                            else
                            {
                                dataField = MarcFactory.Instance.NewDataField("100", '1', '#');
                                dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', " "));
                                record.AddVariableField(dataField);
                            }
                        }


                        dataField = MarcFactory.Instance.NewDataField("020", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.ISBN != null) ? i.ISBN : " "));
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', (i.GiaBia != null) ? i.GiaBia : ""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("041", '0', '#');
                        var getById = _LanguageLogic.GetById(i.IdNgonNgu);
                        if (getById != null)
                        {
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getById.Ten != null) ? getById.Ten : ""));
                            record.AddVariableField(dataField);
                        }


                        dataField = MarcFactory.Instance.NewDataField("250", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.TaiBan != null) ? i.TaiBan : " "));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("300", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.SoTrang != null) ? i.SoTrang : ""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("520", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.TomTat != null) ? i.TomTat : ""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("245", '1', '0');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.TenSach != null) ? i.TenSach : ""));
                        record.AddVariableField(dataField);


                        //dataField = MarcFactory.Instance.NewDataField("260", '#', '#');
                        //dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', (i.NamSanXuat != null) ? i.NamSanXuat : ""));

                        var getNameNXB = _NhaXuatBanLogic.getById(i.IdNhaXuatBan);
                        //var GetNamNXB= _NhaXuatBanLogic.getById
                        if (getNameNXB != null)
                        {
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('b', (getNameNXB.Ten != null) ? getNameNXB.Ten : ""));
                        }
                        record.AddVariableField(dataField);


                        if (i.DDC != null)
                        {
                            dataField = MarcFactory.Instance.NewDataField("082", '0', '4');
                            dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.DDC != null) ? i.DDC : ""));
                            record.AddVariableField(dataField);
                        }


                        dataField = MarcFactory.Instance.NewDataField("044", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.XuatXu != null) ? i.XuatXu : ""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("500", '#', '#');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.TomTat != null) ? i.TomTat : ""));
                        record.AddVariableField(dataField);


                        writer.Write(record);
                    }

                }

                string filename = fileName;
                string filepath = AppDomain.CurrentDomain.BaseDirectory + folderReport + "/" + filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(filedata, contentType);
            }
            //return RedirectToAction("Index", "sach");
        }


        [HttpPost]
        public JsonResult AddList(string[] Id)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);


            if (Session[CommonConstants.Session] == null)//nếu null mới được khởi tạo
                Session[CommonConstants.Session] = new List<IDSach>();

            var container = (List<IDSach>)Session[CommonConstants.Session];
            //var list = _SachLogic.GetById(Id);
           for(var i=0;i<Id.Length;i++)
            {
                if (container == null)
                {
                    container = new List<IDSach>();
                }
                if (container.Any(x => x.Id == Id[i]))
                {
                    container.RemoveAll(x => x.Id == Id[i]);
                    Session[CommonConstants.Session] = container;
                }
                else
                {
                    IDSach newItem = new IDSach();
                    newItem.Id = Id[i];
                    container.Add(newItem);
                }
                Session[CommonConstants.Session] = container;       
            }
            return Json(new
            {
                status = true
            });

        }


        [HttpPost]
        public JsonResult DeleteItem(string[] Id)
        {
            if (Session[CommonConstants.Session] == null)//nếu null mới được khởi tạo
                Session[CommonConstants.Session] = new List<IDSach>();

            var container = (List<IDSach>)Session[CommonConstants.Session];
            for(int i=0;i<Id.Length;i++)
            {
                if (container != null)
                {
                    container.RemoveAll(x => x.Id == Id[i]);
                    Session[CommonConstants.Session] = container;
                }
            }
           
            return Json(new
            {
                status = true
            });
        }


        [HttpPost]
        public JsonResult LoadListSession()
        {
            if (Session[CommonConstants.Session] == null)//nếu null mới được khởi tạo
                Session[CommonConstants.Session] = new List<IDSach>();

            var container = (List<IDSach>)Session[CommonConstants.Session];
           

            return Json(new
            {
                data=container,
                status = true
            },JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.Session] = new List<IDSach>();
            return Json(new
            {
                status = true
            });
        }

    }
}