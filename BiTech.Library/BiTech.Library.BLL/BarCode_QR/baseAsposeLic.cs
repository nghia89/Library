using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.BarCode_QR
{
    public class baseAsposeLic
    {
        public baseAsposeLic()
        {
            new Aspose.BarCode.License().SetLicense(LicenseHelper.License.LStream);
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
            new Aspose.Words.License().SetLicense(LicenseHelper.License.LStream);
        }
    }
}
