using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VaultViewer.ServiceLayer;

namespace VaultViewer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

        }


        // test db connection
        private void BtnTestConnection_Click(object sender, RoutedEventArgs e) // click <-- event | sender = what created the event | routedeventargs <-- info of clicker (like role) (rarely used)
        {
            var database = new LoginService();
            database.TestConnection();
        }

        private void BtnCreateUser(object sender, RoutedEventArgs e)
        {
            var database = new LoginService();
            string Username = txt_Username.Text;
            string Password = txt_Password.Password;
            bool success = database.CreateUser(Username, Password);

            if (success)
            {
                MessageBox.Show("User created succesfully : D");
            }
            else
            {
                MessageBox.Show("User creation unsuccesfull :(");
            }
        }

        private void BtnAuthenticate(object sender, RoutedEventArgs e)
        {
            var database = new LoginService();
            string Username = txt_Username.Text;
            string Password = txt_Password.Password;

            bool success = database.Authenticate(Username, Password, out List<string> userRoles);

            if (success && Username == "admin")
            {
                MessageBox.Show("Admin login succesfull.");
                AdminPopup.IsOpen = true;
            }

            else if (success)
            {
                MessageBox.Show("UserLogin successfull.");

                UserPanel userpanel = new UserPanel(userRoles);
                userpanel.Show();
                this.Close();
            }

            else
            {
                MessageBox.Show("Invalid login details : (");
            }
        }

        private void BtnOpenUserPanel(object sender, RoutedEventArgs e)
        {
            var database = new LoginService();
            string Username = txt_Username.Text;
            string Password = txt_Password.Password;

            database.Authenticate(Username, Password, out List<string> userRoles);

            UserPanel userpanel = new UserPanel(userRoles);
            userpanel.Show();
            this.Close(); // Current instance of loginwindow
        }

        private void BtnOpenAdminPanel(object sender, RoutedEventArgs e)
        {
            AdminWindow adminPanel = new AdminWindow();
            adminPanel.Show();
            this.Close();
        }

        // Key stuff (This stuff is really more complex then it needs to be imo...)
        // Esc = close window methods
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // If the focused control is a Button --> simulate a click event
                if (Keyboard.FocusedElement is Button button)
                {
                    button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
        private void exit_button(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if(e.Key == Key.Escape) 
            {
                exit_button(null, e);
            }


        }
    }
}





    