using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
//using MARC;
using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using BiTech.Library.Marc;
using System.Web;
using MARC4J.Net;
using MARC4J.Net.MARC;

namespace BiTech.Library.Controllers
{
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

        //[HttpPost]
        //public ActionResult ImportMarc(DataFieldMarcVm dataField)
        //{
        //    #region  Lấy thông tin người dùng
        //    var userdata = GetUserData();
        //    if (userdata == null)
        //        return RedirectToAction("LogOff", "Account");
        //    #endregion                   

        //    SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);


        //    //var file = dataField.LinFileMarc;
        //    //FileMARC marcRecords = new FileMARC();

        //    //iterating through multiple file collection   
        //    foreach (HttpPostedFileBase files in dataField.Files)
        //    {
        //        if (files != null)
        //        {
        //            DateTime now = DateTime.Now;
        //            var imageFolder = $@"\Upload\{now.ToString("yyyyMMdd")}";
        //            files.SaveAs(HttpContext.Server.MapPath(imageFolder) + files.FileName);
        //            char[] Character = { '.', ',', ':' };
        //            //string ext = Path.GetExtension(files.FileName);
        //            //marcRecords.ImportMARC(HttpContext.Server.MapPath(imageFolder) + file.FileName);
        //            string LinkFile = HttpContext.Server.MapPath(imageFolder) + files.FileName;
        //            using (FileMARCReaders marcRecords = new FileMARCReaders(LinkFile, false))
        //            {

        //                IEnumerable recordEnumerator = marcRecords;
        //                //if (LinkFile != null)
        //                //{
        //                //    marcRecords = new FileMARCReaders(LinkFile, false);
        //                //    recordEnumerator = marcRecords;
        //                //}
        //                //else
        //                //{
        //                //    //Collection<Record> importedSRU = (Collection<Record>)e.Argument;
        //                //    recordEnumerator = LinkFile;
        //                //}

        //                foreach (Marc.Record record in recordEnumerator)
        //                {
        //                    var tags = record.Fields;
        //                    foreach (var t in tags)
        //                    {

        //                        MARC.Field eachField1 = (MARC.Field)record[t.Tag];

        //                        if (eachField1.GetType() == typeof(DataField))
        //                        {
        //                            DataField eachField = (MARC.DataField)eachField1;
        //                            dataField.MARC21 += "=" + eachField.Tag + "  ";
        //                            dataField.MARC21 += Convert.ToString(eachField.Indicator1) != " " ? Convert.ToString(eachField.Indicator1) : "#";
        //                            dataField.MARC21 += Convert.ToString(eachField.Indicator2) != " " ? Convert.ToString(eachField.Indicator2) : "#";
        //                            foreach (var subf in eachField.Subfields)
        //                            {
        //                                dataField.MARC21 += "$" + Convert.ToString(subf.Code);
        //                                dataField.MARC21 += subf.Data;
        //                                //dataField.MARC21 += Regex.Replace(subf.Data, "[\u001e\u001d]", "");
        //                            }
        //                            dataField.MARC21 += "\n";
        //                        }
        //                        else if (eachField1.GetType() == typeof(ControlField))
        //                        {
        //                            ControlField eachField = (ControlField)eachField1;

        //                            System.Diagnostics.Debug.WriteLine(eachField.Tag + "-" + eachField.Data);
        //                        }
        //                    }
        //                    //DataFieldMarcVm dataField = new DataFieldMarcVm();
        //                    Field ControlNumber = record["001"];

        //                    //Each tag in the record is a field object. To get the data we have to know if it is a DataField or a ControlField and act accordingly.
        //                    if (ControlNumber != null)
        //                    {
        //                        dataField.MaKiemSoat = ((ControlField)ControlNumber).Data.ToString();
        //                    }
        //                    else
        //                    {
        //                        //Unreachable code!
        //                        Console.WriteLine("Data does not exist");
        //                    }

        //                    foreach (var item in tags)
        //                    {
        //                        if (item.Tag == "020")
        //                        {
        //                            Field eachField1 = item;
        //                            if (eachField1.GetType() == typeof(DataField))
        //                            {
        //                                //Field eachField1 = (Field)record[item.Tag];
        //                                DataField isbnDataField = (DataField)eachField1;
        //                                Subfield GiaBiaName = isbnDataField['c'];
        //                                //dataField.GiaBia = GiaBiaName.Data;
        //                                if (GiaBiaName != null && isbnDataField['c'] != null)
        //                                {
        //                                    //Subfield GiaBiaName = isbnDataField['c'];

        //                                    dataField.GiaBia = GiaBiaName.Data;

