using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class BoSuuTapController : BaseController
    {
        List<BoSuuTapViewModel> AddList = new List<BoSuuTapViewModel>()
        {
            new BoSuuTapViewModel(){Name="Sách",Code="sach",Status=true},
            new BoSuuTapViewModel(){Name="Ấn phẩm định kỳ",Code="an-pham-dinh-ky",Status=false},
             new BoSuuTapViewModel(){Name="Bài trích",Code="bai-trich",Status=false},
            new BoSuuTapViewModel(){Name="Băng từ",Code="bang-tu",Status=false},
             new BoSuuTapViewModel(){Name="CD-bộ",Code="cd-bo",Status=false},
            new BoSuuTapViewModel(){Name="CD-ROM",Code="cd-rom",Status=false},
             new BoSuuTapViewModel(){Name="CD-tập",Code="cd-tap",Status=false},
            new BoSuuTapViewModel(){Name="Luận án",Code="luan-an",Status=false},
             new BoSuuTapViewModel(){Name="Luận án địa chí",Code="lan-an-dia-chi",Status=false},
            new BoSuuTapViewModel(){Name="Sách bộ",Code="sach-bo",Status=false},
            new BoSuuTapViewModel(){Name="Sách tập",Code="sach-tap",Status=false},
            new BoSuuTapViewModel(){Name="Tranh thiếu nhi",Code="tranh-thieu-nhi",Status=false}
        };

        //public BoSuuTapController()
        //{
        //    _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
        //}
        // GET: BoSuuTap
        public ActionResult Index()
        {
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var ListCout = _BoSuuTapLogic.GetAll();
            if (ListCout.Count() <= 0)
            {
                foreach (var item in AddList)
                {
                    BoSuuTap BST = new BoSuuTap()
                    {
                        Name = item.Name,
                        Code = item.Code,
                        Status = item.Status,
                        CreateDateTime = DateTime.Now,
                    };
                    _BoSuuTapLogic.Insert(BST);
                }
            }
            var List = _BoSuuTapLogic.GetAll();
            List<BoSuuTapViewModel> ListAll = new List<BoSuuTapViewModel>();
            foreach (var item in List)
            {
                BoSuuTapViewModel BST = new BoSuuTapViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreateDateTime = item.CreateDateTime
                };
                ListAll.Add(BST);
            }
            return View(ListAll);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BoSuuTapViewModel model)
        {
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(ModelState.Keys.ToString(), ModelState.Values.ToString());
                return View(model);
            }
            BoSuuTap BST = new BoSuuTap()
            {
                Name = model.Name,
                CreateDateTime = DateTime.Now,
            };

            _BoSuuTapLogic.Insert(BST);
            return RedirectToAction("Index");
        }


        public ActionResult Edit(string id)
        {
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var FindId = _BoSuuTapLogic.FindById(id);

            BoSuuTapViewModel BST = new BoSuuTapViewModel()
            {
                Id = FindId.Id,
                Name = FindId.Name,
                CreateDateTime = FindId.CreateDateTime
            };
            return View(BST);
        }

        public ActionResult Delete(string Id)
        {
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            _BoSuuTapLogic.Delete(Id.ToString());
            return RedirectToAction("Index");
        }
    }

}