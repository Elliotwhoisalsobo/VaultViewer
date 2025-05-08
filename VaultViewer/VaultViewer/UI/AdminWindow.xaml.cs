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
        public AdminWindow()
        {
            InitializeComponent();
        }

        // helper functions:
        private DataTable GetEmployeesDataTable()
        {
            var dataTable = new DataTable();
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT ID, Name FROM customer", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    adp.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return dataTable;
        }

        private void DeleteUser(int userId)
        {
            try
            {
                using (var conn = DatabaseConfig.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM customer WHERE ID = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", userId);
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
            EmployeeListBox.ItemsSource = GetEmployeesDataTable().DefaultView;
        }
        private void EmployeeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is DataRowView selectedRow)
            {
                string name = selectedRow["Name"].ToString();
                int id = Convert.ToInt32(selectedRow["ID"]); // assuming ID is available

                var confirm = MessageBox.Show($"Delete {name}?", "Confirm", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    DeleteUser(id);
                    EmployeeListBox.ItemsSource = GetEmployeesDataTable().DefaultView; // Refresh
                }
            }
        }
    }
}
