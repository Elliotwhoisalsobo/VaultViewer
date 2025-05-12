using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultViewer.ServiceLayer;
using VaultViewer.Business; // unused ???

namespace VaultViewer.DataAccessLayer
{
    public class EmployeeRepository
    {


        public class Employee
        {
            public int EmployeeId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            // Add more fields as needed
            public List<Employee> GetAllEmployees()
            {
                List<Employee> employees = new List<Employee>();
                var loginService = new LoginService();

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
