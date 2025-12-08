using System;
using System.Linq;
using System.Windows;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;

namespace WatchShop.WPF.Views
{
    public partial class AddEditStockWindow : Window
    {
        public Stock CurrentStock { get; private set; }
        private readonly WatchRepository _watchRepo = new WatchRepository();

        public AddEditStockWindow(Stock stock = null)
        {
            InitializeComponent();
            CurrentStock = stock ?? new Stock { DeliveryDate = DateTime.Now };
            LoadWatches();

            if (stock != null)
            {
                var selectedWatch = WatchComboBox.Items.OfType<Watch>().FirstOrDefault(w => w.ID == stock.WatchID);
                if (selectedWatch != null)
                {
                    WatchComboBox.SelectedItem = selectedWatch;
                }
                QuantityTextBox.Text = stock.Quantity.ToString();
                DatePicker.SelectedDate = stock.DeliveryDate;
            }
        }

        private void LoadWatches()
        {
            try
            {
                WatchComboBox.ItemsSource = _watchRepo.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить список часов: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WatchComboBox.SelectedItem == null ||
                !int.TryParse(QuantityTextBox.Text, out int quantity) ||
                DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Все поля должны быть корректно заполнены.");
                return;
            }

            CurrentStock.WatchID = (WatchComboBox.SelectedItem as Watch).ID;
            CurrentStock.Quantity = quantity;
            CurrentStock.DeliveryDate = DatePicker.SelectedDate.Value;

            DialogResult = true;
        }
    }
}