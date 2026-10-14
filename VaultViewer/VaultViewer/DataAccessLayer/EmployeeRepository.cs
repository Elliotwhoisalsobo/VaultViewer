using MySql.Data.MySqlClient;
using VaultViewer.Business;

namespace VaultViewer.DataAccessLayer
{
    public class EmployeeRepository
    {
        private const string Columns = "EmployeeID, FirstName, LastName, DateOfBirth, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, EmploymentDate, CurrentMonthlySalary";

        public List<Employee> GetActive(int skip = 0, int? limit = null)
        {
            var employees = new List<Employee>();
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            var sql = $"SELECT {Columns} FROM employee WHERE IsDeleted = 0";
            if (limit.HasValue)
            {
                sql += " LIMIT @limit OFFSET @skip";
            }

            using var command = new MySqlCommand(sql, connection);
            if (limit.HasValue)
            {
                command.Parameters.AddWithValue("@limit", limit.Value);
                command.Parameters.AddWithValue("@skip", skip);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(ReadEmployee(reader));
            }

            return employees;
        }

        public Employee? GetByFirstName(string firstName)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            var sql = $"SELECT {Columns} FROM employee WHERE FirstName = @firstName AND IsDeleted = 0 LIMIT 1";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            using var reader = command.ExecuteReader();
            return reader.Read() ? ReadEmployee(reader) : null;
        }

        public void Add(Employee employee)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            using var idCommand = new MySqlCommand("SELECT COALESCE(MAX(EmployeeID), 0) + 1 FROM employee", connection);
            employee.EmployeeID = Convert.ToInt32(idCommand.ExecuteScalar());

            const string sql = @"INSERT INTO employee
                (EmployeeID, FirstName, LastName, DateOfBirth, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, EmploymentDate, CurrentMonthlySalary, IsDeleted)
                VALUES
                (@employeeId, @firstName, @lastName, @dateOfBirth, @addressLine1, @addressLine2, @postalCode, @postalCity, @country, @employmentDate, @salary, 0)";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeeId", employee.EmployeeID);
            command.Parameters.AddWithValue("@firstName", employee.FirstName);
            command.Parameters.AddWithValue("@lastName", employee.LastName);
            command.Parameters.AddWithValue("@dateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@addressLine1", employee.AddressLine1);
            command.Parameters.AddWithValue("@addressLine2", (object?)employee.AddressLine2 ?? DBNull.Value);
            command.Parameters.AddWithValue("@postalCode", employee.PostalCode);
            command.Parameters.AddWithValue("@postalCity", employee.PostalCity);
            command.Parameters.AddWithValue("@country", employee.Country);
            command.Parameters.AddWithValue("@employmentDate", employee.EmploymentDate);
            command.Parameters.AddWithValue("@salary", employee.CurrentMonthlySalary);
            command.ExecuteNonQuery();
        }

        public void Delete(int employeeId)
        {
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            using var employeeCommand = new MySqlCommand("UPDATE employee SET IsDeleted = 1 WHERE EmployeeID = @employeeId", connection, transaction);
            employeeCommand.Parameters.AddWithValue("@employeeId", employeeId);
            employeeCommand.ExecuteNonQuery();

            using var loginCommand = new MySqlCommand("DELETE FROM employeelogin WHERE EmployeeID = @employeeId", connection, transaction);
            loginCommand.Parameters.AddWithValue("@employeeId", employeeId);
            loginCommand.ExecuteNonQuery();

            transaction.Commit();
        }

        private static Employee ReadEmployee(MySqlDataReader reader)
        {
            return new Employee(
                reader.GetInt32("EmployeeID"),
                reader.GetString("FirstName"),
                reader.GetString("LastName"),
                reader.GetDateTime("DateOfBirth"),
                reader.GetString("AddressLine1"),
                reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString("AddressLine2"),
                reader.GetString("PostalCode"),
                reader.GetString("PostalCity"),
                reader.GetString("Country"),
                reader.GetDateTime("EmploymentDate"),
                reader.IsDBNull(reader.GetOrdinal("CurrentMonthlySalary")) ? 0 : reader.GetDecimal("CurrentMonthlySalary"));
        }
    }
}
