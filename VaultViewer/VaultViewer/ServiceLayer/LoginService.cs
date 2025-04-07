using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace VaultViewer.ServiceLayer
{ // should not know whether it's wpf/console/etc
    public class LoginService
    {
        private string connectionString = "Server=localhost;Database=vaultviewer;Uid=root;Pwd=root";

       // check if connectionstring is valid or not.
        public bool TestConnection()
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Database connection successful!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Database connection failed:\n" + ex.Message);
                return false;
            }
        }



        public bool Authenticate(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open(); // don't forget to close
                string query = "SELECT EmployeeID from employeelogin WHERE username = @username AND passwordhash = @passwordhash";
                // employee --> employeeRole --> role 
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordhash", password);

                    using (var reader = cmd.ExecuteReader())
                    {

                        return reader.HasRows; // also return list of roles
                    }
                }

/* SELECT r.name
FROM
    Users u JOIN
    Employee e ON u.EmployeeId = e.EmployeeId JOIN
    EmployeeRole er ON e.EmployeeId = er.EmployeeId JOIN
    Role r ON er.RoleId = r.RoleId
WHERE
    u.UserName = @UserName AND
    u.Password = @Password;
*/

            }
        }
    }
}
