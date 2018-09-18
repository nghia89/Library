using BiTech.Library.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{    
    public class ImportExcelDDCViewModel
    {
        public List<string[]> RawDataList { get; set; }
        public int TotalEntry { get; set; }
        public List<DDC> ListSuccess { get; set; } = new List<DDC>();
        public List<DDC> ListFail { get; set; } = new List<DDC>();
        public List<ArrayList> ListShow { get; set; } = new List<ArrayList>();
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}