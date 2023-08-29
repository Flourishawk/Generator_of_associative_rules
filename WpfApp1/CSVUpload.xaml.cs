using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class CSVUpload : Window
    {
        string nameFile;
        string[] filePath;
        List<string> csvHeader;
        List<double>[] csvData;
        public bool[] conf =  {false,false,false,false};
        public bool first;

        public CSVUpload()
        {
            InitializeComponent();
            passiveButton.IsEnabled = false;
        }

        private void mouse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.None;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                filePath = (string[])e.Data.GetData(DataFormats.FileDrop);

                    // Перевірка розширення файлу
                    string extension = Path.GetExtension(filePath[0]);
                    if (extension == ".csv")
                    {
                        try
                        {
                            // Зчитування назви CSV-файлу
                            nameFile = Path.GetFileNameWithoutExtension(filePath[0]);
                            // Збереження CSV-файлу у базову директорію
                            HelloText.Text = "Файл успішно завантажено";
                            Logo1.Source = new BitmapImage(new Uri("/Assets/Drag-And-Drop3.png", UriKind.RelativeOrAbsolute));
                            first = true;
                        if (CheckValue(conf) == 1 && first)
                        {
                            passiveButton.IsEnabled = true;
                        }
                        else if (CheckValue(conf) == 2 && first)
                        {
                            passiveButton.IsEnabled = true;
                        }
                        else if (CheckValue(conf) == 3 && first)
                        {
                            passiveButton.IsEnabled = true;
                        }
                        else if (CheckValue(conf) == 4 && first)
                        {
                            passiveButton.IsEnabled = true;
                        }
                        else if (CheckValue(conf) == 5 && first)
                        {
                            passiveButton.IsEnabled = true;
                        }
                        else
                        {
                            passiveButton.IsEnabled = false;
                        }
                    }catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message, "Помилка");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Дозволені тільки CSV-файли. Будь ласка, перетягніть правильний файл.", "Помилка");
                    }
            }
        }

        private async void passiveButton_Click_1(object sender, RoutedEventArgs e)
        {
            Options options;
            if (CheckValue(conf) == 1 && first)
            {
                (csvHeader, csvData) = await CsvRecorder.ReadCsvFile(this, $"{nameFile}", filePath[0]);
                //прибрати зайву строчку
                if (csvData.Count() > 0)
                {
                    Array.Resize(ref csvData, csvData.Length - 1);
                }
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                options = new Options(this, csvData);
            }
            else if (CheckValue(conf) == 2 && first)
            {

                (csvHeader, csvData) = await CsvRecorder.ReadCsvFile(this, $"{nameFile}", filePath[0]);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                await CsvRecorder.ConvertToSparse(csvData, this, $"Transformed {nameFile}");
                if (csvData.Count() > 0)
                {
                    Array.Resize(ref csvData, csvData.Length - 1);
                }
                options = new Options(this, csvData);
            }
            else if (CheckValue(conf) == 3 && first)
            {
                (csvHeader, csvData) = await CsvRecorder.ReadCsvFile(this, $"{nameFile}", filePath[0]);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                await CsvRecorder.ConvertToRegular(csvHeader, csvData, this, $"Transformed {nameFile}");
                if (csvData.Count() > 0)
                {
                    Array.Resize(ref csvData, csvData.Length - 1);
                }
                options = new Options(this, csvData);
            }
            else if (CheckValue(conf) == 4 && first)
            {
                (csvHeader, csvData) = await CsvRecorder.ReadCsvFile(this, $"{nameFile}", filePath[0]);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                QuickSort.QuicksortContinue(csvData);

                if (csvData.Count() > 0)
                {
                    Array.Resize(ref csvData, csvData.Length - 1);
                }


                csvData = CsvRecorder.CleaningTransaction(csvData);
                foreach (var iter in csvData)
                {
                    MessageBox.Show(string.Join(",", iter));
                }
                await CsvRecorder.ConvertToSparse(csvData, this, $"Transformed {nameFile}");
                foreach (var iter in csvData)
                {
                    MessageBox.Show(string.Join(",", iter));
                }
                options = new Options(this, csvData);
            }
            else if (CheckValue(conf) == 5 && first)
            {
                (csvHeader, csvData) = await CsvRecorder.ReadCsvFile(this, $"{nameFile}", filePath[0]);
                QuickSort.Quicksort(csvData, 0, csvData.Count() - 2);
                QuickSort.QuicksortContinue(csvData);
                if (csvData.Count() > 0)
                {
                    Array.Resize(ref csvData, csvData.Length - 1);
                }
                csvData = CsvRecorder.CleaningTransaction(csvData);
                options = new Options(this, csvData);
            }
            else
            {
                options = null;
            }
            options.Owner = this;
            options.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private int CheckValue(bool[] booling)
        {

            switch (conf)
            {
                case bool[] array when array[0] && array[1] && !array[2] && !array[3]:
                    return 1;
                case bool[] array when array[0] && !array[1] && array[2] && !array[3]:
                    return 2;
                case bool[] array when array[0] && !array[1] && !array[2] && array[3]:
                    return 3;
                case bool[] array when !array[0] && !array[1] && array[2] && !array[3]:
                    return 4;
                case bool[] array when !array[0] && !array[1] && !array[2] && array[3]:
                    return 5;
                default:
                    return 0;
            }
        }


        private void CheckType_Checked(object sender, RoutedEventArgs e)
        {
            conf[0] = true;
            if (CheckValue(conf)==1 && first){
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
            //----
            if(checkRead.IsChecked==true && checkToRegular.IsChecked == true && checkToSparse.IsChecked == false)
            {
                checkRead.IsChecked = false;
                checkToRegular.IsChecked = false;
                conf[1] = false;
                conf[3] = false;
            }else if(checkToRegular.IsChecked == true && checkToSparse.IsChecked == true && checkRead.IsChecked==false)
            {
                checkToSparse.IsChecked = false;
                checkToRegular.IsChecked = false;
                conf[2] = false;
                conf[3] = false;
            }
            else if (checkRead.IsChecked == true && checkToSparse.IsChecked == true && checkToRegular.IsChecked == false)
            {
                checkRead.IsChecked = false;
                checkToSparse.IsChecked = false;
                conf[1] = false;
                conf[2] = false;
            }else if(checkRead.IsChecked == true && checkToSparse.IsChecked == true && checkToRegular.IsChecked == true)
            {
                checkRead.IsChecked = false;
                checkToSparse.IsChecked = false;
                checkToRegular.IsChecked = false;
                conf[1] = false;
                conf[2] = false;
                conf[3] = false;
            }
        }


        private void CheckRead_Checked(object sender, RoutedEventArgs e)
        {
            conf[1] = true;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }

            checkType.IsChecked = true;
            conf[0] = true;
            conf[1] = true;
            checkToSparse.IsChecked= false;
            checkToRegular.IsChecked= false;
            conf[2]= false;
            conf[3]= false;
            checkRead.IsChecked= true;
            

        }

        private void checkToSparse_Checked(object sender, RoutedEventArgs e)
        {
            conf[2] = true;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
            //--

            checkRead.IsChecked = false;
            checkToRegular.IsChecked = false;
            conf[1] = false;
            conf[3] = false;
        }

        private void checkToRegular_Checked(object sender, RoutedEventArgs e)
        {
            conf[3] = true;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else {
                passiveButton.IsEnabled = false;
            }

            checkRead.IsChecked = false;
            checkToSparse.IsChecked = false;
            conf[1] = false;
            conf[2] = false;
        }



        private void checkType_Unchecked(object sender, RoutedEventArgs e)
        {
            conf[0] = false;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
        }

 
        private void checkRead_Unchecked(object sender, RoutedEventArgs e)
        {
            conf[1] = false;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
        }

        private void checkToSparse_Unchecked(object sender, RoutedEventArgs e)
        {
            conf[2] = false;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
        }

        private void checkToRegular_Unchecked(object sender, RoutedEventArgs e)
        {
            conf[3] = false;
            if (CheckValue(conf) == 1 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 2 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 3 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 4 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else if (CheckValue(conf) == 5 && first)
            {
                passiveButton.IsEnabled = true;
            }
            else
            {
                passiveButton.IsEnabled = false;
            }
        }
    }
}
