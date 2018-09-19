using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using BiTech.Library.Marc;
using System.Web;
using BiTech.Library.Helpers;
using MARC4J.Net;
using MARC4J.Net.MARC;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class MarcController : BaseController
    {
        //private FileMARCReaders marcRecords;
        // GET: Marc
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ImportMarc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportMarc(DataFieldMarcVm dataFieldMarcVm)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            char[] Character = { '/', '*', ' ', ':', '.', ',' };

            foreach (HttpPostedFileBase file in dataFieldMarcVm.Files)
            {
                if (file != null)
                {
                    var files = dataFieldMarcVm.Files;
                    DateTime now = DateTime.Now;
                    var marcFolder = Path.Combine(Tool.GetUploadFolder(UploadFolder.FileMrc, _SubDomain), now.ToString("yyyyMMdd"));

                    string physicalWebRootPath = Server.MapPath("~/");
                    var localMarcFolder = Path.Combine(physicalWebRootPath, marcFolder);
                    if (!Directory.Exists(localMarcFolder))
                        Directory.CreateDirectory(localMarcFolder);

                    file.SaveAs(Path.Combine(localMarcFolder, file.FileName));
                    
                    string LinkFile = Path.Combine(localMarcFolder, file.FileName);

                    using (var fs = new FileStream(LinkFile, FileMode.Open))
                    {
                        using (var reader = new MarcPermissiveStreamReader(fs, true, true, "UTF-8"))
                        {
                            using (var output = new MemoryStream())
                            {
                                MarcStreamWriter writer = new MarcStreamWriter(output);
                                foreach (var record in reader)
                                {
                                    var leader = record.Leader;
                                    var controlFields = record.GetControlFields();
                                    var dataFields = record.GetDataFields();
                                    var allFields = record.GetVariableFields();

                                    dataFieldMarcVm.MaKiemSoat = "";
                                    dataFieldMarcVm.MARC21 = "";
                                    dataFieldMarcVm.ISBN = "";
                                    dataFieldMarcVm.TenSach = "";
                                    dataFieldMarcVm.GiaBia = "";
                                    dataFieldMarcVm.TaiBan = "";
                                    dataFieldMarcVm.XuatXu = "";
                                    dataFieldMarcVm.NamXuatBan = "";
                                    dataFieldMarcVm.SoTrang = "";
                                    dataFieldMarcVm.TomTat = "";
                                    dataFieldMarcVm.IdNgonNgu = "";
                                    dataFieldMarcVm.DDC = "";
                                    dataFieldMarcVm.NamXuatBan = "";
                  

                                    ////ghi vào file dạng marcXml
                                    for (var i = 0; i < allFields.Count; i++)
                                    {
                                        dataFieldMarcVm.MARC21 += "=" + allFields[i] + "  ";
                                        dataFieldMarcVm.MARC21 += "\n";
                                    }

                                    /// các trường kiễm soát
                                    foreach (var controlFile in controlFields)
                                    {
                                        //mã kiễm soát

                                        if (controlFile.Tag == "001" && controlFile.Data != null)
                                        {
                                            dataFieldMarcVm.MaKiemSoat = controlFile.Data;
                                        }
                                        else
                                        {
                                            //Unreachable code!
                                            Console.WriteLine("Data does not exist");
                                        }


                                        //if (controlFile.Tag == "008" && controlFile.Data != null)
                                        //{
                                        //    dataFieldMarcVm.DDC = controlFile.Data;
                                        //}
                                        //else
                                        //{
                                        //    //Unreachable code!
                                        //    Console.WriteLine("Data does not exist");
                                        //}

                                        ///ngày giao dịch chưa có

                                        //if (controlFile.Tag == "005" && controlFile.Data != null)
                                        //{
                                        //    dataFieldMarcVm.MaKiemSoat = controlFile.Data;
                                        //}
                                        //else
                                        //{
                                        //    //Unreachable code!
                                        //    Console.WriteLine("Data does not exist");
                                        //}                              
                                    }

                                    var DataTacGia = "";
                                    ///các trường nội dung thông tin sách
                                    foreach (var dataFile in dataFields)
                                    {
                                        //mã ISBN "a"
                                        if (dataFile.Tag == "020")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.ISBN = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //giá bìa "c"
                                        if (dataFile.Tag == "020")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'c' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.GiaBia = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }
                                            }
                                        }

                                        //DDC
                                        if (dataFile.Tag == "082")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.DDC = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //ngôn ngữ
                                        if (dataFile.Tag == "041")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null && subfield.Data != " ")
                                                {
                                                    var getNamelanguageCode = _LanguageLogic.FindNamelanguge(subfield.Data);
                                                    var getById = _LanguageLogic.FindNameId(subfield.Data);
                                                    if (getNamelanguageCode.Count <= 0)
                                                    {
                                                        dataFieldMarcVm.IdNgonNgu = _LanguageLogic.InsertNew(new Language()
                                                        {
                                                            Ten = subfield.Data
                                                        });
                                                    }
                                                    else
                                                    {
                                                        dataFieldMarcVm.IdNgonNgu = getById.Id;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }
                                            }
                                        }

                                        //tên sách
                                        if (dataFile.Tag == "245")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.TenSach = subfield.Data.Trim(Character);
                                                    dataFieldMarcVm.TenSachKhongDau = ConvertToUnSign.ConvertName(subfield.Data.Trim(Character));
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //tái bản
                                        if (dataFile.Tag == "250")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.TaiBan = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //số trang mô tả vật lý
                                        if (dataFile.Tag == "300")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' || subfield.Code == 'c' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.SoTrang += subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //tóm tắt
                                        if (dataFile.Tag == "500")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.TomTat = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //tác giả
                                        if (dataFile.Tag == "100")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null && subfield.Data != string.Empty)
                                                {
                                                    DataTacGia = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                            }
                                        }

                                        //năm xuất bản "c"
                                        //nhà xuất bản "b"
                                        if (dataFile.Tag == "260")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'c' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.NamXuatBan = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }

                                                if (subfield.Code == 'b' && subfield.Data != null)
                                                {
                                                    var getList = _NhaXuatBanLogic.GetByFindName(subfield.Data);
                                                    var getById = _NhaXuatBanLogic.FindNameId(subfield.Data);
                                                    if (getList.Count <= 0)
                                                    {
                                                        dataFieldMarcVm.IdNhaXuatBan = _NhaXuatBanLogic.ThemNXB(new NhaXuatBan()
                                                        {
                                                            Ten = subfield.Data
                                                        });
                                                    }
                                                    else
                                                    {
                                                        dataFieldMarcVm.IdNhaXuatBan = getById.Id;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");

                                                }
                                            }

                                        }

                                        //xuất xứ
                                        if (dataFile.Tag == "044")
                                        {
                                            var subfields = dataFile.GetSubfields();
                                            foreach (var subfield in subfields)
                                            {
                                                if (subfield.Code == 'a' && subfield.Data != null)
                                                {
                                                    dataFieldMarcVm.XuatXu = subfield.Data;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Data does not exist");
                                                }
                                            }
                                        }


                                    }

                                    Sach ListdataField = new Sach()
                                    {
                                        //MaKiemSoat = dataFieldMarcVm.MaKiemSoat,
                                        ISBN = dataFieldMarcVm.ISBN,
                                        GiaBia = dataFieldMarcVm.GiaBia,
                                        SoTrang = dataFieldMarcVm.SoTrang,
                                        TenSach = dataFieldMarcVm.TenSach,
                                        TenSachKhongDau = dataFieldMarcVm.TenSachKhongDau,
                                        TaiBan = dataFieldMarcVm.TaiBan,
                                        MARC21 = dataFieldMarcVm.MARC21,
                                        TomTat = dataFieldMarcVm.TomTat,
                                        XuatXu = dataFieldMarcVm.XuatXu,
                                        NamXuatBan = dataFieldMarcVm.NamXuatBan,
                                        IdNgonNgu = dataFieldMarcVm.IdNgonNgu,
                                        DDC = dataFieldMarcVm.DDC,
                                        IdNhaXuatBan = dataFieldMarcVm.IdNhaXuatBan

                                    };

                                    var id = _SachLogic.ThemSach(ListdataField);

                                    if (DataTacGia != null && DataTacGia != string.Empty)
                                    {
                                        var getNameTacGia = _TacGiaLogic.FindNameTacGia(DataTacGia);
                                        var getById = _TacGiaLogic.FindNameId(DataTacGia);
                                        if (getNameTacGia.Count <= 0)
                                        {
                                            _TacGiaLogic.Insert(new TacGia()
                                            {
                                                TenTacGia = DataTacGia
                                            });
                                            var getId = _TacGiaLogic.FindNameId(DataTacGia);
                                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                                            {
                                                IdSach = id,
                                                IdTacGia = getId.Id
                                            });
                                        }

                                        else
                                        {
                                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                                            {
                                                IdSach = id,
                                                IdTacGia = getById.Id
                                            });
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Data does not exist");
                                    }

                                    writer.Write(record);
                                }
                                //sau khi đọc file đóng lại
                                writer.Close();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File Null");
                }
            }

            return RedirectToAction("Index", "sach");
        }
    }
}
