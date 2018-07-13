using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{

    public class LanguageModel
    {
        public Language TheL { get; set; }
    }
    public class BaseLanguageModel
    {     
        public string Id { get; set; }

        public string Ten { get; set; }

        public string TenNgan { get; set; }

    }

    //public class BaseLanguageModel
    //{
    //    public string Id { get; set; }

    //    public string Ten { get; set; }

    //    public string TenNgan { get; set; }

    //    public DateTime CreateDateTime { get; set; }

    //    public BaseLanguageModel()
    //    {
    //        Id = "";
    //        Ten = "";
    //        TenNgan = "";
    //        CreateDateTime = new DateTime();
    //    }

    //    public BaseLanguageModel(Language dto)
    //    {
    //        Id = dto.Id;
    //        Ten = dto.Ten;
    //        TenNgan = dto.TenNgan;
    //        CreateDateTime = dto.CreateDateTime;
    //    }

    //    public Language ToDTO()
    //    {
    //        return new Language()
    //        {
    //            Id = string.IsNullOrWhiteSpace(this.Id) == false ? this.Id : null,
    //            Ten = this.Ten,
    //            TenNgan = this.TenNgan,
    //            CreateDateTime = this.CreateDateTime
    //        };
    //    }
    //}
}