using BiTech.Library.DTO;
using MongoDB.Driver;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BiTech.Library.DAL.Respository
{
    public class EntityRepository<T> : IEntityRepository<T> where T : IModel
    {
        /// <summary>
        /// Table name in Database
        /// </summary>
        private string _TableName { get; set; }

        public IMongoCollection<T> _DatabaseCollection { get; set; }

        public IMongoDatabase _Database { get; set; }

        public EntityRepository(IDatabase database, string tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _TableName = tableName;
            _DatabaseCollection = _Database.GetCollection<T>(_TableName);
        }

        public T GetById(string id)
        {
            if (id?.Length != 24)
                return default(T);
            return _DatabaseCollection.Find(m => m.Id == id).FirstOrDefault();
        }

        public string Insert(T entity)
        {
            entity.Id = null;
            entity.CreateDateTime = DateTime.Now;

            _DatabaseCollection.InsertOne(entity);
            return entity.Id.ToString();
        }

        public bool Remove(string id)
        {
            return _DatabaseCollection.DeleteOne(mbox => mbox.Id == id).DeletedCount > 0;
        }

        public bool Update(T entity)
        {
            var updateResult = _DatabaseCollection.ReplaceOne<T>(m => m.Id == entity.Id, entity);
            return (updateResult.ModifiedCount > 0);
        }
  

    
    }
}
