using System.Windows;
using VaultViewer.Business;
using VaultViewer.ServiceLayer;

namespace VaultViewer.UI
{
    public partial class AdminWindow : Window
    {
        private readonly AdminpanelService adminpanelService = new();

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void GetEmployees()
        {
            try
            {
                EmployeeListBox.ItemsSource = adminpanelService.GetEmployees();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Werknemers konden niet worden geladen: {exception.Message}");
            }
        }

        private void BtnEmployeeRole(object sender, RoutedEventArgs e)
        {
            GetEmployees();
            EmployeeListBox.Visibility = Visibility.Visible;
            Btn_AddEmployeeRole.Visibility = Visibility.Visible;
            DeleteEmployeeBtn.Visibility = Visibility.Hidden;
        }

        private void BtnAddEmployeeRole(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is not Employee)
            {
                MessageBox.Show("Selecteer eerst een werknemer.");
                return;
            }

            AssignRolePopup.IsOpen = true;
        }

        private void AssignRole(string roleName)
        {
            if (EmployeeListBox.SelectedItem is not Employee employee)
            {
                MessageBox.Show("Selecteer eerst een werknemer.");
                return;
            }

            try
            {
                if (!adminpanelService.AssignRole(employee.EmployeeID, roleName))
                {
                    MessageBox.Show($"De rol '{roleName}' bestaat niet.");
                    return;
                }

                MessageBox.Show($"De rol '{roleName}' is toegewezen aan {employee.FirstName} {employee.LastName}.");
                AssignRolePopup.IsOpen = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"De rol kon niet worden toegewezen: {exception.Message}");
            }
        }

        private void AssignRole_Default(object sender, RoutedEventArgs e)
        {
            AssignRole("Default");
        }

        private void AssignRole_HR(object sender, RoutedEventArgs e)
        {
            AssignRole("HR");
        }

        private void AssignRole_Engineer(object sender, RoutedEventArgs e)
        {
            AssignRole("Engineering");
        }

        private void AssignRole_Admin(object sender, RoutedEventArgs e)
        {
            AssignRole("Admin");
        }

        private void CancelAssignRole(object sender, RoutedEventArgs e)
        {
            AssignRolePopup.IsOpen = false;
        }

        private void BtnDeleteEmployee(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is not Employee employee)
            {
                MessageBox.Show("Selecteer eerst een werknemer.");
                return;
            }

            try
            {
                adminpanelService.DeleteEmployee(employee.EmployeeID);
                GetEmployees();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"De werknemer kon niet worden verwijderd: {exception.Message}");
            }
        }

        private void ShowEmployees(object sender, RoutedEventArgs e)
        {
            EmployeeListBox.Visibility = Visibility.Visible;
            DeleteEmployeeBtn.Visibility = Visibility.Visible;
            Btn_AddEmployeeRole.Visibility = Visibility.Hidden;
            GetEmployees();
        }

        private void addEmployeePopup(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = !AddEmployeePopup.IsOpen;
        }

        private void SaveEmployee(object sender, RoutedEventArgs e)
        {
            if (DateOfBirthBox.SelectedDate is not DateTime dateOfBirth)
            {
                MessageBox.Show("Selecteer een geboortedatum.");
                return;
            }

            var employee = new Employee(
                0,
                FirstNameBox.Text,
                LastNameBox.Text,
                dateOfBirth,
                AddressLineBox.Text,
                null,
                PostalCodeBox.Text,
                PostalCityBox.Text,
                CountryBox.Text,
                DateTime.Today,
                0);

            try
            {
                adminpanelService.AddEmployee(employee);
                MessageBox.Show("De werknemer is toegevoegd.");
                AddEmployeePopup.IsOpen = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"De werknemer kon niet worden opgeslagen: {exception.Message}");
            }
        }

        private void CancelPopup(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = false;
        }
    }
}
