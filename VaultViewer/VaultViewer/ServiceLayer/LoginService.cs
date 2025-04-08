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
        // Enum thingy for all possible types of wrong userinput:
        public enum InputValidationResult
        {
            Valid,
            MissingUsername,
            MissingPassword,
            EmptyUsernameAndPassword,
            UsernameTooShort,
            PasswordTooShort,
            EmployeeNotFound
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
                string selectEmployeeQuery = "SELECT EmployeeID FROM employee WHERE FirstName = @username LIMIT 1";
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
                }   

                // Step 3: Insert the new user login with the associated EmployeeID
                string insertQuery = "INSERT INTO employeelogin (UserName, PasswordHash, EmployeeID) VALUES (@username, @passwordhash, @employeeid)";
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


