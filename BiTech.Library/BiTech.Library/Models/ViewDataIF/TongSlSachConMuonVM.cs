using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models.ViewDataIF
{
    public class TongSlSachConMuonVM
    {
        public string Maks { get; set; }
        public string TenSach { get; set; }
        public int TongSl { get; set; }
        public int SLChoMuon { get; set; }
        public int SLKHChoMuon { get; set; }
    }
}