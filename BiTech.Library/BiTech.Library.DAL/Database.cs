using MongoDB.Driver;

namespace BiTech.Library.DAL
{
    /// <summary>
    /// Dùng để kết nối tới Database của MongoDB
    /// </summary>
    public class Database : IDatabase
    {
        public static IMongoClient _client;
        //public static IMongoDatabase _database;
        public string ConnectionString { get; set; }
        //public string DatabaseName { get; set; }

        public Database()
        {

        }

        public Database(string connectionString)
        {
            if (_client == null)
            {
                _client = new MongoClient(connectionString);
                Mongo.Migration.Services.Initializers.MongoMigration.Initialize();
            }
            
            //if (_database == null)
            //{
            //    _database = _client.GetDatabase(databaseName);
            //}
        }


        public void DropDatabase(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _client.DropDatabase(databaseName);
        }

        public object GetConnection(string databaseName)
        {
            return _client.GetDatabase(databaseName);
        }
    }
}
