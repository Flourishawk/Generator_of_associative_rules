using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class DatabaseInfo : Window
    {
        internal DataBaseForm dataBaseForm;

        public DatabaseInfo()
        {
            InitializeComponent();
            this.buttonNext.IsEnabled = false;
        }

        public DatabaseInfo(DataBaseForm dataBaseForm)
        {
            InitializeComponent();
            this.dataBaseForm = dataBaseForm;
            this.buttonNext.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            List<double>[] csvData;
            string[] conStringElement = dataBaseForm.DatabaseNameTextbox;
            string strConnection =
                $"server={conStringElement[0]};port={conStringElement[1]};username={conStringElement[3]};password={conStringElement[4]};database={conStringElement[2]}";
            string query = "";

            if ((nameTable1.Text != nameTable2.Text && checkSparse.IsChecked == false)
            )
            {
                query = $"SELECT {nameColumn1.Text},{nameColumn2.Text} FROM {conStringElement[2]}.{nameTable1.Text}," + $"{conStringElement[2]}.{nameTable2.Text};";
            }
            else if((nameTable1.Text == nameTable2.Text && checkSparse.IsChecked == false))
            {
                query =$"SELECT {nameColumn1.Text},{nameColumn2.Text} FROM {conStringElement[2]}.{nameTable1.Text};";
            }else if (checkSparse.IsChecked == true)
            {
                query = $"SELECT * FROM {conStringElement[2]}.{nameTable1.Text};";
            }

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                string headerQuery = $"Show columns FROM {conStringElement[2]}.{nameTable1.Text}";
                List<string> csvHeader = DatabaseHelper.executeHeader(headerQuery, strConnection);
                csvData = DatabaseHelper.executeReader(query, strConnection);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                await CsvRecorder.CreateCsvFile(csvHeader, csvData, this, "1");
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                csvData = DatabaseHelper.executeReader(query, strConnection);
                List<string> csvHeader =  new List<string> { "Transaction", "item" };
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                
                csvData = await CsvRecorder.ConvertToRegular(csvData, this, "2");
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                csvData = DatabaseHelper.executeReader(query, strConnection);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                QuickSort.QuicksortContinue(csvData);
                csvData=CsvRecorder.CleaningTransaction(csvData);
                await CsvRecorder.ConvertToSparse(csvData, this, $"Transformed");
            }
            else if(checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                csvData = DatabaseHelper.executeReader(query, strConnection);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                QuickSort.QuicksortContinue(csvData);
                csvData = CsvRecorder.CleaningTransaction(csvData);
            }
            else
            {
                csvData = null;
            }
            /*
            foreach(var item in csvData)
            {
                MessageBox.Show(string.Join(",",item));
            }*/
            Options options = new Options(this,csvData);
            options.Owner = this;
            options.Show();
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

        private void CheckSparse_Checked(object sender, RoutedEventArgs e)
        {
            nameTable2.IsEnabled = false;
            nameColumn1.IsEnabled = false;
            nameColumn2.IsEnabled = false;
            checkToSparse.IsChecked = false;

            if(checkSparse.IsChecked == true && checkRead.IsChecked==true && checkToSparse.IsChecked==false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if(checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {

                    this.buttonNext.IsEnabled = true;

            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkRead_Checked(object sender, RoutedEventArgs e)
        {
            checkToSparse.IsChecked = false;
            checkToRegular.IsChecked = false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                    this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkToSparse_Checked(object sender, RoutedEventArgs e)
        {
            checkSparse.IsChecked = false;
            checkRead.IsChecked = false;
            checkToRegular.IsChecked= false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {

                    this.buttonNext.IsEnabled = true;

            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkToRegular_Checked(object sender, RoutedEventArgs e)
        {
            checkRead.IsChecked = false;
            checkToSparse.IsChecked= false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {

                    this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkSparse_Unchecked(object sender, RoutedEventArgs e)
        {
            nameTable2.IsEnabled = true;
            nameColumn1.IsEnabled = true;
            nameColumn2.IsEnabled = true;

            checkSparse.IsChecked = false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                    this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkRead_Unchecked(object sender, RoutedEventArgs e)
        {
            checkRead.IsChecked = false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                    this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkToSparse_Unchecked(object sender, RoutedEventArgs e)
        {
            checkToSparse.IsChecked = false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                    this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void checkToRegular_Unchecked(object sender, RoutedEventArgs e)
        {

            checkToRegular.IsChecked = false;

            if (checkSparse.IsChecked == true && checkRead.IsChecked == true && checkToSparse.IsChecked == false && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == true && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {

                    this.buttonNext.IsEnabled = true;

            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                this.buttonNext.IsEnabled = true;
            }
            else if (checkSparse.IsChecked == false && checkRead.IsChecked == false && checkToSparse.IsChecked == false && checkToRegular.IsChecked == true)
            {
                this.buttonNext.IsEnabled = true;
            }
            else
            {
                this.buttonNext.IsEnabled = false;
            }
        }

        private void textboxTableHeader_TextChanged(object sender, TextChangedEventArgs e)
        {
                this.buttonNext.IsEnabled = false;
        }

        private void textboxColumnHeader_TextChanged(object sender, TextChangedEventArgs e)
        {
                this.buttonNext.IsEnabled = false;
        }
    }
}
