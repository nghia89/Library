using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statictical


        private ThongKeLogic _thongKeLogic;
        private ChiTietPhieuMuonLogic _chiTietPhieuMuonLogic;
        public StatisticController()
        {
            _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }
    }
}