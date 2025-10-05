using System;
using System.Linq;
using System.Windows;
using WatchStore.DataAccess.Models;
using WatchStore.DataAccess.Repositories;

namespace WatchStore.WPF.Views
{
    public partial class AddEditWatchWindow : Window
    {
        public Watch CurrentWatch { get; private set; }
        private readonly ManufacturerRepository _manufacturerRepo = new ManufacturerRepository();

        public AddEditWatchWindow(Watch watch = null)
        {
            InitializeComponent();
            CurrentWatch = watch ?? new Watch();
            LoadManufacturers();

            if (watch != null)
            {
                ModelTextBox.Text = watch.WatchModel;
                TypeTextBox.Text = watch.WatchType;
                PriceTextBox.Text = watch.Price.ToString();

                var selectedManufacturer = ManufacturerComboBox.Items.OfType<Manufacturer>()
                    .FirstOrDefault(m => m.ID == watch.ManufacturerID);
                if (selectedManufacturer != null)
                {
                    ManufacturerComboBox.SelectedItem = selectedManufacturer;
                }
            }
        }

        private void LoadManufacturers()
        {
            try
            {
                ManufacturerComboBox.ItemsSource = _manufacturerRepo.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить производителей: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                string.IsNullOrWhiteSpace(TypeTextBox.Text) ||
                !decimal.TryParse(PriceTextBox.Text, out decimal price) ||
                ManufacturerComboBox.SelectedItem == null)
            {
                MessageBox.Show("Все поля должны быть корректно заполнены.");
                return;
            }

            CurrentWatch.WatchModel = ModelTextBox.Text;
            CurrentWatch.WatchType = TypeTextBox.Text;
            CurrentWatch.Price = price;
            CurrentWatch.ManufacturerID = (ManufacturerComboBox.SelectedItem as Manufacturer).ID;

            DialogResult = true;
        }
    }
}