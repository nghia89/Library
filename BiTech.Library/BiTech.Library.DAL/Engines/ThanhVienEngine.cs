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
        public ThanhVienEngine(IDatabase database, string databaseName, string tableName) : base(database,databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ThanhVien>(tableName);
        }
        /// <summary>
        /// Get all ThanhVien object (Active - DeActive)GetByIdUser
        /// </summary>
        /// <returns></returns>
        public List<ThanhVien> GetAll()
        {
            //return _DatabaseCollection.Find(_ => _.TrangThai != EUser.Deleted).ToList();
            return _DatabaseCollection.Find(_ =>true).ToList();
        }
        /// <summary>
        /// Get all ThanhVien object (Active)GetByIdUser
        /// </summary>
        /// <returns></returns>
        public List<ThanhVien> GetAllActive()
        {
            return _DatabaseCollection.Find(_ => _.TrangThai == EUser.Active).ToList();
        }
        /// <summary>
        /// Get ThanhVien object (DeActive)GetByIdUser
        /// </summary>
        /// <returns></returns>
        public ThanhVien GetByMaSoThanhVienDeActive(string idUser)
        {
            return _DatabaseCollection.Find(_ => _.TrangThai == EUser.DeActive && _.MaSoThanhVien == idUser).FirstOrDefault();
        }
        /// <summary>
        /// Lấy 1 thành viên thông qua mã thành viên 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public ThanhVien GetByMaSoThanhVien(string idUser)
        {
            return _DatabaseCollection.Find(_ => _.MaSoThanhVien == idUser).FirstOrDefault();
        }

        public List<ThanhVien> GetByName(string ten)
        {
            return _DatabaseCollection.Find(_ => _.Ten == ten).ToList();
        }

        public List<ThanhVien> GetAllHS()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "hs").ToList();
        }

        public List<ThanhVien> GetAllGV()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "gv").ToList();
        }
    }
}
