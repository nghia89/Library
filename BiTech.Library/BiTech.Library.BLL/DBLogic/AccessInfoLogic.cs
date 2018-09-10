using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;

namespace BiTech.Library.BLL.DBLogic
{
    /// <summary>
    /// Connect to Store ONLY
    /// </summary>
    public class AccessInfoLogic : BaseLogic
    {
        AccessInfoEngine _AccessInfoEngine;

        public AccessInfoLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _AccessInfoEngine = new AccessInfoEngine(database, databaseName, DBTableNames.AccessInfo_Table);
        }

        public bool Update(AccessInfo model)
        {
            var data = _AccessInfoEngine.GetWorkPlaceByIdWorkplace(model.IdWorkplace);

            // insert if new
            if(data == null)
            {
                var rs = _AccessInfoEngine.Insert(model);
                return rs.Length > 0;
            }
            else
            {
                model.Id = data.Id;
                return _AccessInfoEngine.Update(model);
            }
        }

        public AccessInfo GetBySubDomain(string subdomain)
        {
            return _AccessInfoEngine.GetWorkPlaceBySubDomain(subdomain);
        }
    }
}
