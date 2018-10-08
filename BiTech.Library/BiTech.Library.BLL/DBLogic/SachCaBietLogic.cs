<<<<<<< HEAD
﻿using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
=======
﻿using System;
>>>>>>> Phongv25
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;
>>>>>>> Phongv25

namespace BiTech.Library.BLL.DBLogic
{
    public class SachCaBietLogic : BaseLogic
    {
        SachCaBietEngine _SachCaBietEngine;
        ThongTinThuVienEngine _ThongTinThuVienEngine;
		
        public SachCaBietLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _SachCaBietEngine = new SachCaBietEngine(database, databaseName, DBTableNames.SachCaBiet_Table);
            _ThongTinThuVienEngine = new ThongTinThuVienEngine(database, databaseName, DBTableNames.ThongTinThuVien_Table);
        }

        #region Vinh
        public string Insert(SachCaBiet sachCBiet)
        {
            return _SachCaBietEngine.Insert(sachCBiet);
        }

        public SachCaBiet GetIdSachFromMaCaBiet(string maCaBiet)
        {
            return _SachCaBietEngine.GetIdSachFromMaCaBiet(maCaBiet);
        }

        public List<SachCaBiet> GetListCaBietFromIdSach(string idSach)
        {
            return _SachCaBietEngine.GetListCaBietFromIdSach(idSach);
        }

        public bool Remove(string id)
        {
            return _SachCaBietEngine.Remove(id);
        }

        public List<SachCaBiet> GetAll()
        {
            return _SachCaBietEngine.GetAllSachCaBiet();
        }
        #endregion

        public string Add(SachCaBiet TL)
        {
            return _SachCaBietEngine.Insert(TL);
        }
        public SachCaBiet getById(string Id)
        {
            return _SachCaBietEngine.GetById(Id);
        }
        
        public bool Update(SachCaBiet id)
        {
            return _SachCaBietEngine.Update(id);
        }

        public bool Delete(string Id)
        {
            return _SachCaBietEngine.Remove(Id);
        }

        #region Phong

        public SachCaBiet GetAllByMaKSCBorMaCaBienCu(string masach)
        {
            return _SachCaBietEngine.GetAllByMaKSCBorMaCaBienCu(masach);
        }

        #endregion
    }
}
