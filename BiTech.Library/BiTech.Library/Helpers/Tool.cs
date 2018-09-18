using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using BiTech.Library.DTO;

namespace BiTech.Library.Helpers
{
    public enum UploadFolder
    {
        Upload,
        CustomerBackup,
        BookCovers,
        QRCodeUser,
        QRCodeBook,
        AvatarUser,
        FileExcel,
        FileWord,
        FileMrc,
        Reports
    }

    public static class Tool
    {
        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key]?.ToString();
        }

        public static string GetUploadFolder(UploadFolder type, string subdomain = "")
        {
            string mainUpload = @"Upload";

            switch (type)
            {
                case UploadFolder.Upload:
                    return Path.Combine(mainUpload, subdomain);
                case UploadFolder.CustomerBackup:
                    return Path.Combine(mainUpload, @"CustomerBackup", subdomain);
                case UploadFolder.BookCovers:
                    return Path.Combine(mainUpload, subdomain, @"BookCovers");
                case UploadFolder.QRCodeUser:
                    return Path.Combine(mainUpload, subdomain, @"QRCodeUser");
                case UploadFolder.QRCodeBook:
                    return Path.Combine(mainUpload, subdomain, @"QRCodeBook");
                case UploadFolder.AvatarUser:
                    return Path.Combine(mainUpload, subdomain, @"AvatarUser");
                case UploadFolder.FileExcel:
                    return Path.Combine(mainUpload, subdomain, @"FileExcel");
                case UploadFolder.FileWord:
                    return Path.Combine(mainUpload, subdomain, @"FileWord");
                case UploadFolder.FileMrc:
                    return Path.Combine(mainUpload, subdomain, @"FileMrc");
                case UploadFolder.Reports:
                    return Path.Combine(mainUpload, subdomain, @"Reports");
                default:
                    return null;
            }
        }

        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png" &&
                        postedFile.ContentType.ToLower() != "image/bmp")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".bmp")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                //if (postedFile.ContentLength > ImageMinimumBytes)
                //{
                //    return false;
                //}

                byte[] buffer = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(buffer, 0, postedFile.ContentLength);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }

            return true;
        }
    }
}