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
        public SachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachEngine = new SachEngine(database, databaseName, DBTableNames.Sach_Table);
            _ThongTinThuVienEngine = new ThongTinThuVienEngine(database, databaseName, DBTableNames.ThongTinThuVien_Table);
            _SachTacGiaEngine = new SachTacGiaEngine(database, databaseName, DBTableNames.Sach_TacGia_Table);

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
		/// <summary>
		/// Ham get all isDelete = false thong qua Ma kiem soat
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		public Sach GetBook_NonDelete_ByMKS(string maKS)
		{
			return _sachEngine.GetBook_NonDelete_ByMKS(maKS);
		}
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
            return _sachEngine.GetByMaKiemSoat(maKS);
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
