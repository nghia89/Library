using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Common;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DAL.Common;
using BiTech.Library.DAL.CommonConstants;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using PagedList;
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
    public class ExportTheController : BaseController
    {
        // GET: ExportThe
        public ActionResult Index()
        {
            return View();
        }
      
    }
}