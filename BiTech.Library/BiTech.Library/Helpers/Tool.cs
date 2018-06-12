using System.Configuration;

namespace BiTech.Library.Helpers
{
    public class Tool
    {
        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key]?.ToString();
        }
    }
}