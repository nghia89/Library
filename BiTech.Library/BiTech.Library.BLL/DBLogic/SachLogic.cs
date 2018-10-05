using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;
using BiTech.Library.Common;

namespace BiTech.Library.BLL.DBLogic
{
    public class SachLogic : BaseLogic
    {
        SachEngine _sachEngine;
        ThongTinThuVienEngine _ThongTinThuVienEngine;
        SachTacGiaEngine _SachTacGiaEngine;
        NhaXuatBanEngine _NXBEngine;
        TacGiaEngine _TacGiaEngine;
        public SachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachEngine = new SachEngine(database, databaseName, DBTableNames.Sach_Table);
            _ThongTinThuVienEngine = new ThongTinThuVienEngine(database, databaseName, DBTableNames.ThongTinThuVien_Table);
            _SachTacGiaEngine = new SachTacGiaEngine(database, databaseName, DBTableNames.Sach_TacGia_Table);
            _NXBEngine = new NhaXuatBanEngine(database, databaseName, DBTableNames.NhaXuatBan_Table);
            _TacGiaEngine = new TacGiaEngine(database, databaseName, DBTableNames.TacGia_Table);

        }

        #region Vinh

        public Sach GetById(string id)
        {
            return _sachEngine.GetById(id);
        }

        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Sach model)
        {
            return _sachEngine.Update(model);
        }
        public List<Sach> GetAll_NonDelete()
        {
            return _sachEngine.GetAll_NonDelete();
        }
        /// <summary>
        /// Get book by idBook
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        //public Sach GetByIdBook(string idBook)
        //{
        //    return _sachEngine.GetByIdBook(idBook);
        //}

        public Sach GetBookById(string idBook)
        {
            return _sachEngine.GetById(idBook);
        }
<<<<<<< HEAD
        /// <summary>
        /// Ham get all isDelete = false thong qua Ma kiem soat
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Sach GetBook_NonDelete_ByMKS(string maKS)
        {
            return _sachEngine.GetBook_NonDelete_ByMKS(maKS);
        }
=======
		/// <summary>
		/// Ham get all isDelete = false thong qua Ma kiem soat hoặc ISBN
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		public Sach GetBook_NonDelete_ByMKS(string maKS)
		{
			return _sachEngine.GetBook_NonDelete_ByMKS(maKS);
		}
