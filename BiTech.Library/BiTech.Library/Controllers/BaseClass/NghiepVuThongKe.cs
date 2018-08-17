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
                DateTime ngayPhaiTra = DateTime.ParseExact(item.NgayPhaiTra, "dd-MM-yyyy", null);
                if (String.IsNullOrEmpty(item.NgayTraThucTe))
                    item.NgayTraThucTe = "01-01-0001";
                DateTime ngayTraThucTe = DateTime.ParseExact(item.NgayTraThucTe, "dd-MM-yyyy", null);
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
                DateTime ngayPhaiTra = DateTime.ParseExact(item.NgayPhaiTra, "dd-MM-yyyy", null);
                if (String.IsNullOrEmpty(item.NgayTraThucTe))
                    item.NgayTraThucTe = "01-01-0001";
                DateTime ngayTraThucTe = DateTime.ParseExact(item.NgayTraThucTe, "dd-MM-yyyy", null);
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
                //var listCTPM = _thongKeLogic.GetCTPMById(pm.Id);
                //foreach (var i in listCTPM)
                //{


                //    // số lượng sách được mượn
                //    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                //    soLuongSach += soLuong; // tổng số sách được mượn trong danh sách phiếu mượn (tháng/ngày)
                //}

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
                && _.NgayGioMuon == item.NgayGioMuon
                && _.NgayPhaiTra == item.NgayPhaiTra
                && _.NgayTraThucTe == item.NgayTraThucTe) > -1)
                {
                    // Đã tồn tại
                    var ttms = listRutGon.Where(_ => _.idUser == item.idUser
                    && _.idSach == item.idSach
                    && _.NgayGioMuon == item.NgayGioMuon
                    && _.NgayPhaiTra == item.NgayPhaiTra
                    && _.NgayTraThucTe == item.NgayTraThucTe).SingleOrDefault();
                    ttms.SoSachTong += 1;
                }
                else
                {
                    listRutGon.Add(item);
                }
            }
            return listRutGon;
        }

    }
}