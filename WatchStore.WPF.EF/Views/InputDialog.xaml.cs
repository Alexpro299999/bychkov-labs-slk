using System.Windows;

namespace WatchShop.WPF.Views
{
    public partial class InputDialog : Window
    {
        public string ResponseText { get; private set; }

        public InputDialog(string question, string defaultText = "")
        {
            InitializeComponent();
            QuestionLabel.Content = question;
            ResponseTextBox.Text = defaultText;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = ResponseTextBox.Text;
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            ResponseTextBox.SelectAll();
            ResponseTextBox.Focus();
        }
    }
}