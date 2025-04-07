using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VaultViewer.ServiceLayer;

namespace VaultViewer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void BtnTestConnection_Click(object sender, RoutedEventArgs e) // click <-- event | sender = what created the event | routedeventargs <-- info of clicker (like role) (rarely used)
        {
            var a = Elliot.ActualHeight;
            // give datagrid a name "var a = datagridname.command
            var database = new LoginService();
            database.TestConnection();
        }

        private void BtnAuthenticate(object sender, RoutedEventArgs e)
        {
            var a = Authenticate.ActualHeight;
            var database = new LoginService();
            string Username = txt_Username.Text;
            string Password = txt_Password.Text;
            database.Authenticate(Username, Password);
            bool success = database.Authenticate(Username, Password);

            if (success)
            {
              MessageBox.Show("Login succesfull : D");
             }
            else
            {
              MessageBox.Show("Invalid login details : (.");
             }
        }
        // Buttons are instantiated or not depending on role of user
        // Make second window LoginWindow --> MainWindow





    }
}