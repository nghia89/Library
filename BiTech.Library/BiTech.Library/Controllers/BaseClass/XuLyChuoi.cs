using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Controllers.BaseClass
{
    public class XuLyChuoi
    {
        public string ChuanHoaChuoi(string strSource)
        {
            string name = strSource.Trim().ToLower();
            string kq = "";
            for (int i = 0; i < name.Length; i++)
            {
                if (i == 0)
                    kq += name[i].ToString().ToUpper();
                else
                    kq += name[i];
                if (name[i] == ' ')
                {
                    while (name[i] == ' ')
                    {
                        i++;
                    }
                    kq += name[i].ToString().ToUpper();
                }
            }
            return kq;
        }
    }
}