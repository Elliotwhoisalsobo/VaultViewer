using MySql.Data.MySqlClient;
using VaultViewer.Business;

namespace VaultViewer.DataAccessLayer
{
    public class LoginRepository
    {
        public void TestConnection()
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
        }

        public Login? GetByUsername(string username)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            const string sql = "SELECT UserName, PasswordHash, EmployeeID FROM employeelogin WHERE UserName = @username";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();

            return reader.Read()
                ? new Login(reader.GetString("UserName"), reader.GetString("PasswordHash"), reader.GetInt32("EmployeeID"))
                : null;
        }

        public List<Login> GetAll(int? limit = null)
        {
            var logins = new List<Login>();
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();

            var sql = "SELECT UserName, PasswordHash, EmployeeID FROM employeelogin";
            if (limit.HasValue)
            {
                sql += " LIMIT @limit";
            }

            using var command = new MySqlCommand(sql, connection);
            if (limit.HasValue)
            {
                command.Parameters.AddWithValue("@limit", limit.Value);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                logins.Add(new Login(reader.GetString("UserName"), reader.GetString("PasswordHash"), reader.GetInt32("EmployeeID")));
            }

            return logins;
        }

        public bool TryAdd(Login login)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            const string sql = "INSERT INTO employeelogin (UserName, PasswordHash, EmployeeID) VALUES (@username, @passwordHash, @employeeId)";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@username", login.UserName);
            command.Parameters.AddWithValue("@passwordHash", login.PasswordHash);
            command.Parameters.AddWithValue("@employeeId", login.EmployeeID);

            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException exception) when (exception.Number == 1062)
            {
                return false;
            }
        }
    }
}
