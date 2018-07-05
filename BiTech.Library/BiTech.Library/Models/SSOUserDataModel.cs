using System.Collections.Generic;

namespace BiTech.Library.Models
{
    public class SSOUserDataModel
    {
        public string Id { get; set; } = "";

        public string UserName { get; set; } = "";

        public string Role { get; set; } = "";

        public Dictionary<string, SSOUserAppModel> MyApps { get; set; } = new Dictionary<string, SSOUserAppModel>();
    }

    public class SSOUserAppModel
    {
        public string AppName { get; set; } = "";

        public string Licence { get; set; } = "";

        public string ConnectionString { get; set; } = "";

        public string DatabaseName { get; set; } = "";
    }
}