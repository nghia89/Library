namespace BiTech.Library.DAL
{
    public interface IDatabase
    {
        /// <summary>
        /// Chuỗi kế nối database
        /// </summary>
        string ConnectionString { get; set; }
        
        object GetConnection(string databaseName);
    }
}
