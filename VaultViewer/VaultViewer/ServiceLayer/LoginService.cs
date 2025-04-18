using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Security.Cryptography.X509Certificates;
using VaultViewer.UI;
using Org.BouncyCastle.Asn1.X509;
using System.Configuration;
using VaultViewer.DataAccessLayer;

namespace VaultViewer.ServiceLayer
{ // should not know whether it's wpf/console/etc <-- NO UI LOGIC
    public class LoginService
    {
        // db connection
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["VaultViewer"].ConnectionString);
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["VaultViewer"].ConnectionString; // No hardcoded connectionstring : D
        //public string connectionString = "Server=localhost;Database=vaultviewer;Uid=root;Pwd=root"; // removed hardcode


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

        // Get Roles:
        public List<string> GetRoles(int employeeId)
        {
            List<string> roles = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT r.Name 
                         FROM employeerole er 
                         JOIN role r ON er.RoleId = r.RoleId 
                         WHERE er.EmployeeId = @employeeId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString("Name"));
                        }
                    }
                }
            }

            return roles;
        }


        public bool Authenticate(string username, string password, out List<string> roles) // out = return parameter value instead of take in value for parameter
        {
            roles = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open(); // don't forget to close
                string query = "SELECT PasswordHash, EmployeeID FROM employeelogin WHERE UserName = @username";
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
                            int employeeId = reader.GetInt32("EmployeeID");

                            // Verify password
                            if (BCrypt.Net.BCrypt.Verify(password, storedHash)) // BCrypt --> One-way hashing algorithm
                            {
                                // password correct, fetch list of roles for user  
                                roles = GetRoles(employeeId);
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }

                    }

                }
            }
            return false; // authentication failed
        }

        // Enum thingy for all possible types of wrong userinput:
        public enum InputValidationResult
        {
            Valid,
            MissingUsername,
            MissingPassword,
            EmptyUsernameAndPassword,
            UsernameTooShort,
            PasswordTooShort,
            EmployeeNotFound,
            UserAlreadyExists, // not yet implemented
            InvaliddatabaseConnection // not yet implemented
        }

        // Helper method getuser for useralreadyexists case:
        private bool UserExists(string username)
        {
            // employeeLogin
            if username is in 
        }


        // Helper method to validate userinput:
        InputValidationResult ValidateUserInput(string username, string password, int employeeId)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                return InputValidationResult.EmptyUsernameAndPassword;
            if (string.IsNullOrEmpty(username))
                return InputValidationResult.MissingUsername;
            if (string.IsNullOrEmpty(password))
                return InputValidationResult.MissingPassword;
            if (username.Length < 3)
                return InputValidationResult.UsernameTooShort;
            if (password.Length < 6)
                return InputValidationResult.PasswordTooShort;
            if (employeeId == -1)
                return InputValidationResult.EmployeeNotFound;
            //if (username == already in employeeLogin)
            //return InputValidationResult.UserAlreadyExists;

            return InputValidationResult.Valid;
        }
        // Create new user & convert plaintext pwd into hashed pwd
        public bool CreateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if username == firstname (of employee)
                const string selectEmployeeQuery = "SELECT EmployeeID FROM employee WHERE FirstName = @username LIMIT 1";
                int employeeId = -1; // no employee found

                using (MySqlCommand selectCmd = new MySqlCommand(selectEmployeeQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@username", username);

                    // Execute and retrieve the EmployeeID if it exists
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employeeId = reader.GetInt32("EmployeeID");
                        }
                    }
                }
                var validationResult = ValidateUserInput(username, password, employeeId);
                // If userinput invalid return false else add new employeelogin : )
                switch (validationResult)
                {
                    case InputValidationResult.MissingUsername:
                        MessageBox.Show("Username is missing.");
                        return false;

                    
                    case InputValidationResult.MissingPassword:
                        MessageBox.Show("Password is missing.");
                        return false;

                    case InputValidationResult.EmptyUsernameAndPassword:
                        MessageBox.Show("Username and password are missing.");
                        return false;

                    case InputValidationResult.UsernameTooShort:
                        MessageBox.Show("Username is too short (minimum of 3 chars");
                        return false;

                    case InputValidationResult.PasswordTooShort:
                        MessageBox.Show("Password is too short (minmum of x chars");
                        return false;

                    case InputValidationResult.EmployeeNotFound:
                        MessageBox.Show("Employee was not found, your username should match with at least one employee.");
                        return false;

                    case InputValidationResult.Valid:
                        break;

                        // user already exists
                }   

                // Step 3: Insert the new user login with the associated EmployeeID
                const string insertQuery = "INSERT INTO employeelogin (UserName, PasswordHash, EmployeeID) VALUES (@username, @passwordhash, @employeeid)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordhash", hashedPassword);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
// create method to assign role(s) to certain customers? (for admin panel)




// Retrieve a user's roles
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


