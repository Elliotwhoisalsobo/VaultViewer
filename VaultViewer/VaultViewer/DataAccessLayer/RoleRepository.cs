using MySql.Data.MySqlClient;
using VaultViewer.Business;

namespace VaultViewer.DataAccessLayer
{
    public class RoleRepository
    {
        public List<Role> GetForEmployee(int employeeId)
        {
            var roles = new List<Role>();
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            const string sql = @"SELECT r.RoleID, r.Name, r.Description, d.Name AS DepartmentName
                                 FROM employeerole er
                                 JOIN role r ON er.RoleID = r.RoleID
                                 LEFT JOIN department d ON r.DepartmentID = d.DepartmentID
                                 WHERE er.EmployeeID = @employeeId";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeeId", employeeId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                roles.Add(ReadRole(reader));
            }

            return roles;
        }

        public bool AssignToEmployee(int employeeId, string roleName)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            const string roleSql = @"SELECT r.RoleID, r.Name, r.Description, d.Name AS DepartmentName
                                     FROM role r
                                     LEFT JOIN department d ON r.DepartmentID = d.DepartmentID
                                     WHERE r.Name = @name
                                     LIMIT 1";
            using var roleCommand = new MySqlCommand(roleSql, connection);
            roleCommand.Parameters.AddWithValue("@name", roleName);
            using var reader = roleCommand.ExecuteReader();
            if (!reader.Read())
            {
                return false;
            }

            var role = ReadRole(reader);
            reader.Close();

            const string assignSql = "INSERT INTO employeerole (EmployeeID, RoleID) VALUES (@employeeId, @roleId)";
            using var assignCommand = new MySqlCommand(assignSql, connection);
            assignCommand.Parameters.AddWithValue("@employeeId", employeeId);
            assignCommand.Parameters.AddWithValue("@roleId", role.RoleID);
            assignCommand.ExecuteNonQuery();
            return true;
        }

        private static Role ReadRole(MySqlDataReader reader)
        {
            return new Role(
                reader.GetInt32("RoleID"),
                reader.GetString("Name"),
                reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString("DepartmentName"));
        }
    }
}
