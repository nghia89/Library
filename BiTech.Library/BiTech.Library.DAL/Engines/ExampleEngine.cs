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
    public class ExampleEngine : EntityRepository<Example>
    {
        public ExampleEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<Example>(tableName);
        }

        public List<Example> getAllExample()
        {
            return _DatabaseCollection.Find(_ => _.Data == "").ToList();
        }
    }
}
