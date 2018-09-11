using System.Collections.Generic;

namespace BiTech.Library.Models
{
    public class SSOUserDataModel
    {
        public string Id { get; set; } = "";

        public string UserName { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Avatar { get; set; } = "";

        public string Role { get; set; } = "";

        public string WorkPlaceId { get; set; } = "";

        public string WorkPlaceName { get; set; } = "";

        public Dictionary<string, SSOUserAppModel> MyApps { get; set; } = new Dictionary<string, SSOUserAppModel>();
    }

    public class SSOUserAppModel
    {
        public string AppName { get; set; } = "";

        public string Licence { get; set; } = "";

        public string ConnectionString { get; set; } = "";

        public string DatabaseName { get; set; } = "";
    }

    public static class SSOUserDataClaimType
    {
        public static string Id               = "SSOUserData_Id";
        public static string UserName         = "SSOUserData_UserName";
        public static string FullName         = "SSOUserData_FullName";
        public static string Role             = "SSOUserData_Role";
        public static string AppName          = "SSOUserData_AppName";
        public static string Licence          = "SSOUserData_Licence";
        public static string ConnectionString = "SSOUserData_ConnectionString";
        public static string DatabaseName     = "SSOUserData_DatabaseName";
    }

    public class UserAccessInfo
    {
        public string Id { get; set; } = "";

        public string UserName { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Avatar { get; set; } = "";

        public string Role { get; set; } = "";

        public string DatabaseName { get; set; } = "";

        public string WorkPlaceId { get; set; }
    }

    public class SubAccess
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public ushort Type { get; set; }

        public List<SubAccess> ListSubAccess { get; set; }
    }
}