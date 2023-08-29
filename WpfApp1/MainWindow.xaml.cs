using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainButtonClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MainButtonBd_Click(object sender, RoutedEventArgs e)
        {
            DataBaseForm dataBaseForm = new DataBaseForm();
            dataBaseForm.Owner = this;
            dataBaseForm.Show();
            this.Hide();
        }

        protected void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MainButtonCSV_Click(object sender, RoutedEventArgs e)
        {
            CSVUpload CSVUpload = new CSVUpload();
            CSVUpload.Owner = this;
            CSVUpload.Show();
            this.Hide();
        }
    }
}
