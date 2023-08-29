using System;
using System.Windows;

namespace WpfApp1
{
    public partial class DataBaseForm : Window
    {
        public string[] DatabaseNameTextbox
        {
            get
            {
                string[] arrString =
                {
                    databaseServerTextbox.Text,
                    databaseServerPortTextbox.Text,
                    databaseNameTextbox.Text,
                    databaseUsernameTextbox.Text,
                    databasePasswordTextbox.Password
                };
                return arrString;
            }
        }

        public DataBaseForm()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseInfo dataBaseinfo = new DatabaseInfo(this);
            dataBaseinfo.Owner = this;
            dataBaseinfo.Show();
            this.Hide();
        }

        private void mouse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
