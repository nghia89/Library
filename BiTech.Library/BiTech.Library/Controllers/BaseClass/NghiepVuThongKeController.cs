using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers.BaseClass
{
    public class NghiepVuThongKeController : BaseController
    {
        /// <summary>
        /// Đếm số phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoPhieuMuon(List<PhieuMuon> listPM)
        {
            int dem = 0;
            foreach (var pm in listPM)
            {
                dem++;
            }
            return dem;
        }
        /// <summary>
        /// Đếm số người mượn sách trong phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoNguoiMuonSach(List<PhieuMuon> listPM)
        {
            bool flat = false;
            List<object> listSoLuongThanhVien = new List<object>();
            string idThanhVien = null;
            foreach (var pm in listPM)
            {
                // Đếm số người mượn trong ngày (không trùng)
                flat = false;
                idThanhVien = pm.IdUser;
                if (listSoLuongThanhVien.Count == 0)
                {
                    listSoLuongThanhVien.Add(idThanhVien);
                }
                else
                {
                    foreach (var x in listSoLuongThanhVien.ToList())
                    {
                        if (x.ToString() != idThanhVien.ToString())
                        {
                            flat = true;
                        }
                        else
                        {
                            flat = false;
                            break;
                        }
                    }
                }
                // Kiểm tra trùng người,nếu không thì tính đó là 1 người mượn 
                if (flat)
                {
                    listSoLuongThanhVien.Add(idThanhVien);
                }
            }
            return listSoLuongThanhVien.Count();
        }
        /// <summary>
        /// Đếm số người trả sách trễ trong phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoNguoiTraTre(List<PhieuMuon> listPM)
        {
            int soNguoiTraTre = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var pm in listPM)
            {
                if (pm.NgayTra != ngayTraNull && pm.NgayTra != null && pm.NgayTra != null && pm.NgayTra > pm.NgayPhaiTra)
                {
                    soNguoiTraTre++;
                }
            }
            return soNguoiTraTre;
        }
        /// <summary>
        /// Đếm số người không trả sách trong phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoNguoiKhongTra(List<PhieuMuon> listPM)
        {
            int soNguoiKhongTra = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var pm in listPM)
            {
                if (pm.NgayPhaiTra < DateTime.Today && pm.NgayTra == ngayTraNull && pm.NgayTra != null)
                {
                    soNguoiKhongTra++;
                }
            }
            return soNguoiKhongTra;
        }   
        /// <summary>
        /// Đếm số sách được mượn trong phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoSachDuocMuon(List<PhieuMuon> listPM)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return 0;
            #endregion        
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int soLuongSach = 0;                       
            foreach (var pm in listPM)
            {
                var listCTPM = _thongKeLogic.GetCTPMById(pm.Id);               
                foreach (var i in listCTPM)
                {
                    // số lượng sách được mượn
                    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                    soLuongSach += soLuong; // tổng số sách được mượn trong danh sách phiếu mượn (tháng/ngày)
                }
            }
            return soLuongSach;
        }

    }
}