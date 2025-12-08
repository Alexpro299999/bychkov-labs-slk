using System.Collections.Generic;
using WatchShop.DataAccess.Models;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;

namespace WatchShop.WPF.Services
{
    public class WordReportGenerator
    {
        public void GenerateAllWatchesReport(List<Watch> data)
        {
            var app = new Word.Application();
            var doc = app.Documents.Add();

            Word.Paragraph titlePar = doc.Paragraphs.Add();
            titlePar.Range.Text = "Отчет по всем часам";
            titlePar.Range.Font.Bold = 1;
            titlePar.Range.Font.Size = 16;
            titlePar.Range.InsertParagraphAfter();

            Word.Table table = doc.Tables.Add(titlePar.Range, data.Count + 1, 3);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "Модель";
            table.Cell(1, 2).Range.Text = "Тип";
            table.Cell(1, 3).Range.Text = "Цена";

            for (int i = 0; i < data.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = data[i].WatchModel;
                table.Cell(i + 2, 2).Range.Text = data[i].WatchType;
                table.Cell(i + 2, 3).Range.Text = data[i].Price.ToString("C");
            }

            app.Visible = true;
        }

        public void GenerateFilteredWatchesReport(List<Watch> data, string title)
        {
            if (data == null || data.Count == 0)
            {
                System.Windows.MessageBox.Show("Нет данных для отчета.");
                return;
            }
            var app = new Word.Application();
            var doc = app.Documents.Add();

            Word.Paragraph titlePar = doc.Paragraphs.Add();
            titlePar.Range.Text = title;
            titlePar.Range.Font.Bold = 1;
            titlePar.Range.Font.Size = 16;
            titlePar.Range.InsertParagraphAfter();

            Word.Table table = doc.Tables.Add(titlePar.Range, data.Count + 1, 3);
            table.Borders.Enable = 1;
            table.Cell(1, 1).Range.Text = "Модель";
            table.Cell(1, 2).Range.Text = "Тип";
            table.Cell(1, 3).Range.Text = "Цена";

            for (int i = 0; i < data.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = data[i].WatchModel;
                table.Cell(i + 2, 2).Range.Text = data[i].WatchType;
                table.Cell(i + 2, 3).Range.Text = data[i].Price.ToString("C");
            }

            app.Visible = true;
        }

        public void GenerateGroupedManufacturerReport(List<DailyProductionCost> data)
        {
            if (data == null || data.Count == 0)
            {
                System.Windows.MessageBox.Show("Нет данных для отчета.");
                return;
            }
            var app = new Word.Application();
            var doc = app.Documents.Add();

            Word.Paragraph titlePar = doc.Paragraphs.Add();
            titlePar.Range.Text = "Группирующий отчет по стоимости произведенных часов";
            titlePar.Range.Font.Bold = 1;
            titlePar.Range.Font.Size = 16;
            titlePar.Range.InsertParagraphAfter();

            var groupedData = data.GroupBy(d => d.ManufacturerName);
            decimal grandTotal = 0;

            foreach (var group in groupedData)
            {
                Word.Paragraph manufacturerPar = doc.Paragraphs.Add();
                manufacturerPar.Range.Text = $"Производитель: {group.Key}";
                manufacturerPar.Range.Font.Bold = 1;
                manufacturerPar.Range.Font.Size = 14;
                manufacturerPar.Range.InsertParagraphAfter();

                var items = group.ToList();
                Word.Table table = doc.Tables.Add(manufacturerPar.Range, items.Count + 2, 2);
                table.Borders.Enable = 1;
                table.Cell(1, 1).Range.Text = "Дата";
                table.Cell(1, 2).Range.Text = "Стоимость";

                decimal manufacturerTotal = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    table.Cell(i + 2, 1).Range.Text = items[i].DeliveryDate.ToShortDateString();
                    table.Cell(i + 2, 2).Range.Text = items[i].TotalCost.ToString("C");
                    manufacturerTotal += items[i].TotalCost;
                }

                table.Cell(items.Count + 2, 1).Range.Text = "Итого по производителю:";
                table.Cell(items.Count + 2, 2).Range.Text = manufacturerTotal.ToString("C");
                table.Rows[items.Count + 2].Range.Font.Bold = 1;

                grandTotal += manufacturerTotal;
                doc.Paragraphs.Add();
            }

            Word.Paragraph finalTotalPar = doc.Paragraphs.Add();
            finalTotalPar.Range.Text = $"Общий итог по всем производителям: {grandTotal:C}";
            finalTotalPar.Range.Font.Bold = 1;
            finalTotalPar.Range.Font.Size = 14;

            app.Visible = true;
        }

    }
}