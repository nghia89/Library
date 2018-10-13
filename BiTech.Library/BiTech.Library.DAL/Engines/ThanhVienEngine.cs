using BiTech.Library.DAL.Common;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
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
        public ThanhVienEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
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
            return _DatabaseCollection.Find(_ => true).ToList();
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

        public ThanhVien GetByUserName(string userName)
        {
            return _DatabaseCollection.Find(_ => _.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
        }

        public List<ThanhVien> GetAllHS()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "hs" && _.IsDeleted == false).ToList();
        }

        public List<ThanhVien> GetAllGV()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "gv" && _.IsDeleted == false).ToList();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(ThanhVien).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }

        #region Vinh tim kiem thanh vien

        public List<ThanhVien> GetMembersSearch (string KeySearch, string memType)
        {
            FilterDefinition<ThanhVien> filterDefinition = new BsonDocument();
            var builder = Builders<ThanhVien>.Filter;
            //Tim thanh vien khong bi xoa
            filterDefinition = filterDefinition & builder.Where(_ => _.IsDeleted == false);

            filterDefinition = filterDefinition & builder.Where(_ => _.TrangThai == EUser.Active);

            if (!string.IsNullOrEmpty(memType))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.LoaiTK.ToLower().Contains(memType.ToLower()));
            }
            //Tim theo ma thanh vien 
            if (!string.IsNullOrEmpty(KeySearch))
            {
                filterDefinition = filterDefinition 
                    & builder.Where(_ => _.MaSoThanhVien.ToLower().Contains(KeySearch.ToLower())
                    || _.Ten.ToLower().Contains(KeySearch.ToLower()));                    
            }
           

            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        public List<ThanhVien> GetAllHS_Active()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "hs" && _.IsDeleted == false && _.TrangThai == EUser.Active).ToList();
        }

        public List<ThanhVien> GetAllGV_Active()
        {
            return _DatabaseCollection.Find(_ => _.LoaiTK.ToLower() == "gv" && _.IsDeleted == false && _.TrangThai == EUser.Active).ToList();
        }

        #endregion
    }
}
