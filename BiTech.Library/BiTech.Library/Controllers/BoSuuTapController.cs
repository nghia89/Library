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
        //public BoSuuTapController()
        //{
        //    _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
        //}
        // GET: BoSuuTap
        public ActionResult Index()
        {
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            
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