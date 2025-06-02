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
using ClosedXML.Excel;
using System.Windows.Data;
using System.Windows.Input;

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


        // HELPER FUNCTIONS

        // Group Employeebuttons together 
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
        // Group datagrids together
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

        // Group databuttons together
         private List<Button> GroupOfDataButtons()
        {
            List<Button> groupOfDataButtons = new List<Button>
            {
            //BtnSort,
            BtnFilter,
            BtnExport,
            };
            return groupOfDataButtons;
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

            foreach (var btn in GroupOfDataButtons())
            {
                btn.Visibility = Visibility.Visible;
            }
            //BtnFilter.Visibility = Visibility.Visible;
            //BtnSort.Visibility = Visibility.Visible;
            //BtnExport.Visibility = Visibility.Visible; 


            EmployeeData.Visibility = Visibility.Visible; // dataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // establishing connection to DB
                    //MessageBox.Show("Connected to db!"); // debug
                    MySqlCommand cmd = new MySqlCommand("Select * from customer", conn);
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

            foreach (var btn in GroupOfDataButtons())
            {
                btn.Visibility = Visibility.Visible;
            }

            HRData.Visibility = Visibility.Visible; // DataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    //MessageBox.Show("Connected to db!"); // debug
                    MySqlCommand cmd = new MySqlCommand("Select EmployeeID, FirstName, LastName, DateOfBirth, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, EmploymentDate from employee", conn);
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

            foreach (var btn in GroupOfDataButtons())
            {
                btn.Visibility = Visibility.Visible;
            }

            EngineerData.Visibility = Visibility.Visible; // DataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    //MessageBox.Show("Connected to db!"); // debug
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

            foreach (var btn in GroupOfDataButtons())
            {
                btn.Visibility = Visibility.Visible;
            }

            AdminData.Visibility = Visibility.Visible; // DataGrid
            try
            { 
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    //MessageBox.Show("Connected to db!"); // debug
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
        // Reveal radio buttons & text when "Export" btn is pressed
        private void ExportDataOptions(object sender, RoutedEventArgs e)
        {
            radiobtn1.Visibility = Visibility.Visible;
            radiobtntext1.Visibility = Visibility.Visible;
            radiobtn2.Visibility = Visibility.Visible;
            radiobtntext2.Visibility = Visibility.Visible;
        }

        private void ExportData(object sender, RoutedEventArgs e)
        {
            var visibleDataGrid = GroupOfDatagrids().Where(x => x.Visibility == Visibility.Visible).Single();

            if (visibleDataGrid == null)
            {
                MessageBox.Show("No datagrid is currently visible : (");
            }



            // Make case statement with individual methods for each unique datagridName
            switch (visibleDataGrid.Name)
            {
                case "EmployeeData":
                    ExportEmployee(visibleDataGrid);
                    break;


                case "EngineerData":
                    ExportEngineer(visibleDataGrid);
                    break;


                case "HRData":
                    ExportHR(visibleDataGrid);
                    break;


                case "AdminData":
                    ExportAdmin(visibleDataGrid);
                    break;


            }
        }
        private void ExportDataExcel(object sender, RoutedEventArgs e)
        {
            var visibleDataGrid = GroupOfDatagrids().FirstOrDefault(x => x.Visibility == Visibility.Visible);

            if (visibleDataGrid == null)
            {
                MessageBox.Show("No DataGrid is currently visible.");
                return;
            }

            // Use DataGrid name to determine filename/sheetname
            switch (visibleDataGrid.Name)
            {
                case "EmployeeData":
                    ExportToExcel(visibleDataGrid, "Employees", "EmployeeData.xlsx");
                    break;
                case "EngineerData":
                    ExportToExcel(visibleDataGrid, "Products", "EngineerData.xlsx");
                    break;
                case "HRData":
                    ExportToExcel(visibleDataGrid, "HR", "HRData.xlsx");
                    break;
                case "AdminData":
                    ExportToExcel(visibleDataGrid, "Admins", "AdminData.xlsx");
                    break;
                default:
                    MessageBox.Show("Unrecognized DataGrid.");
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
            lines.AddRange(dataGrid.Items.Cast<DataRowView>()
            .Select(x => $"{x.Row["CustomerID"]}, {x.Row["Name"]}, {x.Row["AddressLine1"]}, {x.Row["PostalCode"]}, {x.Row["PostalCity"]}, {x.Row["Country"]}, {x.Row["ContactPerson"]}, {x.Row["VATNumber"]}, {x.Row["PhoneNumber"]}, {x.Row["IsCompany"]}, {x.Row["IsActive"]}")
);
            //var filepath = Directory.GetCurrentDirectory();
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = Path.Combine(basePath, @"..\..\..\DataAccessLayer\Data\");
            filepath = Path.GetFullPath(filepath); // Resolves relative segments
            var filename = "CustomerData.csv";

            using (var writer = new StreamWriter(Path.Combine(filepath, filename)))
            {
                lines.ForEach(x => writer.WriteLine(x));
            }
            MessageBox.Show($"Data successfully exported to Csv:\n{filepath}");
        }

        private void ExportEngineer(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Something went wrong, the data grid was not found : (");
                return;
            }

            List<string> lines = new List<string>();
            lines.Add(String.Join(',', dataGrid.Columns.Select(x => x.Header)));

            lines.AddRange(
            dataGrid.Items
            .Cast<DataRowView>()
            .Select(x => $"{x.Row["ProductID"]}, {x.Row["Name"]}, {x.Row["Description"]}, {x.Row["Price"]}")
);

            //var filepath = Directory.GetCurrentDirectory();
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = Path.Combine(basePath, @"..\..\..\DataAccessLayer\Data\");
            filepath = Path.GetFullPath(filepath); // Resolves relative segments
            var filename = "EngineerData.csv";

            using (var writer = new StreamWriter(Path.Combine(filepath, filename)))
            {
                lines.ForEach(x => writer.WriteLine(x));
            }
            MessageBox.Show($"Data successfully exported to Csv:\n{filepath}");
        }

        private void ExportHR(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Something went wrong, the data grid was not found : (");
                return;
            }

            List<string> lines = new List<string>();
            lines.Add(String.Join(',', dataGrid.Columns.Select(x => x.Header)));
            lines.AddRange(
            dataGrid.Items
            .Cast<DataRowView>()
            .Select(x => $"{x.Row["EmployeeID"]}, {x.Row["FirstName"]}, {x.Row["LastName"]}, {x.Row["DateOfBirth"]} {x.Row["AddressLine1"]}, {x.Row["PostalCode"]}, {x.Row["PostalCity"]}, {x.Row["Country"]}, {x.Row["EmploymentDate"]}")
);
            //var filepath = Directory.GetCurrentDirectory();
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = Path.Combine(basePath, @"..\..\..\DataAccessLayer\Data\");
            filepath = Path.GetFullPath(filepath); // Resolves relative segments
            var filename = "CustomerData.csv";

            using (var writer = new StreamWriter(Path.Combine(filepath, filename)))
            {
                lines.ForEach(x => writer.WriteLine(x));
            }
            MessageBox.Show($"Data successfully exported to Csv:\n{filepath}");
        }

        private void ExportAdmin(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Something went wrong, the data grid was not found : (");
                return;
            }

            List<string> lines = new List<string>();
            lines.Add(String.Join(',', dataGrid.Columns.Select(x => x.Header)));
            lines.AddRange(dataGrid.Items
            .Cast<DataRowView>()
            .Select(x => $"{x.Row["UserName"]}, {x.Row["PasswordHash"]}, {x.Row["EmployeeID"]}")
);
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = Path.Combine(basePath, @"..\..\..\DataAccessLayer\Data\");
            filepath = Path.GetFullPath(filepath); // Resolves relative segments
            //var filepath = @"C:\Users\Elliot\OneDrive - Thomas More\Applied Data Intelligence\sem2\inspiration lab\project\VaultViewer\VaultViewer\VaultViewer\DataAccessLayer\Data\";
            var filename = "CustomerData.csv";

            using (var writer = new StreamWriter(Path.Combine(filepath, filename)))
            {
                lines.ForEach(x => writer.WriteLine(x));
            }
            MessageBox.Show($"Data successfully exported to Csv:\n{filepath}");
        }

        private void ExportToExcel(DataGrid dataGrid, string sheetName, string filename)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("No data found to export.");
                return;
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Write headers <-- first row
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].Header?.ToString() ?? $"Column{i + 1}";
            }

            // Write data <-- second row and beyond
            int row = 2;
            foreach (DataRowView drv in dataGrid.Items)
            {
                for (int col = 0; col < dataGrid.Columns.Count; col++)
                {
                    var boundColumn = dataGrid.Columns[col] as DataGridBoundColumn;
                    var binding = boundColumn?.Binding as Binding;
                    var columnName = binding?.Path?.Path;

                    if (!string.IsNullOrEmpty(columnName))
                    {
                        worksheet.Cell(row, col + 1).Value = drv.Row[columnName]?.ToString();
                    }
                }
                row++;
            }

            // Save file

            // change later (to work for everyone not just me : )
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filepath = Path.Combine(basePath, @"..\..\..\DataAccessLayer\Data\");
            filepath = Path.GetFullPath(filepath); // Resolves relative segments
            //string exportPath = @"C:\Users\Elliot\OneDrive - Thomas More\Applied Data Intelligence\sem2\inspiration lab\project\VaultViewer\VaultViewer\VaultViewer\DataAccessLayer\Data\";
            string fullPath = Path.Combine(filepath, filename);

            workbook.SaveAs(fullPath);
            MessageBox.Show($"Data successfully exported to Excel:\n{fullPath}");
        }

        // Filter (LIMIT)
        private void FilterData(object sender, RoutedEventArgs e)
        {

            LimitInput.Visibility = Visibility.Visible;
            var visibleDataGrid = GroupOfDatagrids().FirstOrDefault(x => x.Visibility == Visibility.Visible);
            if (visibleDataGrid == null)
            {
                MessageBox.Show("No DataGrid is currently visible.");
                return;
            }

            if (!int.TryParse(LimitInput.Text, out int limit) || limit <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for limit.");
                return;
            }

            string query = visibleDataGrid.Name switch
            {   // I absolutely fucking love the fat squid (=>) lambda function : DDD
                "EmployeeData" => $"SELECT * FROM customer LIMIT {limit}",
                "EngineerData" => $"SELECT * FROM product LIMIT {limit}",
                "HRData" => $"SELECT * FROM employee LIMIT {limit}",
                "AdminData" => $"SELECT * FROM employeelogin LIMIT {limit}",
                _ => null
            };

            if (query == null)
            {
                MessageBox.Show("Unrecognized DataGrid.");
                return;
            }

            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adp.Fill(dataset, "FilteredData");
                    visibleDataGrid.ItemsSource = dataset.Tables["FilteredData"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
        
    

