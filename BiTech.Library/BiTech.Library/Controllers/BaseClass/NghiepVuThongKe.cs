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
    public class NghiepVuThongKe
    {
        /// <summary>
        /// Đếm số phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoPhieuMuon(List<ThongTinMuonSach> listPM)
        {
            List<ThongTinMuonSach> listRutGon = new List<ThongTinMuonSach>();
            foreach (var item in listPM)
            {
                // Gộp sách                
                if (listRutGon.FindIndex(_ => _.idUser == item.idUser
                && _.idSach == item.idSach
                && _.NgayGioMuon.ToShortDateString() == item.NgayGioMuon.ToShortDateString()
                && _.NgayPhaiTra.ToShortDateString() == item.NgayPhaiTra.ToShortDateString()
                && _.NgayTraThucTe.ToShortDateString() == item.NgayTraThucTe.ToShortDateString()) > -1)
                {
                    // Đã tồn tại
                    var ttms = listRutGon.Where(_ => _.idUser == item.idUser
                    && _.idSach == item.idSach
                    && _.NgayGioMuon.ToShortDateString() == item.NgayGioMuon.ToShortDateString()
                    && _.NgayPhaiTra.ToShortDateString() == item.NgayPhaiTra.ToShortDateString()
                    && _.NgayTraThucTe.ToShortDateString() == item.NgayTraThucTe.ToShortDateString()).SingleOrDefault();
                    ttms.SoSachTong += 1;
                }
                else
                {
                    listRutGon.Add(item);
                }
            }
            return listRutGon.Count();
        }

        /// <summary>
        /// Đếm số người mượn sách trong phiếu mượn
        /// </summary>
        /// <param name="listPM"></param>
        /// <returns></returns>
        public int DemSoNguoiMuonSach(List<ThongTinMuonSach> listPM)
        {
            bool flat = false;
            List<object> listSoLuongThanhVien = new List<object>();
            string idThanhVien = null;
            foreach (var pm in listPM)
            {
                // Đếm số người mượn trong ngày (không trùng)
                flat = false;
                idThanhVien = pm.idUser;
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
        public int DemSoNguoiTraTre(List<ThongTinMuonSach> listPM)
        {
            int soNguoiTraTre = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPM)
            {
                DateTime ngayPhaiTra = item.NgayPhaiTra;

                //if (item.NgayTraThucTe == null)
                //    item.NgayTraThucTe = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);

                DateTime ngayTraThucTe = item.NgayTraThucTe;
                if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
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
        public int DemSoNguoiKhongTra(List<ThongTinMuonSach> listPM)
        {
            int soNguoiKhongTra = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPM)
            {
                DateTime ngayPhaiTra = item.NgayPhaiTra;

                //if (item.NgayTraThucTe == null)
                //    item.NgayTraThucTe = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);

                DateTime ngayTraThucTe = item.NgayTraThucTe;

                // DateTime ngayMuon = DateTime.ParseExact(item.NgayGioMuon, "dd-MM-yyyy", null);
                if (ngayPhaiTra < DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
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
        public int DemSoSachDuocMuon(List<ThongTinMuonSach> listPM, string connectionString, string databaseName)
        {
            var _thongKeLogic = new ThongKeLogic(connectionString, databaseName);
            int soLuongSach = 0;
            foreach (var pm in listPM)
            {
                soLuongSach++;

            }
            return soLuongSach;
        }

        public List<ThongTinMuonSach> RutGonDanhSach(List<ThongTinMuonSach> list)
        {
            List<ThongTinMuonSach> listRutGon = new List<ThongTinMuonSach>();
            foreach (var item in list)
            {
                // Gộp sách                
                if (listRutGon.FindIndex(_ => _.idUser == item.idUser
                && _.idSach == item.idSach
                && _.NgayGioMuon.ToShortDateString() == item.NgayGioMuon.ToShortDateString()
                && _.NgayPhaiTra.ToShortDateString() == item.NgayPhaiTra.ToShortDateString()
                && _.NgayTraThucTe.ToShortDateString() == item.NgayTraThucTe.ToShortDateString()) > -1)
                {
                    // Đã tồn tại
                    var ttms = listRutGon.Where(_ => _.idUser == item.idUser
                    && _.idSach == item.idSach
                    && _.NgayGioMuon.ToShortDateString() == item.NgayGioMuon.ToShortDateString()
                    && _.NgayPhaiTra.ToShortDateString() == item.NgayPhaiTra.ToShortDateString()
                    && _.NgayTraThucTe.ToShortDateString() == item.NgayTraThucTe.ToShortDateString()).SingleOrDefault();
                    ttms.SoSachTong += 1;
                }
                else
                {
                    listRutGon.Add(item);
                }
            }
            return listRutGon;
        }

        // Đếm số sách trả
        public int DemSoSachDuocTra(List<ThongTinMuonSach> listPT, int? day, int? month, int? year)
        {
            int dem = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPT)
            {
                // Thống kê theo ngày
                if (day != null && item.NgayTraThucTe.Day == day)
                {
                    dem++;
                }
                // Thống kê theo tháng
                if (day == null && item.NgayTraThucTe.Month == month && item.NgayTraThucTe.Year == year)
                {
                    dem++;
                }
                // Thống kê theo tuần 
                if (day == null && month == null && year == null)
                {
                    dem++;
                }
            }
            return dem;
        }

        // Đếm số sách trễ hạn
        public int DemSoSachKhongTra(List<ThongTinMuonSach> listPM)
        {
            int dem = 0;
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPM)
            {
                if (item.NgayPhaiTra < DateTime.Today && (item.NgayTraThucTe == ngayTraNull || item.NgayTraThucTe == null))
                {
                    dem++;
                }
            }
            return dem;
        }

        // Thống kê theo tuần
        public int[] ThongKeTheoTuan(DateTime dayInWeek, string connectionString, string databaseName)
        {
            var _thongKeLogic = new ThongKeLogic(connectionString, databaseName);
            int[] arrInfo = new int[5];

            int day = dayInWeek.Day;
            int month = dayInWeek.Month;
            int year = dayInWeek.Year;

            var listMuonSach = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listPM = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listPT = new List<ThongTinMuonSach>();
            foreach (var item in listMuonSach)
            {
                if (item.NgayGioMuon.Day == day && item.NgayGioMuon.Month == month && item.NgayGioMuon.Year == year)
                {
                    listPM.Add(item);
                }
                if (item.NgayTraThucTe.Day == day && item.NgayTraThucTe.Month == month && item.NgayTraThucTe.Year == year)
                {
                    listPT.Add(item);
                }
            }
            int soSachDuocMuon = DemSoSachDuocMuon(listPM, connectionString, databaseName);
            int soSachTra = DemSoSachDuocTra(listPT,null,null,null);
            int soSachTraTre = DemSoSachKhongTra(listPM);
            int soNguoiMuon = DemSoNguoiMuonSach(listPM);
            arrInfo[0] = soNguoiMuon;
            arrInfo[1] = soSachDuocMuon;
            arrInfo[2] = soSachTra;
            arrInfo[3] = soSachTraTre;  
            return arrInfo;
        }
    }
}