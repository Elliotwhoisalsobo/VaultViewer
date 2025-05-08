using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VaultViewer.ServiceLayer.LoginService;
using System.Windows;
using System.Configuration;

namespace VaultViewer.ServiceLayer
{
    public class AdminpanelService
    {
       // MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["VaultViewer"].ConnectionString);
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["VaultViewer"].ConnectionString; // No hardcoded connectionstring : D
        
        //public bool DeleteUser(string user)
        //{
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        // Check if username == firstname (of employee)
        //        const string selectEmployeeQuery = "SELECT EmployeeID FROM employee WHERE FirstName = @username LIMIT 1";
        //        int employeeId = -1; // no employee found

        //        using (MySqlCommand selectCmd = new MySqlCommand(selectEmployeeQuery, conn))
        //        {
        //            selectCmd.Parameters.AddWithValue("@username", user);

        //            // Execute and retrieve the EmployeeID if it exists
        //            using (var reader = selectCmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    employeeId = reader.GetInt32("EmployeeID");
        //                }

        //                else
        //                {
        //                    MessageBox.Show("Employee not found!");
        //                }
        //                conn.Close();
        //            }
        //        }
        //    }
        //}
    }
}
            