        //                                }
        //                                else
        //                                {
        //                                    //Unreachable code!
        //                                    Console.WriteLine("Data does not exist");
        //                                }

        //                                Subfield ISBN = isbnDataField['a'];
        //                                if (ISBN != null && isbnDataField['a'] != null)
        //                                {
        //                                    dataField.ISBN = ISBN.Data;
        //                                }
        //                                else
        //                                    Console.WriteLine("Data does not exist");
        //                            }
        //                        }
        //                        //DataField datafield = (DataField)record["245"];
        //                        //if (datafield != null)
        //                        //{
        //                        //    dataField.TenSach = datafield['a'].Data;
        //                        //}   
        //                        //else
        //                        //{
        //                        //    Console.WriteLine("Data dose not exist");
        //                        //}



        //                        Field TitleStatement = record["245"];
        //                        if (TitleStatement != null)
        //                        {
        //                            DataField TitleStatementDataField = (DataField)TitleStatement;
        //                            Subfield TitleStatementName = TitleStatementDataField['a'];
        //                            dataField.TenSach = TitleStatementName.Data.Replace("\u001e\u001d", "");
        //                            var x = dataField.TenSach;
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data dose not exist");
        //                        }

        //                        Field DisTriBution = record["260"];
        //                        if (DisTriBution != null)
        //                        {

        //                            DataField DisTriButionDataField = (DataField)DisTriBution;
        //                            Subfield DisTriButionName = DisTriButionDataField['a'];
        //                            if (DisTriButionName != null)
        //                            {
        //                                dataField.XuatXu = DisTriButionName.Data.TrimEnd(Character);
        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine("Data does not exist");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data does not exist");
        //                        }

        //                        Field Datepublication = record["260"];
        //                        if (Datepublication != null)
        //                        {

        //                            DataField DatepublicationDataField = (DataField)Datepublication;
        //                            Subfield DatepublicationName = DatepublicationDataField['c'];
        //                            if (DatepublicationName != null)
        //                            {
        //                                dataField.NamXuatBan = DatepublicationName.Data.TrimEnd(Character);

        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine("Data does not exist");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data does not exist");
        //                        }


        //                        Field PhysicalDescription = record["300"];
        //                        if (PhysicalDescription != null)
        //                        {
        //                            DataField PhysicalDescriptionDataField = (DataField)PhysicalDescription;
        //                            Subfield PhysicalDescriptiondName = PhysicalDescriptionDataField['a'];
        //                            if (PhysicalDescriptiondName != null)
        //                            {

        //                                dataField.SoTrang = PhysicalDescriptiondName.Data.TrimEnd(Character);
        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine("Null");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data dose not exist");
        //                        }

        //                        Field Summary = record["520"];
        //                        if (Summary != null)
        //                        {
        //                            DataField SummaryDataField = (DataField)Summary;
        //                            Subfield SummaryName = SummaryDataField['a'];
        //                            if (SummaryName != null)
        //                            {
        //                                dataField.TomTat = SummaryName.Data;
        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine("Data  dose not exist");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data  dose not exist");
        //                        }




        //                        Field EdittionStatement = record["250"];
        //                        if (EdittionStatement != null)
        //                        {
        //                            DataField EdittionStatementDataField = (DataField)EdittionStatement;
        //                            Subfield EdittionStatementName = EdittionStatementDataField['a'];
        //                            dataField.TaiBan = EdittionStatementName.Data;
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data dose not exist");
        //                        }
        //                        Field languageCode = record["041"];
        //                        if (languageCode != null)
        //                        {

        //                            DataField languageCodeDataField = (DataField)languageCode;
        //                            Subfield languageCodeName = languageCodeDataField['a'];
        //                            var getNamelanguageCode = _LanguageLogic.FindNamelanguge(languageCodeName.Data);
        //                            var getById = _LanguageLogic.FindNameId(languageCodeName.Data);
        //                            if (getNamelanguageCode.Count < 0)
        //                            {
        //                                _LanguageLogic.InsertNew(new Language()
        //                                {
        //                                    Ten = languageCodeName.Data
        //                                });
        //                            }
        //                            else
        //                            {
        //                                dataField.IdNgonNgu = getById.Id;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("Data Does not exist");
        //                        }


        //                    }

        //                    Sach ListdataField = new Sach()
        //                    {
        //                        MaKiemSoat = dataField.MaKiemSoat,
        //                        ISBN = dataField.ISBN,
        //                        GiaBia = dataField.GiaBia,
        //                        SoTrang = dataField.SoTrang,
        //                        TenSach = dataField.TenSach,
        //                        TaiBan = dataField.TaiBan,
        //                        MARC21 = dataField.MARC21,
        //                        TomTat = dataField.TomTat,
        //                        XuatXu = dataField.XuatXu,
        //                        NamXuatBan = dataField.NamXuatBan

