using VaultViewer.Business;
using VaultViewer.DataAccessLayer;

namespace VaultViewer.ServiceLayer
{
    public class UserpanelService
    {
        private readonly CustomerRepository customerRepository = new();
        private readonly EmployeeRepository employeeRepository = new();
        private readonly LoginRepository loginRepository = new();
        private readonly ProductRepository productRepository = new();

        public List<Customer> GetCustomers(int? limit = null)
        {
            return customerRepository.GetAll(limit);
        }

        public List<Employee> GetEmployees(int? limit = null)
        {
            return employeeRepository.GetActive(0, limit);
        }

        public List<Product> GetProducts(int? limit = null)
        {
            return productRepository.GetAll(limit);
        }

        public List<Login> GetLogins(int? limit = null)
        {
            return loginRepository.GetAll(limit);
        }
    }
}
