using System;
using MongoDB.Driver;

namespace BiTech.Library.DAL
{
    public class StoreDatabase : IDatabase
    {
        public static IMongoClient _client;
        public static IMongoDatabase _database;
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public StoreDatabase()
        {

        }

        public StoreDatabase(string connectionString)
        {
            if (_client == null)
            {
                _client = new MongoClient(connectionString);
            }
        }

        public StoreDatabase(string connectionString, string databaseName)
        {
            if (_client == null)
            {
                _client = new MongoClient(connectionString);
            }

            if (_database == null)
            {
                _database = _client.GetDatabase(databaseName);
            }
        }

        public void DropDatabase(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _client.DropDatabase(databaseName);
        }

        public object GetConnection()
        {
            return _database;
        }

        public object GetConnection(string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}