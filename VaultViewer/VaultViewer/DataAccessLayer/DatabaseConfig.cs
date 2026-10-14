using MySql.Data.MySqlClient;
using System.Configuration;


namespace VaultViewer.DataAccessLayer
{
    public static class DatabaseConfig
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["VaultViewer"].ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}