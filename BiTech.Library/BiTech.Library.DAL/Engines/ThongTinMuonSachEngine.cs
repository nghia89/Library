using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class ThongTinMuonSachEngine : EntityRepository<ThongTinMuonSach>
    {
        public ThongTinMuonSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ThongTinMuonSach>(tableName);
        }
        /// <summary>
        /// Get all ThongTinMuonSach object 
        /// </summary>
        /// <returns></returns>
        public List<ThongTinMuonSach> GetAll()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
        public List<ThongTinMuonSach> GetAllTT(string id)
        {
            return _DatabaseCollection.Find(x => x.Id != id).ToList();
        }

        public List<ThongTinMuonSach> GetByidUser(string idUser)
        {
            return _DatabaseCollection.Find(x => x.idUser == idUser).ToList();
        }

        public List<ThongTinMuonSach> GetByidSach(string idSach)
        {
            return _DatabaseCollection.Find(x => x.idSach == idSach).ToList();
        }

        public List<ThongTinMuonSach> GetAllbyIdSachCaBiet_ChuaTra(string IdSachCaBiet)
        {
            return _DatabaseCollection.Find(x => x.IdSachCaBiet == IdSachCaBiet && x.DaTra == false).ToList();
        }

        public List<ThongTinMuonSach> GetByThongTinMuonSach(ThongTinMuonSach TT)
        {
            DateTime NgayGioMuon2 = TT.NgayGioMuon.AddDays(1);
            DateTime NgayPhaiTra2 = TT.NgayPhaiTra.AddDays(1);

            return _DatabaseCollection.Find(x => x.idUser == TT.idUser
                && x.idSach == TT.idSach
                && x.NgayGioMuon >= TT.NgayGioMuon && x.NgayGioMuon < NgayGioMuon2
                && x.NgayPhaiTra == TT.NgayPhaiTra && x.NgayPhaiTra < NgayPhaiTra2
                && x.DaTra == false
            ).ToList();
        }

        public List<ThongTinMuonSach> GetByidUser_ChuaTra(string idUser)
        {
            //var l = _DatabaseCollection.Find(x => x.idUser == idUser && x.DaTra == false).ToList();
            //string dates = l[0].NgayGioMuon.ToString("dd/MM/yyyy HH:mm:ss");
            //var tt = new ThongTinMuonSach();
            //tt.NgayGioMuon = DateTime.ParseExact(dates, "dd/MM/yyyy HH:mm:ss", null);

            //var rs = tt.NgayGioMuon == l[0].NgayGioMuon;
            //try
            //{
            //    var l2 = _DatabaseCollection.Find(x => x.NgayGioMuon.Date == tt.NgayGioMuon.Date).ToList();
            //}
            //catch (Exception ex)
            //{
            //}

            return _DatabaseCollection.Find(x => x.idUser == idUser && x.DaTra == false).ToList();
        }

        public List<ThongTinMuonSach> GetBy_ChuaTra_byidSach(string idSach)
        {
            return _DatabaseCollection.Find(x => x.idSach == idSach && x.DaTra == false).ToList();
        }

        #region Tai
        public List<ThongTinMuonSach> GetTTMSByIdUser(string idUser)
        {
            return _DatabaseCollection.Find(x => x.idUser == idUser).ToList();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(ThongTinThuVien).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
        #endregion

    }
}
