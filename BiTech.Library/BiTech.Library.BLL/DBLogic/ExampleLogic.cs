using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class ExampleLogic
    {
        private string TableName = "Example";
        public ExampleEngine _ExampleEngine { get; set; }

        public ExampleLogic(string connectionString, string dbName)
        {
            _ExampleEngine = new ExampleEngine(new Database(connectionString, dbName), TableName);
        }

        public List<Example> getAllExample()
        {
            return _ExampleEngine.getAllExample();
        }
    }
}
