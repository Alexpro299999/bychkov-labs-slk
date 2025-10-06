using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using WatchStore.DataAccess;
using WatchStore.DataAccess.Models;
using WatchStore.DataAccess.Repositories;
using WatchStore.WPF.Services;
using WatchStore.WPF.Views;

namespace WatchStore.WPF
{
    public partial class MainWindow : Window
    {
        private readonly ManufacturerRepository _manufacturerRepo;
        private readonly WatchRepository _watchRepo;
        private readonly StockRepository _stockRepo;

        public MainWindow()
        {
            InitializeComponent();
            using (var context = new WatchDbContext()) { context.Database.Migrate(); }
            _manufacturerRepo = new ManufacturerRepository();
            _watchRepo = new WatchRepository();
            _stockRepo = new StockRepository();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                ManufacturersGrid.ItemsSource = null;
                WatchesGrid.ItemsSource = null;
                StockGrid.ItemsSource = null;

                var allManufacturers = _manufacturerRepo.GetAll();
                var allWatches = _watchRepo.GetAll();
                var allStock = _stockRepo.GetAll();

                var stockForView = from stockItem in allStock
                                   join watchItem in allWatches on stockItem.WatchID equals watchItem.ID
                                   select new StockView
                                   {
                                       StockID = stockItem.ID,
                                       WatchModel = watchItem.WatchModel,
                                       Quantity = stockItem.Quantity,
                                       DeliveryDate = stockItem.DeliveryDate
                                   };

                ManufacturersGrid.ItemsSource = allManufacturers;
                WatchesGrid.ItemsSource = allWatches;
                StockGrid.ItemsSource = stockForView.ToList();
            }
            catch (Exception ex) { MessageBox.Show($"Критическая ошибка загрузки данных: {ex.Message}"); }
        }

        private void AddManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditManufacturerWindow();
            if (addWindow.ShowDialog() == true)
            {
                try { _manufacturerRepo.Add(addWindow.CurrentManufacturer); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка добавления: {ex.Message}"); }
            }
        }

        private void EditManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var selected = ManufacturersGrid.SelectedItem as Manufacturer;
            if (selected == null) { MessageBox.Show("Выберите запись для редактирования."); return; }
            var editWindow = new AddEditManufacturerWindow(selected);
            if (editWindow.ShowDialog() == true)
            {
                try { _manufacturerRepo.Update(editWindow.CurrentManufacturer); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка изменения: {ex.Message}"); }
            }
        }

        private void DeleteManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var selected = ManufacturersGrid.SelectedItem as Manufacturer;
            if (selected == null) { MessageBox.Show("Выберите запись для удаления."); return; }

