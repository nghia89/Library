using BiTech.Library.Common;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

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


        //#region vinh
        ///// <summary>
        ///// Get book by makiemsoat hoặc ISBN isdelete = false
        ///// </summary>
        ///// <param name="idBook"></param>
        ///// <returns></returns>
        //public Sach GetBook_NonDelete_ByMKS(string maKS)
        //{
        //          //return _DatabaseCollection.Find(_ => _.MaKiemSoat == maKS && _.IsDeleted == false).FirstOrDefault();
        //          return _DatabaseCollection.Find(_ => _.ISBN == maKS || _.MaKiemSoat == maKS && _.IsDeleted == false).FirstOrDefault();
        //      }
        //public List<Sach> GetAll_NonDelete()
        //{
        //	return _DatabaseCollection.Find(_ => _.IsDeleted == false).ToList();
        //}
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

        public List<Sach> getPageHighPerformance(KeySearchViewModel KeySearch)
        {

            #region Filter

            FilterDefinition<Sach> filterDefinition = new BsonDocument();

            var builder = Builders<Sach>.Filter;
            //filterDefinition = filterDefinition & builder.Where(_ => _.IsDeleted == false);

            //Bộ sưu tập
            if (!string.IsNullOrEmpty(KeySearch.BoSuuTap))
            {
                if (KeySearch.BoSuuTap == "Any")
                    filterDefinition = filterDefinition & builder.Where(x => x.IsDeleted==false);
                else
                    filterDefinition = filterDefinition & builder.Where(x => x.IdBoSuuTap.Equals(KeySearch.BoSuuTap));
            }
            //tìm theo kệ sách
            if (!string.IsNullOrEmpty(KeySearch.KeSach))
            {
                if (KeySearch.KeSach == "Any")
                    filterDefinition = filterDefinition & builder.Where(x => x.IsDeleted == false);
                else
                    filterDefinition = filterDefinition & builder.Where(x => x.IdKeSach.Equals(KeySearch.KeSach));
            }

            FilterDefinition<Sach> filterDefinitionTacGia = null;
            FilterDefinition<Sach> filterDefinitionTacGia1 = null;
            FilterDefinition<Sach> filterDefinitionTacGia2 = null;
            FilterDefinition<Sach> filterDefinitionTacGia3 = null;
            FilterDefinition<Sach> filterDefinitionTacGia4 = null;

            FilterDefinition<Sach> filterDefinitionNXB = null;
            FilterDefinition<Sach> filterDefinitionNXB1 = null;
            FilterDefinition<Sach> filterDefinitionNXB2 = null;
            FilterDefinition<Sach> filterDefinitionNXB3 = null;
            FilterDefinition<Sach> filterDefinitionNXB4 = null;


            #region tìm kiếm thep opac 5 tiu chí
            if (KeySearch.Keyword != null && KeySearch.ddlLoaiTimKiem0 != null)
            {
                switch (KeySearch.ddlLoaiTimKiem0)
                {
                    case "title":
                        if (KeySearch.Condition == "Contains")
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                                          || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                        }
                        else
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword.ToLower()
                                          || x.TenSachKhongDau.ToLower() == KeySearch.Keyword.ToLower());
                        }
                        break;
                    case "author":
                        if (KeySearch.ListSachIds.Count() > 0)
                        {
                            foreach (var item in KeySearch.ListSachIds)
                            {
                                if (filterDefinitionTacGia == null)
                                {
                                    filterDefinitionTacGia = builder.Where(x => x.Id == item);
                                }
                                else
                                {
                                    filterDefinitionTacGia =filterDefinitionTacGia | builder.Where(x => x.Id == item);
                                }
                            }
                            filterDefinition = filterDefinition & filterDefinitionTacGia;
                        }
                        else
                            filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword));

                        break;

                    case "isbn":
                        if (KeySearch.Condition == "Contains")
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Contains(KeySearch.Keyword));
                        }
                        else
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword));
                        }
                        break;

                    case "place_publication":
                        if (KeySearch.ListIdNXB.Count() > 0)
                        {
                            foreach (var item in KeySearch.ListIdNXB)
                            {
                                if (filterDefinitionNXB == null)
                                {
                                    filterDefinitionNXB = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                }
                                else
                                {
                                    filterDefinitionNXB = filterDefinitionNXB | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                }
                            }
                            filterDefinition = filterDefinition & filterDefinitionNXB;
                        }
                        else
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword));
                        }
                        break;

                    case "date_publication":
                        if (KeySearch.Condition == "Contains")
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword));
                        }
                        else
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword));
                        }
                        break;

                    default:
                        if (KeySearch.Condition == "Contains")
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword) || x.ISBN.Contains(KeySearch.Keyword)
                                          || x.TenSach.ToLower().Contains(KeySearch.Keyword.ToLower())
                                          || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            if (KeySearch.ListIdNXB.Count > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB)
                                {
                                    if (filterDefinitionNXB == null)
                                    {
                                        filterDefinitionNXB = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB = filterDefinitionNXB | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionNXB;
                            }

                            if (KeySearch.ListSachIds.Count > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds)
                                {
                                    if (filterDefinitionTacGia == null)
                                    {
                                        filterDefinitionTacGia = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia =  filterDefinitionTacGia | builder.Where(x => x.Id == item);
                                    }
                                }

                                filterDefinition = filterDefinition | filterDefinitionTacGia;
                            }
                        }
                        else
                        {
                            filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword)
                            || x.ISBN.Equals(KeySearch.Keyword)
                            || x.TenSach.ToLower() == KeySearch.Keyword.ToLower()
                                       || x.TenSachKhongDau.ToLower() == KeySearch.Keyword.ToLower());
                            if (KeySearch.ListIdNXB.Count > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB)
                                {
                                    if (filterDefinitionNXB == null)
                                    {
                                        filterDefinitionNXB = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB = filterDefinitionNXB | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB;
                            }

                            if (KeySearch.ListSachIds.Count > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds)
                                {
                                    if (filterDefinitionTacGia == null)
                                    {
                                        filterDefinitionTacGia = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia = filterDefinitionTacGia | builder.Where(x => x.Id == item);
                                    }
                                }

                                filterDefinition = filterDefinition & filterDefinitionTacGia;
                            }
                        }
                        break;
                }
            }

            if (KeySearch.Keyword1 != null && KeySearch.ddlLoaiTimKiem1 != null)
            {  
                if(KeySearch.dlOperator1=="and")
                {
                    switch (KeySearch.ddlLoaiTimKiem1)
                    {
                        case "title":
                            if (KeySearch.Condition == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword1.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword1.ToLower());
                            }
                            break;
                        case "author":

                            if (KeySearch.ListSachIds1.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds1)
                                {
                                    if (filterDefinitionTacGia1 == null)
                                    {
                                        filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionTacGia1;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword1));
                            break;

                        case "isbn":
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Contains(KeySearch.Keyword1));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword1));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB1.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB1)
                                {
                                    if (filterDefinitionNXB1 == null)
                                    {
                                        filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB1 = filterDefinitionNXB1 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB1;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword1));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword1));
                            }
                            break;

                        default:
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1) || x.ISBN.Contains(KeySearch.Keyword1)
                              || x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));

                                if (KeySearch.ListIdNXB1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB1)
                                    {
                                        if (filterDefinitionNXB1 == null)
                                        {
                                            filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB1 = filterDefinitionNXB1 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB1;
                                }

                                if (KeySearch.ListSachIds1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds1)
                                    {
                                        if (filterDefinitionTacGia1 == null)
                                        {
                                            filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionTacGia1;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword1) || x.ISBN.Equals(KeySearch.Keyword1)
                                             || x.TenSach.ToLower() == KeySearch.Keyword1.ToLower()
                                             || x.TenSachKhongDau.ToLower() == KeySearch.Keyword1.ToLower());

                                if (KeySearch.ListIdNXB1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB1)
                                    {
                                        if (filterDefinitionNXB1 == null)
                                        {
                                            filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB1 = filterDefinitionNXB1 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB1;
                                }

                                if (KeySearch.ListSachIds1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds1)
                                    {
                                        if (filterDefinitionTacGia1 == null)
                                        {
                                            filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionTacGia1;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (KeySearch.ddlLoaiTimKiem1)
                    {
                        case "title":
                            if (KeySearch.Condition == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword1.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword1.ToLower());
                            }
                            break;
                        case "author":

                            if (KeySearch.ListSachIds1.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds1)
                                {
                                    if (filterDefinitionTacGia1 == null)
                                    {
                                        filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionTacGia1;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword1));
                            break;

                        case "isbn":
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Contains(KeySearch.Keyword1));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Equals(KeySearch.Keyword1));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB1.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB1)
                                {
                                    if (filterDefinitionNXB1 == null)
                                    {
                                        filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB1 = filterDefinitionNXB1 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionNXB1;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword1));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword1));
                            }
                            break;

                        default:
                            if (KeySearch.Condition1 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword1) || x.ISBN.Contains(KeySearch.Keyword1)
                              || x.TenSach.ToLower().Contains(KeySearch.Keyword1.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword1.ToLower()));

                                if (KeySearch.ListIdNXB1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB1)
                                    {
                                        if (filterDefinitionNXB1 == null)
                                        {
                                            filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB1 = filterDefinitionNXB1 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB1;
                                }

                                if (KeySearch.ListSachIds1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds1)
                                    {
                                        if (filterDefinitionTacGia1 == null)
                                        {
                                            filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionTacGia1;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword1) || x.ISBN.Equals(KeySearch.Keyword1)
                                             || x.TenSach.ToLower() == KeySearch.Keyword1.ToLower()
                                             || x.TenSachKhongDau.ToLower() == KeySearch.Keyword1.ToLower());

                                if (KeySearch.ListIdNXB1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB1)
                                    {
                                        if (filterDefinitionNXB1 == null)
                                        {
                                            filterDefinitionNXB1 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB1 = filterDefinitionNXB1 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB1;
                                }

                                if (KeySearch.ListSachIds1.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds1)
                                    {
                                        if (filterDefinitionTacGia1 == null)
                                        {
                                            filterDefinitionTacGia1 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia1 = filterDefinitionTacGia1 | builder.Where(x => x.Id == item);
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionTacGia1;
                                }
                            }
                            break;
                    }
                }

            }

            if (KeySearch.Keyword2 != null && KeySearch.ddlLoaiTimKiem2 != null)
            {
                if (KeySearch.dlOperator2 == "and")
                {
                    switch (KeySearch.ddlLoaiTimKiem2)
                    {
                        case "title":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword2.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword2.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds2.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds2)
                                {
                                    if (filterDefinitionTacGia2 == null)
                                    {
                                        filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionTacGia2;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword2));
                            break;

                        case "isbn":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Contains(KeySearch.Keyword2));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword2));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB2.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB2)
                                {
                                    if (filterDefinitionNXB2 == null)
                                    {
                                        filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB2 = filterDefinitionNXB2 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB2;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword2));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword2));
                            }
                            break;

                        default:
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2) || x.ISBN.Contains(KeySearch.Keyword2)
                                           || x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                           || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword2.ToLower()));

                                if (KeySearch.ListIdNXB2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB2)
                                    {
                                        if (filterDefinitionNXB2 == null)
                                        {
                                            filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB2 = filterDefinitionNXB2 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB2;
                                }

                                if (KeySearch.ListSachIds2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds2)
                                    {
                                        if (filterDefinitionTacGia2 == null)
                                        {
                                            filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia2;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword2) || x.ISBN.Equals(KeySearch.Keyword2)
                                          || x.TenSach.ToLower() == KeySearch.Keyword2.ToLower()
                                          || x.TenSachKhongDau.ToLower() == KeySearch.Keyword2.ToLower());
                                if (KeySearch.ListIdNXB2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB2)
                                    {
                                        if (filterDefinitionNXB2 == null)
                                        {
                                            filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB2 = filterDefinitionNXB2 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB2;
                                }

                                if (KeySearch.ListSachIds2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds2)
                                    {
                                        if (filterDefinitionTacGia2 == null)
                                        {
                                            filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia2;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (KeySearch.ddlLoaiTimKiem2)
                    {
                        case "title":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword2.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword2.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds2.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds2)
                                {
                                    if (filterDefinitionTacGia2 == null)
                                    {
                                        filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionTacGia2;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword2));
                            break;

                        case "isbn":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Contains(KeySearch.Keyword2));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Equals(KeySearch.Keyword2));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB2.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB2)
                                {
                                    if (filterDefinitionNXB2 == null)
                                    {
                                        filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB2 = filterDefinitionNXB2 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionNXB2;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword2));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword2));
                            }
                            break;

                        default:
                            if (KeySearch.Condition2 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword2) || x.ISBN.Contains(KeySearch.Keyword2)
                                           || x.TenSach.ToLower().Contains(KeySearch.Keyword2.ToLower())
                                           || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword2.ToLower()));

                                if (KeySearch.ListIdNXB2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB2)
                                    {
                                        if (filterDefinitionNXB2 == null)
                                        {
                                            filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB2 = filterDefinitionNXB2 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB2;
                                }

                                if (KeySearch.ListSachIds2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds2)
                                    {
                                        if (filterDefinitionTacGia2 == null)
                                        {
                                            filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia2;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword2) || x.ISBN.Equals(KeySearch.Keyword2)
                                          || x.TenSach.ToLower() == KeySearch.Keyword2.ToLower()
                                          || x.TenSachKhongDau.ToLower() == KeySearch.Keyword2.ToLower());
                                if (KeySearch.ListIdNXB2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB2)
                                    {
                                        if (filterDefinitionNXB2 == null)
                                        {
                                            filterDefinitionNXB2 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB2 = filterDefinitionNXB2 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB2;
                                }

                                if (KeySearch.ListSachIds2.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds2)
                                    {
                                        if (filterDefinitionTacGia2 == null)
                                        {
                                            filterDefinitionTacGia2 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia2 = filterDefinitionTacGia2 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia2;
                                }
                            }
                            break;
                    }
                }
                    
            }

            if (KeySearch.Keyword3 != null && KeySearch.ddlLoaiTimKiem3 != null)
            {
                if (KeySearch.dlOperator3 == "and")
                {
                    switch (KeySearch.ddlLoaiTimKiem3)
                    {
                        case "title":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword3.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword3.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds3.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds3)
                                {
                                    if (filterDefinitionTacGia3 == null)
                                    {
                                        filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionTacGia3;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword3));
                            break;

                        case "isbn":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Contains(KeySearch.Keyword3));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword3));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB3.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB3)
                                {
                                    if (filterDefinitionNXB3 == null)
                                    {
                                        filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB3 = filterDefinitionNXB3 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB3;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword3));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword3));
                            }
                            break;

                        default:
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3) || x.ISBN.Contains(KeySearch.Keyword3)
                          || x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                          || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword3.ToLower()));

                                if (KeySearch.ListIdNXB3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB3)
                                    {
                                        if (filterDefinitionNXB3 == null)
                                        {
                                            filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB3 = filterDefinitionNXB3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB3;
                                }

                                if (KeySearch.ListSachIds3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds3)
                                    {
                                        if (filterDefinitionTacGia3 == null)
                                        {
                                            filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia3;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword3) || x.ISBN.Equals(KeySearch.Keyword3)
                                                  || x.TenSach.ToLower() == KeySearch.Keyword3.ToLower()
                                                  || x.TenSachKhongDau.ToLower() == KeySearch.Keyword3.ToLower());

                                if (KeySearch.ListIdNXB3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB3)
                                    {
                                        if (filterDefinitionNXB3 == null)
                                        {
                                            filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB3 = filterDefinitionNXB3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB3;
                                }

                                if (KeySearch.ListSachIds3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds3)
                                    {
                                        if (filterDefinitionTacGia3 == null)
                                        {
                                            filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia3;
                                }
                            }

                            break;
                    }
                }
                else
                {
                    switch (KeySearch.ddlLoaiTimKiem3)
                    {
                        case "title":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword3.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword3.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds3.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds3)
                                {
                                    if (filterDefinitionTacGia3 == null)
                                    {
                                        filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionTacGia3;
                            }
                            else
                                filterDefinition = filterDefinition | builder.Where(x => x.Id.Contains(KeySearch.Keyword3));
                            break;

                        case "isbn":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Contains(KeySearch.Keyword3));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Equals(KeySearch.Keyword3));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB3.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB3)
                                {
                                    if (filterDefinitionNXB3 == null)
                                    {
                                        filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB3 = filterDefinitionNXB3 & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB3;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword3));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword3));
                            }
                            break;

                        default:
                            if (KeySearch.Condition3 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword3) || x.ISBN.Contains(KeySearch.Keyword3)
                          || x.TenSach.ToLower().Contains(KeySearch.Keyword3.ToLower())
                                          || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword3.ToLower()));

                                if (KeySearch.ListIdNXB3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB3)
                                    {
                                        if (filterDefinitionNXB3 == null)
                                        {
                                            filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB3 = filterDefinitionNXB3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB3;
                                }

                                if (KeySearch.ListSachIds3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds3)
                                    {
                                        if (filterDefinitionTacGia3 == null)
                                        {
                                            filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia3;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword3) || x.ISBN.Equals(KeySearch.Keyword3)
                                                  || x.TenSach.ToLower() == KeySearch.Keyword3.ToLower()
                                                  || x.TenSachKhongDau.ToLower() == KeySearch.Keyword3.ToLower());

                                if (KeySearch.ListIdNXB3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB3)
                                    {
                                        if (filterDefinitionNXB3 == null)
                                        {
                                            filterDefinitionNXB3 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB3 = filterDefinitionNXB3 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB3;
                                }

                                if (KeySearch.ListSachIds3.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds3)
                                    {
                                        if (filterDefinitionTacGia3 == null)
                                        {
                                            filterDefinitionTacGia3 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia3 = filterDefinitionTacGia3 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia3;
                                }
                            }

                            break;
                    }
                }
                
            }

            if (KeySearch.Keyword4 != null && KeySearch.ddlLoaiTimKiem4 != null)
            {
                if (KeySearch.dlOperator4 == "and")
                {
                    switch (KeySearch.ddlLoaiTimKiem4)
                    {
                        case "title":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword4.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword4.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds4.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds4)
                                {
                                    if (filterDefinitionTacGia4 == null)
                                    {
                                        filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionTacGia4;
                            }
                            else
                                filterDefinition = filterDefinition & builder.Where(x => x.Id.Contains(KeySearch.Keyword4));
                            break;

                        case "isbn":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Contains(KeySearch.Keyword4));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.ISBN.Equals(KeySearch.Keyword4));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB4.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB4)
                                {
                                    if (filterDefinitionNXB4 == null)
                                    {
                                        filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB4 = filterDefinitionNXB & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition & filterDefinitionNXB4;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword4));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4));
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword4));
                            }
                            break;

                        default:
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4) || x.ISBN.Contains(KeySearch.Keyword4)
                            || x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                            || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword4.ToLower()));
                                if (KeySearch.ListIdNXB4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB4)
                                    {
                                        if (filterDefinitionNXB4 == null)
                                        {
                                            filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB4 = filterDefinitionNXB4 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB4;
                                }

                                if (KeySearch.ListSachIds4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds4)
                                    {
                                        if (filterDefinitionTacGia4 == null)
                                        {
                                            filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia4;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword4) || x.ISBN.Equals(KeySearch.Keyword4)
                                                     || x.TenSach.ToLower() == KeySearch.Keyword4.ToLower()
                                                     || x.TenSachKhongDau.ToLower() == KeySearch.Keyword4.ToLower());
                                if (KeySearch.ListIdNXB4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB4)
                                    {
                                        if (filterDefinitionNXB4 == null)
                                        {
                                            filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB4 = filterDefinitionNXB4 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB4;
                                }

                                if (KeySearch.ListSachIds4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds4)
                                    {
                                        if (filterDefinitionTacGia4 == null)
                                        {
                                            filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia4;
                                }
                            }

                            break;
                    }
                }
                else
                {
                    switch (KeySearch.ddlLoaiTimKiem4)
                    {
                        case "title":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                              || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword.ToLower()));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.TenSach.ToLower() == KeySearch.Keyword4.ToLower()
                                              || x.TenSachKhongDau.ToLower() == KeySearch.Keyword4.ToLower());
                            }
                            break;
                        case "author":
                            if (KeySearch.ListSachIds4.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListSachIds4)
                                {
                                    if (filterDefinitionTacGia4 == null)
                                    {
                                        filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                    }
                                    else
                                    {
                                        filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionTacGia4;
                            }
                            else
                                filterDefinition = filterDefinition | builder.Where(x => x.Id.Contains(KeySearch.Keyword4));
                            break;

                        case "isbn":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Contains(KeySearch.Keyword4));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.ISBN.Equals(KeySearch.Keyword4));
                            }
                            break;

                        case "place_publication":
                            if (KeySearch.ListIdNXB4.Count() > 0)
                            {
                                foreach (var item in KeySearch.ListIdNXB4)
                                {
                                    if (filterDefinitionNXB4 == null)
                                    {
                                        filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                    else
                                    {
                                        filterDefinitionNXB4 = filterDefinitionNXB & builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                    }
                                }
                                filterDefinition = filterDefinition | filterDefinitionNXB4;
                            }
                            else
                            {
                                filterDefinition = builder.Where(x => x.IdNhaXuatBan.Equals(KeySearch.Keyword4));
                            }
                            break;

                        case "date_publication":
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4));
                            }
                            else
                            {
                                filterDefinition = filterDefinition | builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword4));
                            }
                            break;

                        default:
                            if (KeySearch.Condition4 == "Contains")
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Contains(KeySearch.Keyword4) || x.ISBN.Contains(KeySearch.Keyword4)
                            || x.TenSach.ToLower().Contains(KeySearch.Keyword4.ToLower())
                                            || x.TenSachKhongDau.ToLower().Contains(KeySearch.Keyword4.ToLower()));
                                if (KeySearch.ListIdNXB4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB4)
                                    {
                                        if (filterDefinitionNXB4 == null)
                                        {
                                            filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB4 = filterDefinitionNXB4 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB4;
                                }

                                if (KeySearch.ListSachIds4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds4)
                                    {
                                        if (filterDefinitionTacGia4 == null)
                                        {
                                            filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia4;
                                }
                            }
                            else
                            {
                                filterDefinition = filterDefinition & builder.Where(x => x.NamXuatBan.Equals(KeySearch.Keyword4) || x.ISBN.Equals(KeySearch.Keyword4)
                                                     || x.TenSach.ToLower() == KeySearch.Keyword4.ToLower()
                                                     || x.TenSachKhongDau.ToLower() == KeySearch.Keyword4.ToLower());
                                if (KeySearch.ListIdNXB4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListIdNXB4)
                                    {
                                        if (filterDefinitionNXB4 == null)
                                        {
                                            filterDefinitionNXB4 = builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                        else
                                        {
                                            filterDefinitionNXB4 = filterDefinitionNXB4 | builder.Where(x => x.IdNhaXuatBan.Equals(item));
                                        }
                                    }
                                    filterDefinition = filterDefinition | filterDefinitionNXB4;
                                }

                                if (KeySearch.ListSachIds4.Count > 0)
                                {
                                    foreach (var item in KeySearch.ListSachIds4)
                                    {
                                        if (filterDefinitionTacGia4 == null)
                                        {
                                            filterDefinitionTacGia4 = builder.Where(x => x.Id == item);
                                        }
                                        else
                                        {
                                            filterDefinitionTacGia4 = filterDefinitionTacGia4 | builder.Where(x => x.Id == item);
                                        }
                                    }

                                    filterDefinition = filterDefinition | filterDefinitionTacGia4;
                                }
                            }

                            break;
                    }
                }
                  
            }
            #endregion

            //search cơ bản
            if (!string.IsNullOrEmpty(KeySearch.KeywordBasic))
            {
                filterDefinition = filterDefinition & builder.Where(x => x.TenSach.ToLower().Contains(KeySearch.KeywordBasic.ToLower())
                    || x.TenSachKhongDau.ToLower().Contains(KeySearch.KeywordBasic.ToLower()) || x.ISBN.Equals(KeySearch.KeywordBasic)
                    || x.NamXuatBan.Contains(KeySearch.KeywordBasic));
            }
            #endregion
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }
    }
}

