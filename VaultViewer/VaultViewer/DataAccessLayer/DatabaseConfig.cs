using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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