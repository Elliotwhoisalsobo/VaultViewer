using System.Configuration;
using System.Data;
using System.Windows;

namespace VaultViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main(string[] args) // is this really needed?
        {
            App app = new App();
            app.InitializeComponent();
            app.Run(); // creates instance of app and runs it
        }
    }
}