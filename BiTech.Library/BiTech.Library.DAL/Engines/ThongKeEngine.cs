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
    public class ThongKeEngine : EntityRepository<PhieuMuon>
    {
        public ThongKeEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<PhieuMuon>(tableName);
        }

        public List<PhieuMuon> ListPhieuMuon()
        {
            // return _DatabaseCollection.Find(_ => _.TrangThaiPhieu != EPhieuMuon.Deleted).SortByDescending(x=>x.NgayPhaiTra).ToList();
            return _DatabaseCollection.Find(_ => _.TrangThaiPhieu != EPhieuMuon.Deleted).ToList();
        }    
        public List<PhieuMuon>GetPMByNgayMuon(DateTime ngayMuon)
        {
            return _DatabaseCollection.Find(_ => _.NgayMuon == ngayMuon).ToList();
        }
              
    }
}
