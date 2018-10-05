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
        /// Get book by makiemsoat isdelete = false
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        public Sach GetBook_NonDelete_ByMKS(string maKS)
        {
            return _DatabaseCollection.Find(_ => _.MaKiemSoat == maKS && _.IsDeleted == false).FirstOrDefault();
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
        public Sach GetByID_IsDeleteFalse(string id)
        {
            return _DatabaseCollection.Find(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
        }
        #endregion

        public List<Sach> ListName(string keyWord)
        {
            FilterDefinition<Sach> filterDefinition = new BsonDocument();
            var builder = Builders<Sach>.Filter;
            filterDefinition = builder.Where(x => x.TenSach.ToLower().Contains(keyWord.ToLower()) || x.TenSachKhongDau.ToLower().Contains(keyWord.ToLower()));
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
            filterDefinition = filterDefinition & builder.Where(_ => _.IsDeleted == false);
            // Tìm theo UserName
            if (!string.IsNullOrEmpty(KeySearch.Keyword))
            {
                //ToLower chuyễn chữ hoa sang thường
                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                || x.ISBN.ToLower().Contains(KeySearch.Keyword.ToLower()) || x.NamXuatBan.ToLower().Contains(KeySearch.Keyword.ToLower()));
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

        public List<Sach> getPageHighPerformance(KeySearchViewModel KeySearch)
        {

            #region Filter

            FilterDefinition<Sach> filterDefinition = new BsonDocument();

            var builder = Builders<Sach>.Filter;
            //filterDefinition = filterDefinition & builder.Where(_ => _.IsDeleted == false);

            //tìm theo kệ sách
            if (!string.IsNullOrEmpty(KeySearch.KeSach))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.IdKeSach.Equals(KeySearch.KeSach));
            }
            FilterDefinition<Sach> filterDefinition4 = builder.Where(x => false);
            #region tìm kiếm thep opac 5 tiu chí
            if (KeySearch.Keyword != null && KeySearch.ddlLoaiTimKiem0 != null)
            {         
                switch (KeySearch.ddlLoaiTimKiem0)
                {
                    case "title":
                        filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                                        || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                        break;
                    case "author":
                        foreach (var item in KeySearch.ListSachIds)
                        {
                            filterDefinition4 = builder.Where(x => x.Id.Equals(item));
                        }

                        filterDefinition = filterDefinition & filterDefinition4;
                        break;

                    case "isbn":
                        filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword));
                        break;

                    case "place_publication":
                        FilterDefinition<Sach> filterDefinition3 = builder.Where(x => false);
                        foreach (var item in KeySearch.ListIdNXB)
                        {
                            filterDefinition3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                        }
                        filterDefinition = filterDefinition & filterDefinition3;
                        break;

                    case "date_publication":
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword));
                        break;

                    default:

                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword) || x.ISBN.Equals(KeySearch.Keyword)
                        || x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                                        || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                        if (KeySearch.ListIdNXB.Count > 0)
                        {
                            //FilterDefinition<Sach> filterDefinition4 = builder.Where(x => false);
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                filterDefinition4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                            }
                            filterDefinition = filterDefinition | filterDefinition4;
                        }

                        if (KeySearch.ListSachIds.Count > 0)
                        {
                            FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                            foreach (var item in KeySearch.ListSachIds)
                            {
                                filterDefinition2 = builder.Where(x => x.Id.Equals(item));
                            }

                            filterDefinition = filterDefinition | filterDefinition2;
                        }
                        break;
                }
            }

            if (KeySearch.Keyword1 != null && KeySearch.ddlLoaiTimKiem1 != null)
            {
                switch (KeySearch.ddlLoaiTimKiem1)
                {
                    case "title":
                        filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                       || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));
                        break;
                    case "author":
                        foreach (var item in KeySearch.ListSachIds)
                        {
                            filterDefinition4 = builder.Where(x => x.Id.Equals(item));
                        }

                        filterDefinition = filterDefinition & filterDefinition4;
                        break;

                    case "isbn":
                        filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword1));
                        break;

                    case "place_publication":
                        FilterDefinition<Sach> filterDefinition3 = builder.Where(x => false);
                        foreach (var item in KeySearch.ListIdNXB)
                        {
                            filterDefinition3 = filterDefinition3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                        }
                        filterDefinition = filterDefinition & (filterDefinition3);
                        break;

                    case "date_publication":
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1));
                        break;

                    default:

                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1) || x.ISBN.Equals(KeySearch.Keyword1)
                         || x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                         || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));
                        if (KeySearch.ListIdNXB.Count > 0)
                        {
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                filterDefinition4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                            }
                            filterDefinition = filterDefinition | filterDefinition4;
                        }

                        if (KeySearch.ListSachIds.Count > 0)
                        {
                            //string[] Id = KeySearch.ListSachIds.Split(',');

                            FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                            foreach (var item in KeySearch.ListSachIds)
                            {
                                filterDefinition2 = builder.Where(x => x.Id.Equals(item));
                            }

                            filterDefinition = filterDefinition | filterDefinition2;
                        }
                        break;
                }
            }

            if (KeySearch.Keyword2 != null && KeySearch.ddlLoaiTimKiem2 != null)
            {
                switch (KeySearch.ddlLoaiTimKiem2)
                {
                    case "title":
                        filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                        || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword2.ToLower()));
                        break;
                    case "author":
                        foreach (var item in KeySearch.ListSachIds)
                        {
                            filterDefinition4 = builder.Where(x => x.Id.Equals(item));
                        }

                        filterDefinition = filterDefinition & filterDefinition4;
                        break;

                    case "isbn":
                        filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword2));
                        break;

                    case "place_publication":
                        FilterDefinition<Sach> filterDefinition3 = builder.Where(x => false);
                        foreach (var item in KeySearch.ListIdNXB)
                        {
                            filterDefinition3 = filterDefinition3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                        }
                        filterDefinition = filterDefinition & (filterDefinition3);
                        break;

                    case "date_publication":
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2));
                        break;

                    default:
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2) || x.ISBN.Equals(KeySearch.Keyword2)
                       || x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                       || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword2.ToLower()));
                        if (KeySearch.ListIdNXB.Count > 0)
                        {
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                filterDefinition4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                            }
                            filterDefinition = filterDefinition | filterDefinition4;
                        }

                        if (KeySearch.ListSachIds.Count > 0)
                        {
                            //string[] Id = KeySearch.ListSachIds.Split(',');

                            FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                            foreach (var item in KeySearch.ListSachIds)
                            {
                                filterDefinition2 = builder.Where(x => x.Id.Equals(item));
                            }

                            filterDefinition = filterDefinition | filterDefinition2;
                        }
                        break;
                }
            }

            if (KeySearch.Keyword3 != null && KeySearch.ddlLoaiTimKiem3 != null)
            {
                switch (KeySearch.ddlLoaiTimKiem3)
                {
                    case "title":
                        filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                        || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword3.ToLower()));
                        break;
                    case "author":
                        foreach (var item in KeySearch.ListSachIds)
                        {
                            filterDefinition4 = builder.Where(x => x.Id.Equals(item));
                        }

                        filterDefinition = filterDefinition & filterDefinition4;
                        break;

                    case "isbn":
                        filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword3));
                        break;

                    case "place_publication":
                        FilterDefinition<Sach> filterDefinition3 = builder.Where(x => false);
                        foreach (var item in KeySearch.ListIdNXB)
                        {
                            filterDefinition3 = filterDefinition3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                        }
                        filterDefinition = filterDefinition & (filterDefinition3);
                        break;

                    case "date_publication":
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3));
                        break;

                    default:
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3) || x.ISBN.Equals(KeySearch.Keyword3)
                       || x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                       || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword3.ToLower()));
                        if (KeySearch.ListIdNXB.Count > 0)
                        {
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                filterDefinition4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                            }
                            filterDefinition = filterDefinition | filterDefinition4;
                        }

                        if (KeySearch.ListSachIds.Count > 0)
                        {
                            //string[] Id = KeySearch.ListSachIds.Split(',');

                            FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                            foreach (var item in KeySearch.ListSachIds)
                            {
                                filterDefinition2 = builder.Where(x => x.Id.Equals(item));
                            }

                            filterDefinition = filterDefinition2 | filterDefinition2;
                        }
                        break;
                }
            }

            if (KeySearch.Keyword4 != null && KeySearch.ddlLoaiTimKiem4 != null)
            {
                switch (KeySearch.ddlLoaiTimKiem4)
                {
                    case "title":
                        filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                      || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword4.ToLower()));
                        break;
                    case "author":
                        foreach (var item in KeySearch.ListSachIds)
                        {
                            filterDefinition4 = builder.Where(x => x.Id.Equals(item));
                        }

                        filterDefinition = filterDefinition & filterDefinition4;
                        break;

                    case "isbn":
                        filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword4));
                        break;

                    case "place_publication":
                        FilterDefinition<Sach> filterDefinition3 = builder.Where(x => false);
                        foreach (var item in KeySearch.ListIdNXB)
                        {
                            filterDefinition3 = filterDefinition3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                        }
                        filterDefinition = filterDefinition & (filterDefinition3);
                        break;

                    case "date_publication":
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4));
                        break;

                    default:
                        filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4) || x.ISBN.Equals(KeySearch.Keyword4)
                       || x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                       || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword4.ToLower()));
                        if (KeySearch.ListIdNXB.Count > 0)
                        {  
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                filterDefinition4 = filterDefinition | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                            }
                            filterDefinition = filterDefinition4;
                        }

                        if (KeySearch.ListSachIds.Count > 0)
                        {
                            //string[] Id = KeySearch.ListSachIds.Split(',');

                            FilterDefinition<Sach> filterDefinition2 = builder.Where(x => false);

                            foreach (var item in KeySearch.ListSachIds)
                            {
                                filterDefinition2 = builder.Where(x => x.Id.Equals(item));
                            }

                            filterDefinition = filterDefinition | filterDefinition2;
                        }
                        break;
                }
            }
            #endregion



            if (!string.IsNullOrEmpty(KeySearch.KeywordBasic))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.KeywordBasic.ToLower())
                    || x.TenSachKhongDau.ToLower().Contains(KeySearch.KeywordBasic.ToLower()) || x.ISBN.Equals(KeySearch.KeywordBasic)
                    || x.NamXuatBan.Contains(KeySearch.KeywordBasic));
            }
            //if (!string.IsNullOrEmpty(KeySearch.TheLoaiSach))
            //{
            //    filterDefinition = filterDefinition & builder.Where(x => x.IdTheLoai.ToLower().Contains(KeySearch.TheLoaiSach.ToLower()));
            //}
            //if (KeySearch.ListSachIds.Count > 0)
            //{
            //    //string[] Id = KeySearch.ListSachIds.Split(',');

            //    foreach (var item in KeySearch.ListSachIds)
            //    {
            //        filterDefinition2 = filterDefinition2 | builder.Where(x => x.Id.Equals(item));
            //    }

            //    filterDefinition = filterDefinition & (filterDefinition2);
            //}
            ////if (!string.IsNullOrEmpty(KeySearch.ListSachIds))
            ////{
            ////    filterDefinition = filterDefinition & builder.Where(x => x.Id.Equals(KeySearch.ListSachIds));
            ////}
            #endregion           
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }
    }
}
