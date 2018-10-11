using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BiTech.Library.DAL.Engines
{
    public class NhaXuatBanEngine : EntityRepository<NhaXuatBan>
    {
        public NhaXuatBanEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<NhaXuatBan>(tableName);
        }

        public List<NhaXuatBan> GetAllNhaXuatBan()
        {
            return _DatabaseCollection.Find(p => true).ToList(); 
        }

        public List<NhaXuatBan> GetByFindName(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten.Equals(Name)).ToList();

        }
        #region nghia
        public NhaXuatBan GetByIdFindName(string Name)
        {
            return _DatabaseCollection.Find(_ => _.Ten.ToLower() == Name.ToLower()).FirstOrDefault();
            //return _DatabaseCollection.AsQueryable().Where(x => x.Ten.Equals(Name)).FirstOrDefault();
        }

        public List<NhaXuatBan> GetByListName1(string Name)
        {
            FilterDefinition<NhaXuatBan> filterDefinition = new BsonDocument();
            var builder = Builders<NhaXuatBan>.Filter;
            filterDefinition = builder.Where(x => x.Ten.ToLower().Contains(Name.ToLower()));
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        public List<NhaXuatBan> GetByListName2(string Name)
        {
            FilterDefinition<NhaXuatBan> filterDefinition = new BsonDocument();
            var builder = Builders<NhaXuatBan>.Filter;
            filterDefinition = builder.Where(x => x.Ten.ToLower()==(Name.ToLower()));
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }
        #endregion


        #region Tai
        public NhaXuatBan GetByTenNXB(string tenNXB)
        {
            return _DatabaseCollection.Find(_ => _.Ten.ToLower() == tenNXB.ToLower()).FirstOrDefault();
        }
        #endregion
    }
}
