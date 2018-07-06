using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL
{
    public interface IDatabase
    {
        /// <summary>
        /// Chuỗi kế nối database
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Tên Database
        /// </summary>
        //string DatabaseName { get; set; }

        object GetConnection(string databaseName);
    }
}
