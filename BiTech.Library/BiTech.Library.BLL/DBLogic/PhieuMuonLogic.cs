using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class PhieuMuonLogic
    {
        private PhieuMuonEngine _PhieuMuonEngine;
        private ChiTietPhieuMuonEngine _ChiTietPhieuMuonEngine;
        private PhieuTraEngine _PhieuTraEngine;
        private ChiTietPhieuTraEngine _ChiTietPhieuTraEngine;

        public PhieuMuonLogic(string connectionString, string dbName)
        {
            _PhieuMuonEngine = new PhieuMuonEngine(new Database(connectionString, dbName), DBTableNames.PhieuMuon_Table);
            _ChiTietPhieuMuonEngine = new ChiTietPhieuMuonEngine(new Database(connectionString, dbName), DBTableNames.ChiTietPhieuMuon_Table);
            
            _PhieuTraEngine = new PhieuTraEngine(new Database(connectionString, dbName), DBTableNames.PhieuTra_Table);
            _ChiTietPhieuTraEngine = new ChiTietPhieuTraEngine(new Database(connectionString, dbName), DBTableNames.ChiTietPhieuTra_Table);
        }

        /// <summary>
        /// Get all PhieuMuon object _ chưa trả
        /// </summary>
        /// <returns></returns>
        public List<PhieuMuon> GetAll()
        {
            return _PhieuMuonEngine.GetAll();
        }

        public PhieuMuon GetById(string id)
        {
            return _PhieuMuonEngine.GetById(id);
        }

        /// <summary>
        /// get by UserID _ chưa trả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PhieuMuon> GetByIdUser(string idUser)
        {
            return _PhieuMuonEngine.GetByIdUser(idUser);
        }

        /// <summary>
        /// Insert a PhieuMuon object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(PhieuMuon model)
        {
            return _PhieuMuonEngine.Insert(model);
        }

        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(PhieuMuon model)
        {
            return _PhieuMuonEngine.Update(model);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Remove(string id)
        {
            //todo
            //_ChiTietPhieuMuonEngine.RemoveByPhieuMuonId(id);
            return _PhieuMuonEngine.Remove(id);
        }

        /// <summary>
        /// Lấy ra thông tin sách đầy đủ của 1 phiếu mượn bao gồm sách mượn và đã trả
        /// </summary>
        /// <param name="idPM"></param>
        /// <returns></returns>
        public PhieuMuonSach_FullDetail GetReturnBooksTicket(string idPM)
        {
            var PM = _PhieuMuonEngine.GetById(idPM);
            var lstCT_PM = _ChiTietPhieuMuonEngine.GetByIdPhieuMuon(idPM);

            // lấy toàn bộ sách trong các phiếu trả
            var listPT = _PhieuTraEngine.GetPTByIdPM(idPM);
            List<ChiTietPhieuTra> allPhieuTra_bookList = new List<ChiTietPhieuTra>();
            foreach (var p in listPT)
            {
                allPhieuTra_bookList.AddRange(_ChiTietPhieuTraEngine.GetByIdPhieuTra(p.Id));
            }

            // Đổi phiếu tra sang full detail
            var detail = new PhieuMuonSach_FullDetail();
            foreach(var ct in lstCT_PM)
            {
                detail.BookList.Add(new ChiTietPhieuMuon_FullDetail()
                {
                    Id = ct.Id,
                    IdPhieuMuon = idPM,
                    IdSach = ct.IdSach,
                    SoLuong = ct.SoLuong,
                    CreateDateTime = ct.CreateDateTime,
                    SoLuongSachDaTra = 0
                });
            }

            // Tính số sách đã được trả
            foreach (var book in detail.BookList)
            {
                foreach(var ct in allPhieuTra_bookList)
                {
                    if(book.IdSach == ct.IdSach)
                    {
                        book.SoLuongSachDaTra += ct.SoLuong;
                    }
                }
            }

            return detail;
        }
    }
}
