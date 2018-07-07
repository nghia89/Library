using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    public enum EUser
    {
        Active,
        DeActive, //khoa the
        Deleted,
    }
    public enum EPhieuMuon
    {
        DaTra,
        ChuaTra,
        Deleted,
    }

    public enum ENgonNgu
    {
        VietNamese,
        English
    }

    public enum ETinhTrangPhieuMuon
    {
        none,
        ChuaTra,
        GanTra,        
        TraTre,
        TraDungHen

    }

}
