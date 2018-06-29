using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.Models
{
    public class baseBarCodeLic
    {
        public baseBarCodeLic()
        {
            new Aspose.BarCode.License().SetLicense(LicenseHelper.License.LStream);
        }
    }
}