        //                    };


        //                    var id = _SachLogic.ThemSach(ListdataField);

        //                    Field authorField = record["100"];
        //                    if (authorField != null)
        //                    {

        //                        DataField authorDataField = (DataField)authorField;
        //                        Subfield authorName = authorDataField['a'];
        //                        var getNameTacGia = _TacGiaLogic.FindNameTacGia(authorName.Data);
        //                        var getById = _TacGiaLogic.FindNameId(authorName.Data);

        //                        if (getNameTacGia.Count <= 0)
        //                        {
        //                            _TacGiaLogic.Insert(new TacGia()
        //                            {
        //                                TenTacGia = authorName.Data
        //                            });
        //                            var getId = _TacGiaLogic.FindNameId(authorName.Data);
        //                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
        //                            {
        //                                IdSach = id,
        //                                IdTacGia = getId.Id
        //                            });
        //                        }

        //                        else
        //                        {
        //                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
        //                            {
        //                                IdSach = id,
        //                                IdTacGia = getById.Id
        //                            });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Unreachable code!
        //                        Console.WriteLine("Data dose not exist");
        //                    }

        //                }
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "sach");
        //        }
        //    }
        //    return RedirectToAction("Index", "sach");
        //}

        [HttpGet]
        public ActionResult ImportMarc4j()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportMarc4j(DataFieldMarcVm dataFieldMarcVm)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion                   

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            MarcFactory factory = MarcFactory.Instance;
            var files = dataFieldMarcVm.File;
            DateTime now = DateTime.Now;
            var imageFolder = $@"\Upload\{now.ToString("yyyyMMdd")}";
            files.SaveAs(HttpContext.Server.MapPath(imageFolder) + files.FileName);
            char[] Character = { '.', ',', ':' };
            string LinkFile = HttpContext.Server.MapPath(imageFolder) + files.FileName;

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
                            dataFieldMarcVm.SoTrang = "";

                            ////ghi vào file dạng marcXml
                            for (var i = 0; i < allFields.Count; i++)
                            {
                                dataFieldMarcVm.MARC21 += "=" + allFields[i] + "  ";
                                dataFieldMarcVm.MARC21 += "\n";
                            }

                            /// các trường kiễm soát
                            foreach (var controlFile in controlFields)
                            {
                                if (controlFile.Tag == "001" && controlFile.Data != null)
                                {
                                    dataFieldMarcVm.MaKiemSoat = controlFile.Data;
                                }
                                else
                                {
                                    //Unreachable code!
                                    Console.WriteLine("Data does not exist");
                                }

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

                                //ngôn ngữ
                                if (dataFile.Tag == "041")
                                {
                                    var subfields = dataFile.GetSubfields();
                                    foreach (var subfield in subfields)
                                    {
                                        if(subfield.Code == 'c' && subfield.Data != null)
                                        {
                                            var getNamelanguageCode = _LanguageLogic.FindNamelanguge(subfield.Data);
                                            var getById = _LanguageLogic.FindNameId(subfield.Data);
                                            if (getNamelanguageCode.Count <= 0)
                                            {
                                                _LanguageLogic.InsertNew(new Language()
                                                {
                                                    Ten = subfield.Data
                                                });
                                                dataFieldMarcVm.IdNgonNgu = getById.Id;
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
                                            dataFieldMarcVm.TenSach = subfield.Data;
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
                                if (dataFile.Tag == "520")
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
                                        if (subfield.Code == 'a' && subfield.Data != null)
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
                                    }
                                }

                            }

                            Sach ListdataField = new Sach()
                            {
                                MaKiemSoat = dataFieldMarcVm.MaKiemSoat,
                                ISBN = dataFieldMarcVm.ISBN,
                                GiaBia = dataFieldMarcVm.GiaBia,
                                SoTrang = dataFieldMarcVm.SoTrang,
                                TenSach = dataFieldMarcVm.TenSach,
                                TaiBan = dataFieldMarcVm.TaiBan,
                                MARC21 = dataFieldMarcVm.MARC21,
                                TomTat = dataFieldMarcVm.TomTat,
                                XuatXu = dataFieldMarcVm.XuatXu,
                                NamXuatBan = dataFieldMarcVm.NamXuatBan
                            };
                            var id = _SachLogic.ThemSach(ListdataField);

                            if (DataTacGia != null)
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
            return RedirectToAction("Index", "sach");
        }
    }
}
