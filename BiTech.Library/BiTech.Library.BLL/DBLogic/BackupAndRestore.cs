using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class BackupAndRestore : BaseLogic
    {
        private static string _dirMongodb = @"mongodb\";
        private static string _dirUpload = @"upload\";

        //start mongodump.exe --host localhost --port 27017 --username adminbk --password password --authenticationDatabase admin --db ABCD_S_sohcm --out D:\backup\localhostdb\backup_12212018_121212

        // BackupPath = .\CustomerBackup\<WebSubDomain>\<backup_ddMMyy_HHmmss>\
        // sub dir
        // BackupPath = .\CustomerBackup\<WebSubDomain>\<backup_ddMMyy_HHmmss>\mongodb\
        // BackupPath = .\CustomerBackup\<WebSubDomain>\<backup_ddMMyy_HHmmss>\upload\

        public List<FileInfo> GetBackupFiles(string backupPath)
        {
            List<FileInfo> listFiles = new List<FileInfo>();

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(backupPath);
                var files = dirInfo.GetFiles("*.zip");

                listFiles.AddRange(files);
            }
            catch { }

            return listFiles;
        }

        public ResultInfo Backup(BackupRestoreInfo info)
        {
            #region Help
            /*
             * run by cmd
             * "mongoredump --host {0} --port {1} --username {2} --password {3} --authenticationDatabase {4} --out {5} --db {6}"
             * 
             * run by ProcessStartInfo
             * "--host {0} --port {1} --username {2} --password {3} --out {4} --db {5}"
             * 
             * {0} = address: localhost; 192.168.1.230
             * {1} = port: 27017 (default)
             * {2} = username have write access to mongodb: admin (default)
             * {3} = password for {2}
             * {4} = authentication for database name
             * {5} = dirctory output: d:\backup\
             * {6} = database name to backup - can be empty : BiTech_db1
             * 
             * example
             * --host localhost --port 27017 --username --password --authenticationDatabase BiTech_db1 --out d:\backup\BiTech_db1 --db BiTech_db1
             */
            #endregion

            ResultInfo rs = new ResultInfo();

            try
            {
                // prepare directory

                var backupPath = string.Format(@"{0}{1}_{2}", info.BackupPath, "backup", DateTime.Now.ToString("ddMMyy_HHmmss"));
                var backup_dirMongodb = backupPath + "\\" + _dirMongodb;
                var backup_dirUpload = backupPath + "\\" + _dirUpload;

                if (!Directory.Exists(backup_dirMongodb))
                    Directory.CreateDirectory(backup_dirMongodb);

                if (!Directory.Exists(backup_dirUpload))
                    Directory.CreateDirectory(backup_dirUpload);

                // create command line

                string auth = "";
                if (info.UserName.Length > 0)
                {
                    auth = string.Format(" --username {0} --password {1} --authenticationDatabase {2}", info.UserName, info.Password, info.AuthenticationDatabase);
                }
                var argumentString = "--host {0} --port {1}{2} --out {3} --db {4}";
                var cmdString = string.Format(argumentString, info.Host, info.Port, auth, backup_dirMongodb, info.DatabaseName);

                // start backup mongodb

                Process backupProcess = new Process();
                backupProcess.StartInfo = new ProcessStartInfo(info.MongoExePath, cmdString);
                backupProcess.Start();

                backupProcess.WaitForExit();
                backupProcess.Refresh();

                // start copy upload
                DirectoryCopy(info.UploadPath, backup_dirUpload, true);

                // start compress backupPath

                var fileName = string.Format("{0}.zip", backupPath);
                Compress(backupPath, fileName);

                // clean backupPath
                Directory.Delete(backupPath, true);

                rs.Status = ResultInfo.ResultStatus.OK;
                rs.Data = Path.GetFileName(fileName);

            }
            catch (Exception ex)
            {
                rs.Status = ResultInfo.ResultStatus.Error;
                rs.Data = "Exception in backup: " + ex.Message;

                Directory.Delete(info.BackupPath, true);
            }

            return rs;
        }

        public ResultInfo Restore(BackupRestoreInfo info)
        {
            #region Help
            /*
             * run by cmd
             * "mongorestore --host {0} --port {1} --username {2} --password {3} --authenticationDatabase {4} --db {5} --drop --dir {6}"
             * 
             * run by ProcessStartInfo
             * "--host {0} --port {1} --username {2} --password {3} --db {4} --drop --dir {5}"
             * 
             * {0} = address: localhost; 192.168.1.230
             * {1} = port: 27017 (default)
             * {2} = username have write access to mongodb: admin (default)
             * {3} = password for {2}
             * {4} = authentication for database name
             * {5} = database name to restore : BiTech_db1
             * {6} = dirctory location content database name folder: d:\backup\BiTech_db1
             *
             * example
             * --host localhost --port 27017 --username --password --authenticationDatabase BiTech_db1 --db BiTech_db1 --drop --dir d:\backup\BiTech_db1
             */
            #endregion

            ResultInfo rs = new ResultInfo();
            string backupPath = "";
            try
            {
                // prepare directory

                backupPath = Decompress(info.BackupPath);
                var backup_dirMongodb = backupPath + _dirMongodb + info.DatabaseName;
                var backup_dirUpload = backupPath + _dirUpload;

                // create command line
                string auth = "";
                if (info.UserName.Length > 0)
                {
                    auth = string.Format(" --username {0} --password {1} --authenticationDatabase {2}", info.UserName, info.Password, info.AuthenticationDatabase);
                }
                var argumentString = "--host {0} --port {1}{2} --db {3} --drop --dir {4}";
                var cmdString = string.Format(argumentString, info.Host, info.Port, auth, info.DatabaseName, backup_dirMongodb);

                // start restore

                Process restoreProcess = new Process();
                restoreProcess.StartInfo = new ProcessStartInfo(info.MongoExePath, cmdString);
                restoreProcess.Start();

                restoreProcess.WaitForExit();

                // start copy upload directory

                Directory.Delete(info.UploadPath, true); // Delete current upload\<WebSubDomain> path
                Directory.CreateDirectory(info.UploadPath); // Re-create current upload\<WebSubDomain> path
                DirectoryCopy(backup_dirUpload, info.UploadPath, true);

                // clean unzip path
                Directory.Delete(backupPath, true);

                rs.Status = ResultInfo.ResultStatus.OK;
                rs.Data = "";
            }
            catch (Exception ex)
            {
                rs.Status = ResultInfo.ResultStatus.Error;
                rs.Data = "Exception in restore: " + ex.Message;

                try
                {
                    if (backupPath.Length > 0 && Directory.Exists(backupPath))
                        Directory.Delete(backupPath, true);
                }
                catch
                {
                    // todo log this backupPath
                }
            }
            return rs;
        }

        /// <summary>
        /// Compress the given file to a .zip
        /// </summary>
        /// <param name="zipPath">Full Path</param>
        /// <param name="filename">Full File Name</param>
        private void Compress(string zipPath, string filename)
        {
            using (var f = new ZipFile(filename, Encoding.UTF8))
            {
                f.AddDirectory(zipPath);
                f.CompressionLevel = CompressionLevel.BestCompression;
                f.UseZip64WhenSaving = Zip64Option.AsNecessary;
                f.Save();
            }
        }

        /// <summary>
        /// Decompress the zip file and return the folder path
        /// </summary>
        /// <param name="filename">zip file name</param>
        /// <returns></returns>
        private string Decompress(string filename)
        {
            using (var f = new ZipFile(filename, Encoding.UTF8))
            {
                string folder = filename.Replace(".zip", "");
                f.ExtractAll(folder);
                return folder + "\\";
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }

    public class BackupRestoreInfo
    {
        public string DatabaseName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationDatabase { get; set; }
        public string MongoExePath { get; set; }
        public string BackupPath { get; set; }
        public string UploadPath { get; set; }
    }

    public class ResultInfo
    {
        public ResultStatus Status { get; set; }
        public string Data { get; set; }

        public enum ResultStatus
        {
            OK,
            Error
        }
    }
}
