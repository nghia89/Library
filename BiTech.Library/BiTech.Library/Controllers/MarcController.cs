using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using MARC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class MarcController : BaseController
    {

        // GET: Marc
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportMarc(DataFieldMarcVm dataField, HttpPostedFileBase file, IEnumerable<HttpPostedFileBase> files)
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


            FileMARC marcRecords = new FileMARC();
            if (file != null)
            {
                DateTime now = DateTime.Now;
                var imageFolder = $@"\filemarc\{now.ToString("yyyyMMdd")}";
                file.SaveAs(HttpContext.Server.MapPath(imageFolder) + file.FileName);
                char[] Character = { '.', ',', ':' };
                //marcRecords.ImportMARC(@"C:\Users\quang\Desktop\testMarc\testMarc\PGA-other-2.mrc");
                string ext = Path.GetExtension(file.FileName);
                if (ext == ".mrc")
                {
                    marcRecords.ImportMARC(HttpContext.Server.MapPath(imageFolder) + file.FileName);
                    foreach (Record record in marcRecords)
                    {
                        var tags = record.Fields;
                        dataField.MARC21 = "";
                        foreach (var t in tags)
                        {

                            Field eachField1 = (Field)record[t.Tag];

                            if (eachField1.GetType() == typeof(DataField))
                            {
                                DataField eachField = (DataField)eachField1;
                                dataField.MARC21 += "=" + eachField.Tag + "  ";
                                dataField.MARC21 += Convert.ToString(eachField.Indicator1) != " " ? Convert.ToString(eachField.Indicator1) : "#";
                                dataField.MARC21 += Convert.ToString(eachField.Indicator2) != " " ? Convert.ToString(eachField.Indicator2) : "#";
                                foreach (var subf in eachField.Subfields)
                                {
                                    dataField.MARC21 += "$" + Convert.ToString(subf.Code);
                                    dataField.MARC21 += subf.Data + " ";
                                }
                                dataField.MARC21 += "\n";
                            }
                            else if (eachField1.GetType() == typeof(ControlField))
                            {
                                ControlField eachField = (ControlField)eachField1;

                                System.Diagnostics.Debug.WriteLine(eachField.Tag + "-" + eachField.Data);
                            }
                        }
                        //DataFieldMarcVm dataField = new DataFieldMarcVm();
                        Field ControlNumber = record["001"];

                        //Each tag in the record is a field object. To get the data we have to know if it is a DataField or a ControlField and act accordingly.
                        if (ControlNumber != null)
                        {
                            dataField.MaKiemSoat = ((ControlField)ControlNumber).Data.ToString();
                        }
                        else
                        {
                            //Unreachable code!
                            Console.WriteLine("Data does not exist");
                        }

                        Field isbn = record["020"];
                        if (isbn != null)
                        {
                            DataField isbnDataField = (DataField)isbn;
                            Subfield isbnName = isbnDataField['a'];
                            dataField.ISBN = isbnName.Data;
                        }
                        else
                        {
                            Console.WriteLine("Data does not exist");
                        }


                        Field DisTriBution = record["260"];
                        if (DisTriBution != null)
                        {

                            DataField DisTriButionDataField = (DataField)DisTriBution;
                            Subfield DisTriButionName = DisTriButionDataField['a'];
                            if (DisTriButionName != null)
                            {
                                dataField.XuatXu = DisTriButionName.Data.TrimEnd(Character);
                            }
                            else
                            {
                                Console.WriteLine("Data does not exist");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data does not exist");
                        }

                        Field Datepublication = record["260"];
                        if (Datepublication != null)
                        {

                            DataField DatepublicationDataField = (DataField)Datepublication;
                            Subfield DatepublicationName = DatepublicationDataField['c'];
                            if (DatepublicationName != null)
                            {
                                dataField.NamXuatBan = DatepublicationName.Data.TrimEnd(Character);

                            }
                            else
                            {
                                Console.WriteLine("Data does not exist");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data does not exist");
                        }
                        Field price = record["020"];
                        if (price != null)
                        {
                            DataField priceDataField = (DataField)price;
                            Subfield priceName = priceDataField['c'];
                            if (priceName != null)
                            {
                                dataField.GiaBia = priceName.Data;
                            }
                            else
                            {
                                Console.WriteLine("Null");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data does not exist");
                        }


                        Field PhysicalDescription = record["300"];
                        if (PhysicalDescription != null)
                        {
                            DataField PhysicalDescriptionDataField = (DataField)PhysicalDescription;
                            Subfield PhysicalDescriptiondName = PhysicalDescriptionDataField['a'];
                            if (PhysicalDescriptiondName != null)
                            {

                                dataField.SoTrang = PhysicalDescriptiondName.Data.TrimEnd(Character);
                            }
                            else
                            {
                                Console.WriteLine("Null");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data dose not exist");
                        }

                        Field Summary = record["520"];
                        if (Summary != null)
                        {
                            DataField SummaryDataField = (DataField)Summary;
                            Subfield SummaryName = SummaryDataField['a'];
                            if (SummaryName != null)
                            {
                                dataField.TomTat = SummaryName.Data;
                            }
                            else
                            {
                                Console.WriteLine("Data  dose not exist");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data  dose not exist");
                        }


                        Field TitleStatement = record["245"];
                        if (TitleStatement != null)
                        {
                            DataField TitleStatementDataField = (DataField)TitleStatement;
                            Subfield TitleStatementName = TitleStatementDataField['a'];
                            dataField.TenSach = TitleStatementName.Data.TrimEnd(Character);
                        }
                        else
                        {
                            Console.WriteLine("Data dose not exist");
                        }

                        Field EdittionStatement = record["250"];
                        if (EdittionStatement != null)
                        {
                            DataField EdittionStatementDataField = (DataField)EdittionStatement;
                            Subfield EdittionStatementName = EdittionStatementDataField['a'];
                            dataField.TaiBan = EdittionStatementName.Data;
                        }
                        else
                        {
                            Console.WriteLine("Data dose not exist");
                        }
                        Field languageCode = record["041"];
                        if (languageCode != null)
                        {

                            DataField languageCodeDataField = (DataField)languageCode;
                            Subfield languageCodeName = languageCodeDataField['a'];
                            var getNamelanguageCode = _LanguageLogic.FindNamelanguge(languageCodeName.Data);
                            var getById = _LanguageLogic.FindNameId(languageCodeName.Data);
                            if (getNamelanguageCode.Count <= 0)
                            {
                                _LanguageLogic.InsertNew(new Language()
                                {
                                    Ten = languageCodeName.Data
                                });
                            }
                            else
                            {
                                dataField.IdNgonNgu = getById.Id;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Data Does not exist");
                        }
                        Sach ListdataField = new Sach()
                        {
                            MaKiemSoat = dataField.MaKiemSoat,
                            ISBN = dataField.ISBN,
                            GiaBia = dataField.GiaBia,
                            SoTrang = dataField.SoTrang,
                            TenSach = dataField.TenSach,
                            TaiBan = dataField.TaiBan,
                            MARC21 = dataField.MARC21,
                            TomTat = dataField.TomTat,
                            XuatXu = dataField.XuatXu,
                            NamXuatBan = dataField.NamXuatBan

                        };


                        var id = _SachLogic.ThemSach(ListdataField);

                        Field authorField = record["100"];
                        if (authorField != null)
                        {

                            DataField authorDataField = (DataField)authorField;
                            Subfield authorName = authorDataField['a'];
                            var getNameTacGia = _TacGiaLogic.FindNameTacGia(authorName.Data);
                            var getById = _TacGiaLogic.FindNameId(authorName.Data);

                            if (getNameTacGia.Count <= 0)
                            {
                                _TacGiaLogic.Insert(new TacGia()
                                {
                                    TenTacGia = authorName.Data
                                });
                                var getId = _TacGiaLogic.FindNameId(authorName.Data);
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
                            //Unreachable code!
                            Console.WriteLine("Data dose not exist");
                        }

                    }
                }
                else
                {
                    TempData["alert"] = "Định dạng file không đúng phải là định dạng .mrc";
                    //return RedirectToAction("Index", "sach");
                }
            }
            else
            {
                return RedirectToAction("Index", "sach");
            }
            return RedirectToAction("Index", "sach");
        }

    }
}
