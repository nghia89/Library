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
    public class ThanhVienEngine : EntityRepository<ThanhVien>
    {
        public ThanhVienEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<ThanhVien>(tableName);
        }
        /// <summary>
        /// Get all ThanhVien object (Active - DeActive)GetByIdUser
        /// </summary>
        /// <returns></returns>
        public List<ThanhVien> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.TrangThai != EUser.Deleted).ToList();
        }
        /// <summary>
        /// Lấy 1 thành viên thông qua mã thành viên 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public ThanhVien GetByIdUser(string idUser)
        {
            return _DatabaseCollection.Find(_ => _.MaSoThanhVien == idUser).FirstOrDefault();
        }
    }
}
