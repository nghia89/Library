using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class QuanLyThuVienController : BaseController
    {
        BackupAndRestore brMng = new BackupAndRestore();

        // GET: QuanLyThuVien
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult ThongBao()
        {
            return View();
        }

        public ActionResult CSDL()
        {
            // .\CustomerBackup\<WebSubDomain>

            // create backup

            // angular list bk file

            // upload download file allow

            // choose file to restore

            // torse notification

            return View();
        }

        #region CSDL

        public JsonResult GetBackupFiles()
        {
            List<BackupFileModel> model = new List<BackupFileModel>();

            //string subdomain = Tool.GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var backupPath = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain));

            var lst = brMng.GetBackupFiles(backupPath).OrderByDescending(x => x.CreationTime);
            foreach (var i in lst)
            {
                model.Add(new BackupFileModel()
                {
                    Name = i.Name,
                    Date = i.CreationTime.ToString("dd/MM/yyyy - HH:mm:ss"),
                    Size = (i.Length * 1.0 / 1024).ToString("0") + " KB"
                });
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateBackupFile()
        {
            //string subdomain = Tool.GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var backupPath = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain));
            var uploadPath = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.Upload, _SubDomain));

            var cs = Tool.GetConfiguration("ConnectionString"); // mongodb://superadmin:password@127.0.0.1:27017/admin

            if (cs.StartsWith("mongodb://"))
            {
                cs = cs.Substring(10, cs.Length - 10);

                // Ghi nhận thông tin backup
                BackupRestoreInfo info = new BackupRestoreInfo()
                {
                    BackupPath = backupPath,
                    DatabaseName = _UserAccessInfo.DatabaseName,
                    MongoExePath = Tool.GetConfiguration("MongoDump"),
                    UploadPath = uploadPath
                };

                // Lấy thông tin username password
                if (cs.Contains("@"))
                {
                    var split = cs.Split('@');
                    var split_a = split[0].Split(':');
                    var split_b = split[1].Split(':');


                    info.UserName = split_a[0];
                    info.Password = split_a[1];
                    info.Host = split_b[0];

                    if (split_b[1].Contains('/'))
                    {
                        var split_c = split_b[1].Split('/');
                        info.Port = split_c[0];
                        info.AuthenticationDatabase = split_c[1];
                    }
                    else
                    {
                        info.Port = split_b[1];
                        info.AuthenticationDatabase = "admin";
                    }
                }
                else
                {
                    var split = cs.Split(':');

                    info.UserName = "";
                    info.Password = "";
                    info.Host = split[0];

                    if (split[1].Contains('/'))
                    {
                        var split_c = split[1].Split('/');
                        info.Port = split_c[0];
                        info.AuthenticationDatabase = split_c[1];
                    }
                    else
                    {
                        info.Port = split[1];
                        info.AuthenticationDatabase = "";
                    }
                }

                var rs = brMng.Backup(info);

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "Không phải Mongodb" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RestoreBackupFile(string name)
        {
            //string subdomain = Tool.GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var uploadPath = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.Upload, _SubDomain));
            var upFileName = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain), name);

            if (System.IO.File.Exists(upFileName))
            {

                var cs = Tool.GetConfiguration("ConnectionString"); // mongodb://superadmin:password@127.0.0.1:27017/admin

                if (cs.StartsWith("mongodb://"))
                {
                    cs = cs.Substring(10, cs.Length - 10);

                    BackupRestoreInfo info = new BackupRestoreInfo()
                    {
                        BackupPath = upFileName,
                        DatabaseName = _UserAccessInfo.DatabaseName,
                        MongoExePath = Tool.GetConfiguration("MongoRestore"),
                        UploadPath = uploadPath
                    };

                    if (cs.Contains("@"))
                    {
                        var split = cs.Split('@');
                        var split_a = split[0].Split(':');
                        var split_b = split[1].Split(':');


                        info.UserName = split_a[0];
                        info.Password = split_a[1];
                        info.Host = split_b[0];

                        if (split_b[1].Contains('/'))
                        {
                            var split_c = split_b[1].Split('/');
                            info.Port = split_c[0];
                            info.AuthenticationDatabase = split_c[1];
                        }
                        else
                        {
                            info.Port = split_b[1];
                            info.AuthenticationDatabase = "admin";
                        }
                    }
                    else
                    {
                        var split = cs.Split(':');

                        info.UserName = "";
                        info.Password = "";
                        info.Host = split[0];

                        if (split[1].Contains('/'))
                        {
                            var split_c = split[1].Split('/');
                            info.Port = split_c[0];
                            info.AuthenticationDatabase = split_c[1];
                        }
                        else
                        {
                            info.Port = split[1];
                            info.AuthenticationDatabase = "";
                        }
                    }

                    var rs = brMng.Restore(info);
                    return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.OK, Data = "" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "Không phải Mongodb" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "File không còn tồn tại" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadBackupFile(HttpPostedFileBase file)
        {
            string extension = Path.GetExtension(file.FileName);

            if (extension.ToLower() != ".zip")
                return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "Không phải file zip" }, JsonRequestBehavior.AllowGet);

            //string subdomain = GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var upFileName = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain), file.FileName);

            string location = Path.GetDirectoryName(upFileName);
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            try
            {
                using (FileStream fileStream = new FileStream(upFileName, FileMode.Create))
                {
                    file.InputStream.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.OK, Data = "" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckOverwriteFile(string name)
        {
            //string subdomain = GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var backupPath = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain));

            var lst = brMng.GetBackupFiles(backupPath).OrderByDescending(x => x.CreationTime);
            foreach (var i in lst)
            {
                if (i.Name == name)
                    return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.OK, Data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadBackupFile(string name)
        {
            //string subdomain = GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var filelocation = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain), name);

            if (System.IO.File.Exists(filelocation))
            {
                try
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filelocation);
                    string fileName = name;
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
                }
                catch
                {
                    return RedirectToAction("ErrorOccur", "Error");
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        public JsonResult RemoveBackupFile(string name)
        {
            //string subdomain = GetSubDomain(Request.Url);
            string physicalWebRootPath = Server.MapPath("/");
            var filelocation = Path.Combine(physicalWebRootPath, Tool.GetUploadFolder(UploadFolder.CustomerBackup, _SubDomain), name);

            if (System.IO.File.Exists(filelocation))
            {
                try
                {
                    System.IO.File.Delete(filelocation);
                    return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.OK, Data = "" }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.Error, Data = "Có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new ResultInfo() { Status = ResultInfo.ResultStatus.OK, Data = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}