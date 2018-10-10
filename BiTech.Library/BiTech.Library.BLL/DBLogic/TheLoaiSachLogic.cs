using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;

namespace BiTech.Library.BLL.DBLogic
{
    public class TheLoaiSachLogic : BaseLogic
    {

        TheLoaiSachEngine _theloaiSachEngine;
        public TheLoaiSachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _theloaiSachEngine = new TheLoaiSachEngine(database, databaseName, "TheLoaiSach");
        }

        public List<TheLoaiSach> GetAllTheLoaiSach(bool showlevel = false)
        {
            var listTL = _theloaiSachEngine.GetAllTheLoaiSach();

            if (showlevel)
            {
                foreach (var item in listTL)
                {
                    string level = MakeLevel(item);
                    item.TenTheLoai = level + " " + item.TenTheLoai;
                }
            }

            return listTL;
        }

        public List<TheLoaiSach> GetAllTheLoaiSachRoot()
        {
            return _theloaiSachEngine.GetTheTheLoaiSachRoot();
        }

        public List<TheLoaiSach> GetAllTheLoaiSachChildren(string idParent)
        {
            return _theloaiSachEngine.GetTheTheLoaiSachChildren(idParent);
        }

        public string ThemTheLoaiSach(TheLoaiSach TL)
        {
            return _theloaiSachEngine.Insert(TL);
        }

        public TheLoaiSach getById(string id)
        {
            return _theloaiSachEngine.GetById(id);
        }

        public bool SuaTheLoaiSach(TheLoaiSach TL)
        {
            return _theloaiSachEngine.Update(TL);
        }

        public bool XoaTheLoaiSach(string id)
        {
            bool rs = true;
            
            foreach(var item in _theloaiSachEngine.GetTheCon(id, ""))
            {
                rs = rs ? XoaTheLoaiSach(item.Id) : true;
            }

            return _theloaiSachEngine.Remove(id);
        }

        public List<TheLoaiSach> FindTheLoai(string q)
        {
            return _theloaiSachEngine.FindTheLoai(q);
        }

        public string Insert(TheLoaiSach tacgia)
        {
            return _theloaiSachEngine.Insert(tacgia);
        }

        #region Tai
        public TheLoaiSach GetIdByDDC(string maDDC)
        {
            return _theloaiSachEngine.GetIdByDDC(maDDC);
        }

        public bool Update(TheLoaiSach tls)
        {
            return _theloaiSachEngine.Update(tls);
        }

        public TheLoaiSach GetByTenTheLoai(string tenTheLoai)
        {
            return _theloaiSachEngine.GetByTenTheLoai(tenTheLoai);
        }

        public bool ktrTrung(TheLoaiSach tls)
        {
            return _theloaiSachEngine.ktrTrung(tls);
        }       
        #endregion

        /// <summary>
        /// Tạo '-' trước thể loại con
        /// VD:
        /// Khoa học
        /// -Vật lý
        /// --Vật chất rắn
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string MakeLevel(TheLoaiSach item)
        {
            string level = "";

            if (!string.IsNullOrEmpty(item.IdParent))
            {
                var parent = _theloaiSachEngine.GetById(item.IdParent);
                if (parent != null)
                {
                    level += "-";
                    level += MakeLevel(parent);
                }
            }
            return level;
        }
    }
}
