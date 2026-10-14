using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VaultViewer.ServiceLayer;

namespace VaultViewer.UI
{
    public partial class LoginWindow : Window
    {
        private readonly LoginService loginService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (loginService.TestConnection(out var error))
            {
                MessageBox.Show("De databaseverbinding werkt.");
                return;
            }

            MessageBox.Show($"De databaseverbinding is mislukt:\n{error}");
        }

        private void BtnCreateUser(object sender, RoutedEventArgs e)
        {
            var result = loginService.CreateUser(txt_Username.Text, txt_Password.Password);
            var message = result switch
            {
                LoginService.CreateUserResult.Success => "De gebruiker is aangemaakt.",
                LoginService.CreateUserResult.MissingUsername => "De gebruikersnaam ontbreekt.",
                LoginService.CreateUserResult.MissingPassword => "Het wachtwoord ontbreekt.",
                LoginService.CreateUserResult.EmptyUsernameAndPassword => "De gebruikersnaam en het wachtwoord ontbreken.",
                LoginService.CreateUserResult.UsernameTooShort => "De gebruikersnaam moet minstens 3 tekens bevatten.",
                LoginService.CreateUserResult.PasswordTooShort => "Het wachtwoord moet minstens 6 tekens bevatten.",
                LoginService.CreateUserResult.EmployeeNotFound => "Er bestaat geen werknemer met deze voornaam.",
                LoginService.CreateUserResult.UserAlreadyExists => "Deze gebruiker heeft al een wachtwoord.",
                _ => "De gebruiker kon niet worden aangemaakt."
            };
            MessageBox.Show(message);
        }

        private void BtnAuthenticate(object sender, RoutedEventArgs e)
        {
            var username = txt_Username.Text;
            if (!loginService.Authenticate(username, txt_Password.Password, out var userRoles))
            {
                MessageBox.Show("De aanmeldgegevens zijn ongeldig.");
                return;
            }

            if (username == "admin")
            {
                MessageBox.Show("Aangemeld als beheerder.");
                AdminPopup.IsOpen = true;
                return;
            }

            OpenUserPanel(userRoles);
        }

        private void BtnOpenUserPanel(object sender, RoutedEventArgs e)
        {
            if (!loginService.Authenticate(txt_Username.Text, txt_Password.Password, out var userRoles))
            {
                MessageBox.Show("De aanmeldgegevens zijn ongeldig.");
                return;
            }

            OpenUserPanel(userRoles);
        }

        private void OpenUserPanel(List<string> userRoles)
        {
            new UserPanel(userRoles).Show();
            Close();
        }

        private void BtnOpenAdminPanel(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.FocusedElement is Button button)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void exit_button(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
