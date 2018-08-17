

using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using MARC4J.Net;
using MARC4J.Net.MARC;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class ExportMarcController : BaseController
    {
       
        // GET: ExportMarc
        public ActionResult Index()
        {
            return View();
        }
    
        [HttpGet]
        public ActionResult exportMarc(HttpRequestMessage request)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                            return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);


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
                        IDataField dataField=null;
                       
                        record.AddVariableField(MarcFactory.Instance.NewControlField("008", i.MaKiemSoat)); 

                        dataField = MarcFactory.Instance.NewDataField("245", '1', '0');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a',(i.TenSach!=null)?i.TenSach:""));
                        record.AddVariableField(dataField);

                        var getIdSachTG = _SachTacGiaLogic.getById(i.Id);
                        var getByIdTG = _TacGiaLogic.GetById(getIdSachTG.IdTacGia);

                        dataField = MarcFactory.Instance.NewDataField("100", '1', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getByIdTG.TenTacGia != null) ? getByIdTG.TenTacGia : ""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("020", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.ISBN!=null)?i.ISBN:""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("020", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('c', (i.GiaBia!=null)?i.GiaBia:""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("041", '0',' ');
                        var getById = _LanguageLogic.GetById(i.IdNgonNgu);
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (getById.TenNgan != null)? getById.TenNgan:""));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("250", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a',(i.TaiBan!=null)?i.TaiBan:" "));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("300", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.SoTrang!=null)?i.SoTrang:" "));
                        record.AddVariableField(dataField);

                        dataField = MarcFactory.Instance.NewDataField("520", ' ', ' ');
                        dataField.AddSubfield(MarcFactory.Instance.NewSubfield('a', (i.TomTat != null) ? i.TomTat : " "));
                        record.AddVariableField(dataField);

                        writer.Write(record);
                    }
                  
                }

                string filename = fileName;
                string filepath = AppDomain.CurrentDomain.BaseDirectory + folderReport+"/"+ filename;
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

    }
}