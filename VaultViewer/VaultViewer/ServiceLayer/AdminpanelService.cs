using VaultViewer.Business;
using VaultViewer.DataAccessLayer;

namespace VaultViewer.ServiceLayer
{
    public class AdminpanelService
    {
        private readonly EmployeeRepository employeeRepository = new();
        private readonly RoleRepository roleRepository = new();

        public List<Employee> GetEmployees()
        {
            return employeeRepository.GetActive(4, 10000);
        }

        public void AddEmployee(Employee employee)
        {
            employeeRepository.Add(employee);
        }

        public void DeleteEmployee(int employeeId)
        {
            employeeRepository.Delete(employeeId);
        }

        public bool AssignRole(int employeeId, string roleName)
        {
            return roleRepository.AssignToEmployee(employeeId, roleName);
        }
    }
}
