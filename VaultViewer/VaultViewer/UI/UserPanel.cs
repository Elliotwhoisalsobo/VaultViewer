using System;
using System.Collections.Generic;
using System.Windows;
using VaultViewer.ServiceLayer;

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
                if (role == "Admin")
                {
                    // Show admin buttons or features
                    BtnAdmin.Visibility = Visibility.Visible;
                }
                if (role == "Default")
                {
                    // Show user-specific buttons or features
                    BtnUser.Visibility = Visibility.Visible;
                }
                if (role == "HR")
                {
                    BtnHR.Visibility = Visibility.Visible;
                }
                if (role == "Engineer")
                {
                    BtnUser.Visibility = Visibility.Visible;
                }
                // Add more role-based conditions as needed
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
            UserData.Visibility = Visibility.Visible;
        }
    }
}
