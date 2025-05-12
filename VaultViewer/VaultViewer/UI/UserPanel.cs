using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using VaultViewer.ServiceLayer;
using VaultViewer.DataAccessLayer;
using System.Data;
using System.Windows.Controls;
using VaultViewer.Business;
using System.IO;
using System.Text;

namespace VaultViewer.UI
{
    public partial class UserPanel : Window
    {
        private List<string> _userRoles;

        // Constructor that accepts the roles list
        public UserPanel(List<string> userRoles)
        {
            InitializeComponent();
            _userRoles = userRoles; // Store the roles for later use (e.g., controlling UI visibility)

            // Set up the UI based on user roles
            SetUpUIBasedOnRoles();
        }



        // Group buttons together 
        private List<Button> GroupOfButtons()
        {
            List<Button> groupOfButtons = new List<Button>
            {
            BtnEmployee,
            BtnHR,
            BtnEngineer,
            BtnAdmin
            };
            return groupOfButtons;
        }

        private List<DataGrid> GroupOfDatagrids()
        {
            List<DataGrid> groupOfDatagrids = new List<DataGrid>
            {
            EmployeeData,
            HRData,
            EngineerData,
            AdminData
            };
            return groupOfDatagrids;
        }

        // This method can be used to modify the UI based on roles
        private void SetUpUIBasedOnRoles()
        {
            foreach (var role in _userRoles)
            {
                if (role == "Default")
                {
                    BtnEmployee.Visibility = Visibility.Visible;
                }
                if (role == "HR")
                {
                    BtnHR.Visibility = Visibility.Visible;
                }
                if (role == "Engineering")
                {
                    BtnEngineer.Visibility = Visibility.Visible;
                }
                if (role == "Admin")
                {
                    // Efficencie : D
                    foreach (var button in GroupOfButtons())
                    {
                        button.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }

        private void ShowEmployeeData(object sender, RoutedEventArgs e)
        {
            // Remove previously displayed datagrids
            foreach (var datagrid in GroupOfDatagrids())
            {
                datagrid.Visibility = Visibility.Hidden;
            }


            // Put this into a list later
            BtnFilter.Visibility = Visibility.Visible;
            BtnSort.Visibility = Visibility.Visible;
            BtnExport.Visibility = Visibility.Visible; 


            EmployeeData.Visibility = Visibility.Visible; // dataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // establishing connection to DB
                    MessageBox.Show("Connected to db!"); // Working : DDDD
                    MySqlCommand cmd = new MySqlCommand("Select Name from customer", conn);
                    // Filling up dataset with data from DB
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_employeeData = new DataSet();
                    adp.Fill(dataset_employeeData, "LoadDataBinding");
                    
                    
                    // Binding DB to DataGrid (to display said data)
                    EmployeeData.ItemsSource = dataset_employeeData.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowHRData(object sender, RoutedEventArgs e)
        {
            
            foreach (var datagrid in GroupOfDatagrids())
            {
                datagrid.Visibility = Visibility.Hidden;
            }

            HRData.Visibility = Visibility.Visible; // DataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    MessageBox.Show("Connected to db!"); // Working : DDDD
                    MySqlCommand cmd = new MySqlCommand("Select * from employee", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_employee = new DataSet();
                    adp.Fill(dataset_employee, "LoadDataBinding");
                    // Bind to DataGrid
                    HRData.ItemsSource = dataset_employee.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowEngineerData(object sender, RoutedEventArgs e)
        {
            foreach (var datagrid in GroupOfDatagrids())
            {
                datagrid.Visibility = Visibility.Hidden;
            }

            EngineerData.Visibility = Visibility.Visible; // DataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    MessageBox.Show("Connected to db!"); // Working : DDDD
                    MySqlCommand cmd = new MySqlCommand("Select * from product", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_engineer = new DataSet();
                    adp.Fill(dataset_engineer, "LoadDataBinding");
                    // Bind to DataGrid
                    EngineerData.ItemsSource = dataset_engineer.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowAdminData(object sender, RoutedEventArgs e)
        {
            foreach (var datagrid in GroupOfDatagrids())
            {
                datagrid.Visibility = Visibility.Hidden;
            }

            AdminData.Visibility = Visibility.Visible; // DataGrid
            try
            { 
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    MessageBox.Show("Connected to db!"); 
                    MySqlCommand cmd = new MySqlCommand("Select * from employeelogin", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_admin = new DataSet();
                    adp.Fill(dataset_admin, "LoadDataBinding");
                    // Bind to DataGrid
                    AdminData.ItemsSource = dataset_admin.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        /*private void FilterData(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
        }

        private void OrderData(object sender, RoutedEventArgs e)
        {
            try
            {

            }
        }*/

        private void ExportData(object sender, RoutedEventArgs e)
        {
            var visibleDataGrid = GroupOfDatagrids().Where(x => x.Visibility == Visibility.Visible).Single();
            
            // Make case statement with individual methods for each unique datagridName


            switch (visibleDataGrid.Name)
            {
                case "EmployeeData":
                    ExportEmployee(visibleDataGrid);
                    break;


                case "EngineerData":
                    
                    break;


                case "HRData":
                    
                    break;


                case "AdminData":
                    
                    break;


            }
        }

        private void ExportEmployee(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Something went wrong, the data grid was not found : (");
                return;
            }

            List<string> lines = new List<string>();
            lines.Add(String.Join(',', dataGrid.Columns.Select(x => x.Header)));
            lines.AddRange(dataGrid.Items.Cast<DataRowView>().Select(x => x.Row.Field<string>("Name")));
            //var filepath = Directory.GetCurrentDirectory();
            var filepath = @"C:\Users\Elliot\OneDrive - Thomas More\Applied Data Intelligence\sem2\inspiration lab\project\VaultViewer\VaultViewer\VaultViewer\DataAccessLayer\Data\";
            var filename = "CustomerData.csv";

            using (var writer = new StreamWriter(Path.Combine(filepath, filename)))
            {
                lines.ForEach(x => writer.WriteLine(x));
            }
            MessageBox.Show("Data succesfully exported to .csv");
        }
    }
}
        
    

