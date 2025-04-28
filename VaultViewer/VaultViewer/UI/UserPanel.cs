using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using VaultViewer.ServiceLayer;
using VaultViewer.DataAccessLayer;
using System.Data;

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

        // This method can be used to modify the UI based on roles
        private void SetUpUIBasedOnRoles()
        {
            foreach (var role in _userRoles)
            {
                if (role == "Default")
                {
                    BtnUser.Visibility = Visibility.Visible;
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
                    // Add all buttons visible (group) thingy?
                    BtnAdmin.Visibility = Visibility.Visible;

                }
            }
        }

        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }

        private void ShowUserData(object sender, RoutedEventArgs e)
        {
            UserData.Visibility = Visibility.Visible; // dataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    MessageBox.Show("Connected to db!"); // Working : DDDD
                    MySqlCommand cmd = new MySqlCommand("Select Name from customer", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_customer = new DataSet();
                    adp.Fill(dataset_customer, "LoadDataBinding");
                    // Bind to DataGrid
                    UserData.ItemsSource = dataset_customer.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowHRData(object sender, RoutedEventArgs e)
        {
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
            HRData.Visibility = Visibility.Visible; // DataGrid
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    // do stuff w connection
                    MessageBox.Show("Connected to db!"); // Working : DDDD
                    MySqlCommand cmd = new MySqlCommand("Select Name from customer", conn); // Change
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet dataset_customer = new DataSet();
                    adp.Fill(dataset_customer, "LoadDataBinding");
                    // Bind to DataGrid
                    AdminData.ItemsSource = dataset_customer.Tables["LoadDataBinding"]?.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
        
    

