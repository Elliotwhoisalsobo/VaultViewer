using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace VaultViewer.DataAccessLayer
{
    public class EmployeeRepository
    {


        public class Employee
        {
            public int EmployeeId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public List<Employee> GetAllEmployees()
            {
                List<Employee> employees = new List<Employee>();

                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT EmployeeID, FirstName, LastName FROM employee";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = reader.GetInt32("EmployeeID"),
                                FirstName = reader.GetString("FirstName"),
                                LastName = reader.GetString("LastName")
                            });
                        }
                    }
                }

                return employees;
            }
        }
    }
}
