﻿using DocumentFormat.OpenXml.Bibliography;
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

        private int selectedEmployeeId = -1; // Global variable to get the employeeID from the ListBox (for popup)

        public AdminWindow()
        {
            InitializeComponent();
        }

        // helper functions:
        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }
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
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData; // Initiatiate the currently selected employee record from the ListBox
            if (employee == null)
            {
                MessageBox.Show("Please select an employee first");
                return;
            }
            else

            
            //GetEmployeesDataTable(); // THIS BITCH RELOADED MY SHIT CAUSING EMPLOYEE ID TO BE -1 
            AddEmployeeRole(selectedEmployeeId); // WORKS !!!
        }

        private void AddEmployeeRole(int EmployeeID)
        {

            AssignRolePopup.IsOpen = true;

        }
        private void AssignRoleToEmployee(int employeeId, string roleName)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();

                    // Step 1: Get RoleID from Role table based on role name
                    string getRoleIdQuery = "SELECT RoleID FROM Role WHERE Name = @Name LIMIT 1";
                    int roleId;

                    using (var cmd = new MySqlCommand(getRoleIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", roleName);
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show($"Role '{roleName}' not found in the Role table.");
                            return;
                        }

                        roleId = Convert.ToInt32(result);
                    }

                    // Step 2: Insert into Employeerole table
                    string insertQuery = "INSERT INTO Employeerole (EmployeeID, RoleID) VALUES (@EmployeeID, @RoleID)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@RoleID", roleId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Role '{roleName}' assigned to employee ID {employeeId} successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error assigning role: " + ex.Message);
            }
        }


        private void AssignRole_Default(object sender, RoutedEventArgs e)
            {
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData;
            AssignRoleToEmployee(selectedEmployeeId, "Default");
            AssignRolePopup.IsOpen = false;
        }
        private void AssignRole_HR(object sender, RoutedEventArgs e)
        {
            AssignRoleToEmployee(selectedEmployeeId, "HR");
            AssignRolePopup.IsOpen = false;
        }
        private void AssignRole_Engineer(object sender, RoutedEventArgs e)
        {
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData;
            if (employee == null)
            {
                MessageBox.Show("Please select an employee.");
                return;
            }
            AssignRoleToEmployee(employee.Id, "Engineering");
            AssignRolePopup.IsOpen = false;
        }
        private void AssignRole_Admin(object sender, RoutedEventArgs e)
        {
            AssignRoleToEmployee(selectedEmployeeId, "Admin");
            AssignRolePopup.IsOpen = false;
        }
        private void CancelAssignRole(object sender, RoutedEventArgs e)
        {
            AssignRolePopup.IsOpen = false;
        }



    private void BtnDeleteEmployee(object sender, RoutedEventArgs e)
    {
        //var emplyee1 = (EmployeeViewData)EmployeeListBox.SelectedItem;
            var employee = EmployeeListBox.SelectedItem as EmployeeViewData;
            if (employee == null)
            {
                MessageBox.Show("Please select an Employee first.");
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

                    // Begin a transaction to ensure both queries succeed or fail together
                    using (var transaction = conn.BeginTransaction())
                    {
                        // 1. Soft delete the employee
                        string employeeDeleteQuery = "UPDATE employee SET IsDeleted = 1 WHERE EmployeeID = @EmployeeID";
                        using (var cmd1 = new MySqlCommand(employeeDeleteQuery, conn, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                            cmd1.ExecuteNonQuery();
                        }

                        // 2. Hard delete the employeelogin record (so they can't log in anymore)
                        string loginDeleteQuery = "DELETE FROM employeelogin WHERE EmployeeID = @EmployeeID";
                        using (var cmd2 = new MySqlCommand(loginDeleteQuery, conn, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                            cmd2.ExecuteNonQuery();
                        }

                        // Commit both changes
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
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

                    string insertQuery = @"INSERT INTO employee (EmployeeID, FirstName, LastName, AddressLine1, DateOfBirth, EmploymentDate, PostalCode, PostalCity, Country, IsDeleted)
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
                return;
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
        ////                    AssignRoleToEmployee(selectedEmployeeId, "Default");
        //    AssignRolePopup.IsOpen = false;
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