            try
            {
                int watchCount = _manufacturerRepo.CountWatchesByManufacturer(selected.ID);
                if (watchCount > 0)
                {
                    MessageBox.Show($"Нельзя удалить производителя '{selected.ManufacturerName}', так как за ним закреплено {watchCount} моделей часов.", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("Вы уверены?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _manufacturerRepo.Delete(selected.ID);
                    LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка удаления: {ex.Message}"); }
        }

        private void AddWatch_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWatchWindow();
            if (addWindow.ShowDialog() == true)
            {
                try { _watchRepo.Add(addWindow.CurrentWatch); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка добавления: {ex.Message}"); }
            }
        }

        private void EditWatch_Click(object sender, RoutedEventArgs e)
        {
            var selected = WatchesGrid.SelectedItem as Watch;
            if (selected == null) { MessageBox.Show("Выберите запись для редактирования."); return; }
            var editWindow = new AddEditWatchWindow(selected);
            if (editWindow.ShowDialog() == true)
            {
                try { _watchRepo.Update(editWindow.CurrentWatch); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка изменения: {ex.Message}"); }
            }
        }

        private void DeleteWatch_Click(object sender, RoutedEventArgs e)
        {
            var selected = WatchesGrid.SelectedItem as Watch;
            if (selected == null) { MessageBox.Show("Выберите запись для удаления."); return; }
            if (MessageBox.Show("Вы уверены?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try { _watchRepo.Delete(selected.ID); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка удаления: {ex.Message}"); }
            }
        }

        private void AddStock_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditStockWindow();
            if (addWindow.ShowDialog() == true)
            {
                try { _stockRepo.Add(addWindow.CurrentStock); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка добавления: {ex.Message}"); }
            }
        }

        private void EditStock_Click(object sender, RoutedEventArgs e)
        {
            var selectedView = StockGrid.SelectedItem as StockView;
            if (selectedView == null) { MessageBox.Show("Выберите запись для редактирования."); return; }
            var originalStock = _stockRepo.GetAll().FirstOrDefault(s => s.ID == selectedView.StockID);
            if (originalStock == null) { MessageBox.Show("Не удалось найти исходную запись."); return; }
            var editWindow = new AddEditStockWindow(originalStock);
            if (editWindow.ShowDialog() == true)
            {
                try { _stockRepo.Update(editWindow.CurrentStock); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка изменения: {ex.Message}"); }
            }
        }

        private void DeleteStock_Click(object sender, RoutedEventArgs e)
        {
            var selectedView = StockGrid.SelectedItem as StockView;
            if (selectedView == null) { MessageBox.Show("Выберите запись для удаления."); return; }
            if (MessageBox.Show("Вы уверены?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try { _stockRepo.Delete(selectedView.StockID); LoadData(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка удаления: {ex.Message}"); }
            }
        }

        private void QueryWatchesByType_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Введите тип часов (кварцевые/механические):", "кварцевые");
            if (dialog.ShowDialog() == true) { ResultsGrid.ItemsSource = _watchRepo.GetWatchesByType(dialog.ResponseText); }
        }

        private void QueryMechanicalCheaperThan_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Введите максимальную цену:", "50000");
            if (dialog.ShowDialog() == true)
            {
                if (decimal.TryParse(dialog.ResponseText, out decimal price)) { ResultsGrid.ItemsSource = _watchRepo.GetMechanicalWatchesCheaperThan(price); }
                else { MessageBox.Show("Некорректный ввод. Введите число."); }
            }
        }

        private void QueryWatchesByCountry_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Введите страну:", "Швейцария");
            if (dialog.ShowDialog() == true)
            {
                var result = _watchRepo.GetWatchModelsByCountry(dialog.ResponseText);
                ResultsGrid.ItemsSource = result.Select(r => new { Марка = r }).ToList();
            }
        }

        private void QueryManufacturersByValue_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Введите макс. общую стоимость часов:", "100000");
            if (dialog.ShowDialog() == true)
            {
                if (decimal.TryParse(dialog.ResponseText, out decimal value)) { ResultsGrid.ItemsSource = _watchRepo.GetManufacturersByTotalValue(value); }
                else { MessageBox.Show("Некорректный ввод. Введите число."); }
            }
        }

        private void QueryWatchesWithTotalValue_Click(object sender, RoutedEventArgs e)
        {
            ResultsGrid.ItemsSource = _watchRepo.GetWatchesWithTotalValue();
        }

        private void AllWatchesReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = _watchRepo.GetAll();
                var generator = new WordReportGenerator();
                generator.GenerateAllWatchesReport(data);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка создания отчета: {ex.Message}"); }
        }

        private void ReportMechanicalWatches_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Сформировать отчет для часов дешевле, чем:", "50000");
            if (dialog.ShowDialog() == true)
            {
                if (decimal.TryParse(dialog.ResponseText, out decimal price))
                {
                    try
                    {
                        var data = _watchRepo.GetMechanicalWatchesCheaperThan(price);
                        var generator = new WordReportGenerator();
                        generator.GenerateFilteredWatchesReport(data, $"Отчет по механическим часам (дешевле {price:C})");
                    }
                    catch (Exception ex) { MessageBox.Show($"Ошибка создания отчета: {ex.Message}"); }
                }
                else { MessageBox.Show("Некорректный ввод. Введите число."); }
            }
        }

        private void ReportGrouped_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = _manufacturerRepo.GetGroupedProductionReportData();
                var generator = new WordReportGenerator();
                generator.GenerateGroupedManufacturerReport(data);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка создания отчета: {ex.Message}"); }
        }
    }
}