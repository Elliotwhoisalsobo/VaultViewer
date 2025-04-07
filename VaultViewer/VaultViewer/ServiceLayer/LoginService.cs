using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace VaultViewer.ServiceLayer
{
    public class LoginService
    {
        private string connectionstring = "Server=localhost:3306;Database=vaultviewer;Uid=root;Pwd=root";

       // check if connectionstring is valid or not.
        public bool TestConnection()
        {
            string connectionString = "server=localhost:3306;database=vaultviewer;uid=root;pwd=root;";

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
            using (MySqlConnection conn = new MySqlConnection(connectionstring))
            {
                conn.Open(); // don't forget to close
                string query = "SSELECT id from users WHERE username = @username AND password = @password";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
                // wpf code:
              // LoginService loginService = new LoginService();
                //bool success = loginService.Authenticate(txtUsername.Text, txtPassword.Password);

                //if (success)
                //{
                  //  MessageBox.Show("Login geslaagd!");
               // }
                //else
                //{
                  //  MessageBox.Show("Ongeldige inloggegevens.");
               // }

            }
        }
    }
}
