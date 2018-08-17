using System.Configuration;

namespace BiTech.Library.Helpers
{
    public enum UploadFolder
    {
        BookCovers,
        QRCodeUser,
        AvatarUser,
        FileExcel,
        FileWord,
        FileMrc
    }

    public static class Tool
    {
        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key]?.ToString();
        }

        public static string GetUploadFolder(UploadFolder type)
        {
            string mainUpload = @"Upload\";
            switch (type)
            {
                case UploadFolder.BookCovers:
                    return mainUpload + @"BookCovers\";
                case UploadFolder.QRCodeUser:
                    return mainUpload + @"QRCodeUser\";
                case UploadFolder.AvatarUser:
                    return mainUpload + @"AvatarUser\";
                case UploadFolder.FileExcel:
                    return mainUpload + @"FileExcel\";
                case UploadFolder.FileWord:
                    return mainUpload + @"FileWord\";
                case UploadFolder.FileMrc:
                    return mainUpload + @"FileMrc\";
                default:
                    return null;
            }
        }
    }
}