﻿namespace BiTech.Library.BLL.BarCode_QR
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
