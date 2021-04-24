using System.Configuration;

namespace Common.Utility
{
    public static class ConnectionStringHelper
    {
        /// <summary>
        /// Return ConnectionString
        /// </summary>
        /// <param name="name">ConnectionString Name</param>
        /// <returns>ConnectionString</returns>
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
