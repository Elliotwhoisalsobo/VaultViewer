using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultViewer.ServiceLayer;
using VaultViewer.Business;
using VaultViewer.DataAccessLayer;
using System.Configuration;
using System.Windows.Controls;
using System.IO;

//using DevExpress.Mvvm; // external library
namespace VaultViewer.ServiceLayer
{
    public class UserpanelService
    {

        // Get data from datagrid
        public IEnumerable<Customer> GetCustomersFromDataGrid(DataGrid dataGrid)
        {
            return dataGrid.ItemsSource.Cast<Customer>();
        }


        // Placeholders
        public IEnumerable<Customer> GetConnectionString()
        {
            throw new NotImplementedException();
            //return connectionString;
        }

        public IEnumerable<Customer> GetCustomers(IEnumerable<Customer> list_customers) // IEnumerable 1 = return type | 2 = parameter
        {
            //throw new NotImplementedException();
            return list_customers;
        }



        public IEnumerable<Employee> GetEmployees(IEnumerable<Employee> list_employees)
        {
            // throw new NotImplementedException();
            return list_employees;
            
            //return MySqlConnection.GetEmployees(); // _ = object of sqlrepos
        }

        }
    }

