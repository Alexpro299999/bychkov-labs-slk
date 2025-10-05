using System.Windows;
using WatchStore.DataAccess.Models;

namespace WatchStore.WPF.Views
{
    public partial class AddEditManufacturerWindow : Window
    {
        public Manufacturer CurrentManufacturer { get; private set; }

        public AddEditManufacturerWindow(Manufacturer manufacturer = null)
        {
            InitializeComponent();
            CurrentManufacturer = manufacturer ?? new Manufacturer();
            if (manufacturer != null)
            {
                NameTextBox.Text = manufacturer.ManufacturerName;
                CountryTextBox.Text = manufacturer.Country;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }
            CurrentManufacturer.ManufacturerName = NameTextBox.Text;
            CurrentManufacturer.Country = CountryTextBox.Text;
            DialogResult = true;
        }
    }
}
