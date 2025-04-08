using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Security.Cryptography.X509Certificates;


namespace VaultViewer.ServiceLayer
{ // should not know whether it's wpf/console/etc
    public class LoginService
    {
        // db connection
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

        // password hashing:
        public string HashPassword(string plainPassword)
        {
            
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }


        public bool Authenticate(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open(); // don't forget to close
                string query = "SELECT PasswordHash FROM employeelogin WHERE UserName = @username";
                // employee --> employeeRole --> role 
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    //cmd.Parameters.AddWithValue("@passwordhash", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString("PasswordHash");
                            return BCrypt.Net.BCrypt.Verify(password, storedHash); // also return list of roles
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        // Create new user & convert plaintext pwd into hashed pwd
        public bool CreateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Optional: Check if user already exists
                string checkQuery = "SELECT COUNT(*) FROM employeelogin WHERE UserName = @username";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        // Username already exists
                        return false;
                    }
                }

                string insertQuery = "INSERT INTO employeelogin (UserName, PasswordHash) VALUES (@username, @passwordhash)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordhash", hashedPassword);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
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
