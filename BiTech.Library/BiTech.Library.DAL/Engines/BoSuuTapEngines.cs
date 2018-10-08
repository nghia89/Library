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
  public  class BoSuuTapEngines : EntityRepository<BoSuuTap>
    {
        public BoSuuTapEngines(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<BoSuuTap>(tableName);
        }
        public List<BoSuuTap> GetAll()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }

        public BoSuuTap Getname(string code)
        {
            return _DatabaseCollection.Find(x => x.Code.Equals(code)&&x.Status==true).SingleOrDefault();
        }
    }
}
