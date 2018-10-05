using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class TacGiaEngine : EntityRepository<TacGia>
    {
        public TacGiaEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<TacGia>(tableName);
        }

        public List<TacGia> GetAllTacGia()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
        public List<TacGia> GetByFindName(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x=>x.TenTacGia==Name).ToList();

        }
        public TacGia GetByNameId(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.TenTacGia == Name).FirstOrDefault();

        }
        public List<TacGia> FindTacGia(string q)
        {
            return _DatabaseCollection.AsQueryable().Where(delegate (TacGia c)
            {
                if (ConvertToUnSign(c.TenTacGia.ToLower()).Contains(ConvertToUnSign(q.ToLower())))
                    return true;
                else
                    return false;
            }).ToList(); //(name => ConvertToUnSign(name.TenTacGia.ToLower()).Contains(ConvertToUnSign(q.ToLower()))).ToList();
        }
        public string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        public List<TacGia> GetByListName(string Name)
        {
            FilterDefinition<TacGia> filterDefinition = new BsonDocument();
            var builder = Builders<TacGia>.Filter;
            filterDefinition = builder.Where(x => x.TenTacGia.ToLower().Contains(Name.ToLower()));
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        #region Tai
        public TacGia GetByTenTacGia(string tenTacGia)
        {
            return _DatabaseCollection.Find(_ => _.TenTacGia.ToLower() == tenTacGia.ToLower()).FirstOrDefault();
        }
        #endregion

    }
}
