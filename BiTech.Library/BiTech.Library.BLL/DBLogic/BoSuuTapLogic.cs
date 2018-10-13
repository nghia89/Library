using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class BoSuuTapLogic : BaseLogic
    {
        BoSuuTapEngines _BoSuuTapEngines;

        public BoSuuTapLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _BoSuuTapEngines = new BoSuuTapEngines(database, databaseName, "BoSuuTap");
            
            // Kiểm tra trong bộ sưu tập có j chưa, nếu chưa thì khởi tạo
            if (_BoSuuTapEngines.GetAll().Count() <= 0)
            {
                List<BoSuuTap> AddList = new List<BoSuuTap>()
                {
                    new BoSuuTap(){Name="Sách",Code="sach",Status=true},
                    new BoSuuTap(){Name="Ấn phẩm định kỳ",Code="an-pham-dinh-ky",Status=false},
                    new BoSuuTap(){Name="Bài trích",Code="bai-trich",Status=false},
                    new BoSuuTap(){Name="Băng từ",Code="bang-tu",Status=false},
                    new BoSuuTap(){Name="CD-bộ",Code="cd-bo",Status=false},
                    new BoSuuTap(){Name="CD-ROM",Code="cd-rom",Status=false},
                    new BoSuuTap(){Name="CD-tập",Code="cd-tap",Status=false},
                    new BoSuuTap(){Name="Luận án",Code="luan-an",Status=false},
                    new BoSuuTap(){Name="Luận án địa chí",Code="lan-an-dia-chi",Status=false},
                    new BoSuuTap(){Name="Sách bộ",Code="sach-bo",Status=false},
                    new BoSuuTap(){Name="Sách tập",Code="sach-tap",Status=false},
                    new BoSuuTap(){Name="Tranh thiếu nhi",Code="tranh-thieu-nhi",Status=false}
                };

                foreach (var item in AddList)
                {
                    BoSuuTap BST = new BoSuuTap()
                    {
                        Name = item.Name,
                        Code = item.Code,
                        Status = item.Status,
                        CreateDateTime = DateTime.Now,
                    };
                    Insert(BST);
                }
            }
        }

        public List<BoSuuTap> GetAll()
        {
            return _BoSuuTapEngines.GetAll();
        }

        public BoSuuTap FindById(string Id)
        {
            return _BoSuuTapEngines.GetById(Id);
        }

        public string Insert(BoSuuTap entity)
        {
            return _BoSuuTapEngines.Insert(entity);
        }

        public bool Update(BoSuuTap Id)
        {
            return _BoSuuTapEngines.Update(Id);
        }

        public virtual bool Delete(string Id)
        {
            return _BoSuuTapEngines.Remove(Id);
        }
        public BoSuuTap GetName(string name)
        {
            var Getname = _BoSuuTapEngines.Getname(name);
            return _BoSuuTapEngines.GetById(Getname.Id);
        }

        public void UpdateDBVersion()
        {
            _BoSuuTapEngines.UpdateDBVersion();
        }
    }
}