>>>>>>> 6a455a0a3d0dcc39beffe25b368ae46ea409a164
        #endregion

        #region Thinh
        public List<Sach> getAllSach()
        {
            return _sachEngine.GetAllSach();
        }
        public List<Sach> getAll()
        {
            return _sachEngine.GetAll();
        }
        public List<Sach> GetDatetime(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            return _sachEngine.GetDatetime(firstDayOfMonth, lastDayOfMonth);
        }
        public List<Sach> ListName(string keyWord)
        {
            return _sachEngine.ListName(keyWord);
        }
        public List<Sach> getPageSach(KeySearchViewModel KeySearch)
        {
            var listSachTacgia = _SachTacGiaEngine.GetAllBookIdByAthurId(KeySearch.TenTacGia);
            KeySearch.ListSachIds = new List<string>();

            foreach (var item in listSachTacgia)
            {
                KeySearch.ListSachIds.Add(item.IdSach);
            }

            return _sachEngine.getPageSach(KeySearch);
        }

        public List<Sach> getPageSachHighPerformance(KeySearchViewModel KeySearch)
        {
            if (!string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem0) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem1)
               || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem2) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem3) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem4))
            {
                if (KeySearch.ddlLoaiTimKiem0.Contains("author") || KeySearch.ddlLoaiTimKiem1.Contains("author")
                    || KeySearch.ddlLoaiTimKiem2.Contains("author") || KeySearch.ddlLoaiTimKiem3.Contains("author")
                    || KeySearch.ddlLoaiTimKiem4.Contains("author")|| KeySearch.ddlLoaiTimKiem0.Contains("any") || KeySearch.ddlLoaiTimKiem1.Contains("any") ||
                    KeySearch.ddlLoaiTimKiem2.Contains("any") || KeySearch.ddlLoaiTimKiem3.Contains("any") || KeySearch.ddlLoaiTimKiem4.Contains("any"))
                {
                    KeySearch.ListSachIds = new List<string>();
                    if (KeySearch.Keyword != null)
                    {
                        var listIdTacGia = _TacGiaEngine.GetByListName(KeySearch.Keyword);
                        foreach (var item in listIdTacGia)
                        {
                            var IdSach = _SachTacGiaEngine.GetId(item.Id);
                            if (IdSach != null)
                                KeySearch.ListSachIds.Add(IdSach.IdSach);
                        }
                    }
                    if (KeySearch.Keyword1 != null)
                    {
                        var listIdTacGia = _TacGiaEngine.GetByListName(KeySearch.Keyword1);
                        foreach (var item in listIdTacGia)
                        {
                            var IdSach = _SachTacGiaEngine.GetId(item.Id);
                            if (IdSach != null)
                                KeySearch.ListSachIds.Add(IdSach.IdSach);
                        }
                    }
                    if (KeySearch.Keyword2 != null)
                    {
                        var listIdTacGia = _TacGiaEngine.GetByListName(KeySearch.Keyword2);
                        foreach (var item in listIdTacGia)
                        {
                            var IdSach = _SachTacGiaEngine.GetId(item.Id);
                            if (IdSach != null)
                                KeySearch.ListSachIds.Add(IdSach.IdSach);
                        }
                    }
                    if (KeySearch.Keyword3 != null)
                    {
                        var listIdTacGia = _TacGiaEngine.GetByListName(KeySearch.Keyword3);
                        foreach (var item in listIdTacGia)
                        {
                            var IdSach = _SachTacGiaEngine.GetId(item.Id);
                            if (IdSach != null)
                                KeySearch.ListSachIds.Add(IdSach.IdSach);
                        }
                    }
                    if (KeySearch.Keyword4 != null)
                    {
                        var listIdTacGia = _TacGiaEngine.GetByListName(KeySearch.Keyword4);
                        foreach (var item in listIdTacGia)
                        {
                            var IdSach = _SachTacGiaEngine.GetId(item.Id);
                            if (IdSach != null)
                                KeySearch.ListSachIds.Add(IdSach.IdSach);
                        }
                    }

                }
            }

            if (!string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem0) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem1)
                || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem2) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem3) || !string.IsNullOrEmpty(KeySearch.ddlLoaiTimKiem4))
            {
                if (KeySearch.ddlLoaiTimKiem0.Contains("place_publication") || KeySearch.ddlLoaiTimKiem1.Contains("place_publication")
                    || KeySearch.ddlLoaiTimKiem2.Contains("place_publication") || KeySearch.ddlLoaiTimKiem3.Contains("place_publication")
                    || KeySearch.ddlLoaiTimKiem4.Contains("place_publication")|| KeySearch.ddlLoaiTimKiem0.Contains("any")|| KeySearch.ddlLoaiTimKiem1.Contains("any") ||
                    KeySearch.ddlLoaiTimKiem2.Contains("any") || KeySearch.ddlLoaiTimKiem3.Contains("any") || KeySearch.ddlLoaiTimKiem4.Contains("any"))
                {
                    KeySearch.ListIdNXB = new List<string>();
                    if (KeySearch.Keyword != null)
                    {
                        var ListNXB = _NXBEngine.GetByListName(KeySearch.Keyword);
                        if (ListNXB != null)
                        {
                            foreach (var item in ListNXB)
                            {
                                KeySearch.ListIdNXB.Add(item.Id);
                            }
                        }

                    }
                    if (KeySearch.Keyword1 != null)
                    {
                        var ListNXB = _NXBEngine.GetByListName(KeySearch.Keyword1);
                        if (ListNXB != null)
                            foreach (var item in ListNXB)
                            {
                                KeySearch.ListIdNXB.Add(item.Id);
                            }
                    }
                    if (KeySearch.Keyword2 != null)
                    {
                        var ListNXB = _NXBEngine.GetByListName(KeySearch.Keyword2);
                        if (ListNXB != null)
                            foreach (var item in ListNXB)
                            {
                                KeySearch.ListIdNXB.Add(item.Id);
                            }
                    }
                    if (KeySearch.Keyword3 != null)
                    {
                        var ListNXB = _NXBEngine.GetByListName(KeySearch.Keyword3);
                        if (ListNXB != null)
                            foreach (var item in ListNXB)
                            {
                                KeySearch.ListIdNXB.Add(item.Id);
                            }
                    }
                    if (KeySearch.Keyword4 != null)
                    {
                        var ListNXB = _NXBEngine.GetByListName(KeySearch.Keyword4);
                        if (ListNXB != null)
                            foreach (var item in ListNXB)
                            {
                                KeySearch.ListIdNXB.Add(item.Id);
                            }
                    }

                }
            }

            return _sachEngine.getPageHighPerformance(KeySearch);
        }

        public string ThemSach(Sach s)
        {
            var setting = _ThongTinThuVienEngine.GetMaKiemSoatSachCount();
            ulong max = 0;
            if (!ulong.TryParse(setting, out max))
            {
                _ThongTinThuVienEngine.SetMaKiemSoatSachCount("0");
                max = 1;
            }

            max++;
            do
            {
                var ss = _sachEngine.GetByMaKiemSoat(max.ToString("0000"));
                if (ss != null)
                    max++;
                else
                    break;
            } while (true);
            s.MaKiemSoat = max.ToString("0000");
            _ThongTinThuVienEngine.SetMaKiemSoatSachCount(max.ToString());
            return _sachEngine.Insert(s);
        }

        public bool XoaSach(string id)
        {
            return _sachEngine.Remove(id);
        }

        public Sach GetByMaMaKiemSoat(string maKS)
        {
            return _sachEngine.GetBook_NonDelete_ByMKS(maKS);
        }

        /// <summary>
        /// Lấy sách bằng MaKiemSoat or ISBN
        /// </summary>
        /// <param name="mastring"></param>
        /// <returns></returns>
        public Sach GetByMaKiemSoatorISBN(string mastring)
        {
            return _sachEngine.GetByMaKiemSoatorISBN(mastring);
        }

        #endregion

        #region Phong
        public Sach GetByID_IsDeleteFalse(string id)
        {
            return _sachEngine.GetByID_IsDeleteFalse(id);
        }
        #endregion

    }
}
