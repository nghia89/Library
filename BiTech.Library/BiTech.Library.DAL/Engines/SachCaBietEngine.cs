using BiTech.Library.Common;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class SachCaBietEngine : EntityRepository<SachCaBiet>
    {
        public SachCaBietEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SachCaBiet>(tableName);
        }
    }
}
