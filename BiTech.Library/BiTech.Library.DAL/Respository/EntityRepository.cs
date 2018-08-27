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

        public EntityRepository(IDatabase database, string databaseName, string tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _TableName = tableName;
            _DatabaseCollection = _Database.GetCollection<T>(_TableName);
        }

        public virtual T GetById(string Id)
        {
            if (Id?.Length != 24)
                return default(T);
            return _DatabaseCollection.Find(m => m.Id == Id).FirstOrDefault();
        }

        public virtual string Insert(T entity)
        {
            entity.Id = null;
            entity.CreateDateTime = DateTime.Now;

            _DatabaseCollection.InsertOne(entity);
            return entity.Id.ToString();
        }

        public virtual bool Remove(string id)
        {
            return _DatabaseCollection.DeleteOne(mbox => mbox.Id == id).DeletedCount > 0;
        }

        public virtual bool Update(T entity)
        {
            var updateResult = _DatabaseCollection.ReplaceOne<T>(m => m.Id == entity.Id, entity);
            return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;
        }
  

    
    }
}
