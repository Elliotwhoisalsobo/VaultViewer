using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VaultViewer.Business;
using VaultViewer.DataAccessLayer;
using static VaultViewer.DataAccessLayer.EmployeeRepository;

namespace VaultViewer.UI
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        protected class EmployeeViewData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public AdminWindow()
        {
            InitializeComponent();
        }

        // helper functions:
        private void GetEmployeesDataTable()
        {

            var dataTable = new DataTable();
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    // We do not want to display default users (user, HR, Engineer, Admin, etc)
                    int skip = 4;  // Number of rows to skip
                    int take = 10000;  // Number of rows to fetch after skipping (Mandatory not optional)
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT EmployeeID, FirstName, LastName FROM employee WHERE IsDeleted = 0 " + "LIMIT @Take OFFSET @Skip", conn); // IT WORKSSSSSSS!!! : DDD (default employees are no longer visible !!!
                    cmd.Parameters.AddWithValue("@Skip", skip);
                    cmd.Parameters.AddWithValue("@Take", take);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(dataTable);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            var items = dataTable.Rows.Cast<DataRow>().Select(x => new EmployeeViewData { Id = x.Field<int>("EmployeeID"), Name = $"{x.Field<string>("FirstName")} {x.Field<string>("LastName")}" });
            
            EmployeeListBox.ItemsSource = items; // list of box items
        }

        private void BtnEmployeeRole(object sender, RoutedEventArgs e)
        {
            GetEmployeesDataTable();
            EmployeeListBox.Visibility = Visibility.Visible;
            Btn_AddEmployeeRole.Visibility = Visibility.Visible;
            DeleteEmployeeBtn.Visibility = Visibility.Hidden;
        }

        private void BtnAddEmployeeRole(object sender, RoutedEventArgs e)
        {
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData; //Get the employeeID from the currently selected employee (selected within the ListBox)
            if (employee != null)
            {
                MessageBox.Show("Please select an employee first");
                return;
            }
            AddEmployeeRole(employee.Id); // W I P !!!
            GetEmployeesDataTable();
        }

        private void AddEmployeeRole(int EmployeeID)
        {
            AssignRolePopup.Visibility = Visibility.Visible;

            //    switch (role) // RoleID
            //    {
            //        case x:
            //    try
            //    {
            //        using (var conn = DatabaseConfig.GetConnection())
            //        {
            //            conn.Open();
            //            //string query = "DELETE FROM employee WHERE EmployeeID = @EmployeeID";
            //            string query = "UPDATE employee SET IsDeleted = 1 WHERE EmployeeID = @EmployeeID";
            //            MySqlCommand cmd = new MySqlCommand(query, conn);
            //            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            //            cmd.ExecuteNonQuery();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
    }
        private void AssignRole_Default(object sender, RoutedEventArgs e)
            {
            throw new NotImplementedException();
            }
        private void AssignRole_HR(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void AssignRole_Engineer(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void AssignRole_Admin(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void CancelAssignRole(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void BtnLogout(object sender, RoutedEventArgs e)
    {
        LoginWindow lw = new LoginWindow();
        lw.Show();
        this.Close();
    }
    private void BtnDeleteEmployee(object sender, RoutedEventArgs e)
    {
        //var emplyee1 = (EmployeeViewData)EmployeeListBox.SelectedItem;
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData;
            if (employee == null)
            {
                MessageBox.Show("Is null");
                return;
            }
            DeleteUser(employee.Id);
            GetEmployeesDataTable();
        }



        private void DeleteUser(int EmployeeID)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    //string query = "DELETE FROM employee WHERE EmployeeID = @EmployeeID";
                    string query = "UPDATE employee SET IsDeleted = 1 WHERE EmployeeID = @EmployeeID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowEmployees(object sender, RoutedEventArgs e)
        {
            EmployeeListBox.Visibility = Visibility.Visible;
            DeleteEmployeeBtn.Visibility = Visibility.Visible;
            Btn_AddEmployeeRole.Visibility = Visibility.Hidden;
            GetEmployeesDataTable();
            //EmployeeListBox.ItemsSource = GetEmployeesDataTable().DefaultView; // Results in an ERROR "id not found"
        }


        private void EmployeeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //EmployeeListBox.SelectedItem = 
            var a = 1;
        }


        private void addEmployeePopup(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = !AddEmployeePopup.IsOpen;
        }

        private void SaveEmployee(object sender, RoutedEventArgs e) // Save as employee data
        {
            // Collecting the data
            string EmployeeID = ""; // get highest employeeid +1 (no autoincrement)
            string FirstName = FirstNameBox.Text;
            string LastName = LastNameBox.Text;
            string AddressLine = AddressLineBox.Text;
            DateTime? DateOfBirth = DateOfBirthBox.SelectedDate;
            DateTime? EmploymentDate = DateTime.Today;
            string PostalCode = PostalCodeBox.Text;
            string PostalCity = PostalCityBox.Text;
            string Country = CountryBox.Text;
            //DateTime? EmploymentDate = EmploymentDateBox.SelectedDate;

            int isdeleted = 0; // make this functional later
                               // string department = (DepartmentBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Make this add a record to the employeerole table later

            // Putting the data into the MYSQLDB:

            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();

                    // 1. Get highest EmployeeID
                    string getMaxIdQuery = "SELECT MAX(EmployeeID) FROM employee";
                    using (MySqlCommand getMaxCmd = new MySqlCommand(getMaxIdQuery, conn))
                    {
                        object result = getMaxCmd.ExecuteScalar();
                        int maxId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        // The employeeID we will insert will be the current highest + 1 (we cannot autoincrement because of foreign key restraints
                        EmployeeID = (maxId + 1).ToString();
                    }

                    // 2. Insert the new employee

                    string insertQuery = @"INSERT INTO employee (EmployeeID, FirstName, LastName, AddressLine1, EmploymentDate, DateOfBirth, PostalCode, PostalCity, Country, IsDeleted)
                                   VALUES (@EmployeeID, @FirstName, @LastName, @AddressLine1, @DateOfBirth, @EmploymentDate ,@PostalCode, @PostalCity, @Country, @IsDeleted)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        insertCmd.Parameters.AddWithValue("@FirstName", FirstName);
                        insertCmd.Parameters.AddWithValue("@LastName", LastName);
                        insertCmd.Parameters.AddWithValue("@AddressLine1", AddressLine);
                        insertCmd.Parameters.AddWithValue("@DateOfBirth", (object)DateOfBirth ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@EmploymentDate", EmploymentDate); // fix le bug latur (this is dateofbirth for some reason lol)
                        insertCmd.Parameters.AddWithValue("@PostalCode", PostalCode);
                        insertCmd.Parameters.AddWithValue("@PostalCity", PostalCity);
                        insertCmd.Parameters.AddWithValue("@Country", Country);
                        insertCmd.Parameters.AddWithValue("@IsDeleted", isdeleted);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving employee: " + ex.Message);
            }
            MessageBox.Show("EmployeeRecord added succesfully!");
            AddEmployeePopup.IsOpen = false;
        }

        private void CancelPopup(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = false;
        }

        //   private void addEmployee(int EmployeeID)
        //    {
        //        EmployeeListBox.Items.Add(EmployeeID);
        //        throw new NotImplementedException();
        //    }

        //    private void addEmployeeToDB(int EmployeeID)
        //    {

        //    try
        //    {
        //        using (var conn = DatabaseConfig.GetConnection())
        //        {
        //            conn.Open();
        //            string query = "UPDATE employee SET IsDeleted = 1 WHERE EmployeeID = @EmployeeID";
        //            MySqlCommand cmd = new MySqlCommand(query, conn);
        //            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


    }
}
