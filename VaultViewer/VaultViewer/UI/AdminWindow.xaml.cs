using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT EmployeeID, FirstName, LastName FROM employee WHERE IsDeleted = 0", conn);
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

        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }

        private void DeleteEmployee(object sender, RoutedEventArgs e)
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

        private void SaveEmployee(object sender, RoutedEventArgs e) // Save as employee data lter from routed eventargs
        {
            string FirstName = FirstNameBox.Text;
            string LastName = LastNameBox.Text;
            string AddressLine = AddressLineBox.Text;
            DateTime? DateOfBirth = DateOfBirthBox.SelectedDate;
            string PostalCode = PostalCodeBox.Text;
            string PostalCity = PostalCityBox.Text;
            string Country = CountryBox.Text;
            DateTime? EmploymentDate = EmploymentDateBox.SelectedDate;


            int isdeleted = 0; // make this functional later
            // string department = (DepartmentBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Make this add a record to the employeerole table later

            

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
