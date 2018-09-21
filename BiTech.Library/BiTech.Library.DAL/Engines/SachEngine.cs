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
    public class SachEngine : EntityRepository<Sach>
    {
        public SachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<Sach>(tableName);
        }

        public override string Insert(Sach entity)
        {
            entity.Id = null;
            entity.CreateDateTime = DateTime.Now;

            _DatabaseCollection.InsertOne(entity);
            return entity.Id.ToString();
        }

		#region vinh
		/// <summary>
		/// Get book by makiemsoat hoặc ISBN isdelete = false
		/// </summary>
		/// <param name="idBook"></param>
		/// <returns></returns>
		public Sach GetBook_NonDelete_ByMKS(string maKS)
		{
            //return _DatabaseCollection.Find(_ => _.MaKiemSoat == maKS && _.IsDeleted == false).FirstOrDefault();
            return _DatabaseCollection.Find(_ => _.ISBN == maKS || _.MaKiemSoat == maKS && _.IsDeleted == false).FirstOrDefault();
        }
		public List<Sach> GetAll_NonDelete()
		{
			return _DatabaseCollection.Find(_ => _.IsDeleted == false).ToList();
		}
		public Sach GetByMaKiemSoat(string MaKS)
        {
            return _DatabaseCollection.Find(x => x.MaKiemSoat == MaKS).FirstOrDefault();
        }
        #endregion

        #region Phong

        public Sach GetByMaKiemSoatorISBN(string mastring)
        {
            return _DatabaseCollection.Find(x => x.MaKiemSoat == mastring || x.ISBN == mastring).FirstOrDefault();
        }

        public Sach GetByID_IsDeleteFalse(string id)
        {
            return _DatabaseCollection.Find(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
        }
        #endregion

        public List<Sach> ListName(string keyWord)
        {
            FilterDefinition<Sach> filterDefinition = new BsonDocument();
            var builder = Builders<Sach>.Filter;
            filterDefinition=builder.Where(x => x.TenSach.ToLower().Contains(keyWord.ToLower()) || x.TenSachKhongDau.ToLower().Contains(keyWord.ToLower()));
            filterDefinition = filterDefinition & builder.Where(_ => _.IsDeleted == false);
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        public List<Sach> GetAllSach()
        {
            return _DatabaseCollection.Find(x => x.IsDeleted == false).ToList();
        }
        public List<Sach> GetAll()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
        public List<Sach> GetDatetime(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            return _DatabaseCollection.Find(x => x.CreateDateTime <= lastDayOfMonth && x.CreateDateTime >= firstDayOfMonth).ToList();
        }
        public List<Sach> getPageSach(KeySearchViewModel KeySearch)
        {

            #region Filter

            FilterDefinition<Sach> filterDefinition = new BsonDocument();
            var builder = Builders<Sach>.Filter;
            //lấy những dòng chưa bị xoá
            filterDefinition = filterDefinition & builder.Where(_=>_.IsDeleted == false);
            // Tìm theo UserName
            if (!string.IsNullOrEmpty(KeySearch.Keyword))
            {
                //ToLower chuyễn chữ hoa sang thường
                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                || x.MaKiemSoat.ToLower().Contains(KeySearch.Keyword.ToLower())
                || x.ISBN.ToLower().Contains(KeySearch.Keyword.ToLower()) 
                || x.NamXuatBan.ToLower().Contains(KeySearch.Keyword.ToLower())
                );
            }
            if (!string.IsNullOrEmpty(KeySearch.TenNXB))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.IdNhaXuatBan.ToLower().Contains(KeySearch.TenNXB.ToLower()));
            }
            if (!string.IsNullOrEmpty(KeySearch.TheLoaiSach))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.IdTheLoai.ToLower().Contains(KeySearch.TheLoaiSach.ToLower()));
            }
            if (KeySearch.ListSachIds.Count > 0)
            {
                //string[] Id = KeySearch.ListSachIds.Split(',');

                FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                foreach (var item in KeySearch.ListSachIds)
                {
                    filterDefinition2 = filterDefinition2 | builder.Where(x => x.Id.Equals(item));
                }

                filterDefinition = filterDefinition & (filterDefinition2);
            }
            //if (!string.IsNullOrEmpty(KeySearch.ListSachIds))
            //{
            //    filterDefinition = filterDefinition & builder.Where(x => x.Id.Equals(KeySearch.ListSachIds));
            //}
            #endregion

            return _DatabaseCollection.Find(filterDefinition).ToList();
        }
    }
}
