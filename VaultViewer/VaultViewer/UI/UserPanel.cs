using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using VaultViewer.ServiceLayer;

namespace VaultViewer.UI
{
    public partial class UserPanel : Window
    {
        private readonly List<string> userRoles;
        private readonly UserpanelService userpanelService = new();

        public UserPanel(List<string> userRoles)
        {
            InitializeComponent();
            this.userRoles = userRoles;
            SetUpUIBasedOnRoles();
        }

        private Button[] DataButtons()
        {
            return new[] { BtnFilter, BtnExport };
        }

        private Button[] RoleButtons()
        {
            return new[] { BtnEmployee, BtnHR, BtnEngineer, BtnAdmin };
        }

        private DataGrid[] DataGrids()
        {
            return new[] { EmployeeData, HRData, EngineerData, AdminData };
        }

        private void SetUpUIBasedOnRoles()
        {
            foreach (var role in userRoles)
            {
                switch (role)
                {
                    case "Default":
                        BtnEmployee.Visibility = Visibility.Visible;
                        break;
                    case "HR":
                        BtnHR.Visibility = Visibility.Visible;
                        break;
                    case "Engineering":
                        BtnEngineer.Visibility = Visibility.Visible;
                        break;
                    case "Admin":
                        foreach (var button in RoleButtons())
                        {
                            button.Visibility = Visibility.Visible;
                        }
                        break;
                }
            }
        }

        private void BtnLogout(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void ShowEmployeeData(object sender, RoutedEventArgs e)
        {
            LoadData(EmployeeData, () => userpanelService.GetCustomers());
        }

        private void ShowHRData(object sender, RoutedEventArgs e)
        {
            LoadData(HRData, () => userpanelService.GetEmployees());
        }

        private void ShowEngineerData(object sender, RoutedEventArgs e)
        {
            LoadData(EngineerData, () => userpanelService.GetProducts());
        }

        private void ShowAdminData(object sender, RoutedEventArgs e)
        {
            LoadData(AdminData, () => userpanelService.GetLogins());
        }

        private void LoadData(DataGrid dataGrid, Func<IEnumerable> load)
        {
            foreach (var grid in DataGrids())
            {
                grid.Visibility = Visibility.Hidden;
            }

            foreach (var button in DataButtons())
            {
                button.Visibility = Visibility.Visible;
            }

            try
            {
                dataGrid.ItemsSource = load();
                dataGrid.Visibility = Visibility.Visible;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"De gegevens konden niet worden geladen: {exception.Message}");
            }
        }

        private void ExportDataOptions(object sender, RoutedEventArgs e)
        {
            radiobtn1.Visibility = Visibility.Visible;
            radiobtntext1.Visibility = Visibility.Visible;
            radiobtn2.Visibility = Visibility.Visible;
            radiobtntext2.Visibility = Visibility.Visible;
        }

        private void ExportData(object sender, RoutedEventArgs e)
        {
            var dataGrid = VisibleDataGrid();
            if (dataGrid == null)
            {
                MessageBox.Show("Er is geen gegevensraster zichtbaar.");
                return;
            }

            try
            {
                var (items, properties) = ExportDataFor(dataGrid);
                var lines = new List<string>
                {
                    string.Join(",", properties.Select(property => CsvValue(property.Name)))
                };
                lines.AddRange(items.Select(item => string.Join(",", properties.Select(property => CsvValue(property.GetValue(item))))));
                var fullPath = ExportPath(CsvFilename(dataGrid.Name));
                File.WriteAllLines(fullPath, lines);
                MessageBox.Show($"De gegevens zijn naar CSV geëxporteerd:\n{fullPath}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Exporteren naar CSV is mislukt: {exception.Message}");
            }
        }

        private void ExportDataExcel(object sender, RoutedEventArgs e)
        {
            var dataGrid = VisibleDataGrid();
            if (dataGrid == null)
            {
                MessageBox.Show("Er is geen gegevensraster zichtbaar.");
                return;
            }

            try
            {
                var (items, properties) = ExportDataFor(dataGrid);
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(SheetName(dataGrid.Name));

                for (var column = 0; column < properties.Length; column++)
                {
                    worksheet.Cell(1, column + 1).Value = properties[column].Name;
                }

                for (var row = 0; row < items.Count; row++)
                {
                    for (var column = 0; column < properties.Length; column++)
                    {
                        worksheet.Cell(row + 2, column + 1).Value = Convert.ToString(properties[column].GetValue(items[row]), CultureInfo.InvariantCulture) ?? "";
                    }
                }

                var fullPath = ExportPath(ExcelFilename(dataGrid.Name));
                workbook.SaveAs(fullPath);
                MessageBox.Show($"De gegevens zijn naar Excel geëxporteerd:\n{fullPath}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Exporteren naar Excel is mislukt: {exception.Message}");
            }
        }

        private void FilterData(object sender, RoutedEventArgs e)
        {
            LimitInput.Visibility = Visibility.Visible;
            var dataGrid = VisibleDataGrid();
            if (dataGrid == null)
            {
                MessageBox.Show("Er is geen gegevensraster zichtbaar.");
                return;
            }

            if (!int.TryParse(LimitInput.Text, out var limit) || limit <= 0)
            {
                MessageBox.Show("Voer een positief maximumaantal rijen in.");
                return;
            }

            switch (dataGrid.Name)
            {
                case "EmployeeData":
                    LoadData(dataGrid, () => userpanelService.GetCustomers(limit));
                    break;
                case "HRData":
                    LoadData(dataGrid, () => userpanelService.GetEmployees(limit));
                    break;
                case "EngineerData":
                    LoadData(dataGrid, () => userpanelService.GetProducts(limit));
                    break;
                case "AdminData":
                    LoadData(dataGrid, () => userpanelService.GetLogins(limit));
                    break;
            }
        }

        private DataGrid? VisibleDataGrid()
        {
            return DataGrids().FirstOrDefault(grid => grid.Visibility == Visibility.Visible);
        }

        private static (List<object> Items, PropertyInfo[] Properties) ExportDataFor(DataGrid dataGrid)
        {
            var items = dataGrid.ItemsSource?.Cast<object>().ToList() ?? new List<object>();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Er zijn geen gegevens om te exporteren.");
            }

            return (items, items[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public));
        }

        private static string CsvValue(object? value)
        {
            var text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
            return $"\"{text.Replace("\"", "\"\"")}\"";
        }

        private static string ExportPath(string filename)
        {
            var directory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "DataAccessLayer", "Data"));
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, filename);
        }

        private static string CsvFilename(string dataGridName)
        {
            return dataGridName switch
            {
                "EmployeeData" => "CustomerData.csv",
                "HRData" => "EmployeeData.csv",
                "EngineerData" => "ProductData.csv",
                "AdminData" => "LoginData.csv",
                _ => throw new InvalidOperationException("Onbekend gegevensraster.")
            };
        }

        private static string ExcelFilename(string dataGridName)
        {
            return Path.ChangeExtension(CsvFilename(dataGridName), ".xlsx");
        }

        private static string SheetName(string dataGridName)
        {
            return dataGridName switch
            {
                "EmployeeData" => "Klanten",
                "HRData" => "Werknemers",
                "EngineerData" => "Producten",
                "AdminData" => "Logins",
                _ => throw new InvalidOperationException("Onbekend gegevensraster.")
            };
        }
    }
}
