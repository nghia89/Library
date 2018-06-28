using System.Configuration;

namespace BiTech.Library.Helpers
{
    public enum UploadFolder
    {
        BookCovers
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
                default:
                    return null;
            }
        }
    }
}