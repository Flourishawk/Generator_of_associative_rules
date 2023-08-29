using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        internal CSVUpload upload;
        internal DatabaseInfo databaseInfo;
        List<double>[] csvData;
        bool enter;
        public Options()
        {
            InitializeComponent();
        }

        public Options(CSVUpload upload, List<double>[] parametr)
        {
            InitializeComponent();
            this.upload = upload;
            enter = true;
            this.csvData = parametr;
        }

        public Options(DatabaseInfo databaseInfo, List<double>[] parametr)
        {
            InitializeComponent();
            this.databaseInfo = databaseInfo;
            enter = false;
            this.csvData = parametr;
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
            if (enter)
            {
                upload.first = false;
                upload.Logo1.Source = new BitmapImage(new Uri("/Assets/Drag-And-Drop2.png", UriKind.RelativeOrAbsolute));
                upload.HelloText.Text = "Почнімо роботу з завантаження CSV - файлу";
                upload.checkType.IsChecked = false;
                upload.checkRead.IsChecked = false;
                upload.checkToSparse.IsChecked = false;
                upload.checkToRegular.IsChecked = false;
                upload.conf = new bool[] { false, false, false, false };
                this.Owner.Show();
                this.Hide();
            }
            else
            {
                this.Owner.Show();
                this.Hide();
            }
        }

        private void Apriori_MouseDown(object sender, MouseButtonEventArgs e)
        {

            double minSupport;
            double minConfidence;
            bool success = double.TryParse(supportTextBox.Text, out minSupport);
            bool success2 = double.TryParse(confidenceTextBox.Text, out minConfidence);

            if (success && success2)
            {
                if (csvData.Length > 0)
                {
                    foreach (var iter in csvData)
                    {
                        iter.RemoveAt(0);
                    }
                }

                var result2 = AprioriAlgorithm.RunApriori(csvData, minSupport, minConfidence);
                List<List<double>> antecedent = result2.Item1;
                List<List<double>> consequent = result2.Item2;
                int step = result2.Item3;
                long time = result2.Item4;
                List<double> confidence = new List<double>();
                List<double> support = new List<double>();

                int a = 0;

                for (int i = 0; i < antecedent.Count(); i++)
                {
                    confidence.Add(AprioriAlgorithm.CalculateConfidence(antecedent[i], consequent[i], csvData, 0));
                    List<double> combinedList = antecedent[i].Concat(consequent[i]).ToList();
                    support.Add(AprioriAlgorithm.CalculateSupport(combinedList, csvData, 0).Item1);
                }
                
                Final final = new Final(Tuple.Create(antecedent, consequent, result2.Item3, result2.Item4, confidence, support));
                final.Owner = this;
                final.Show();
                this.Hide();
            }
        }

        private void FpGrowth_MouseDown(object sender, MouseButtonEventArgs e)
        {

            double minSupport;
            bool success = double.TryParse(supportTextBox.Text, out minSupport);

            if (success)
            {
                if (csvData.Length > 0)
                {
                    foreach (var iter in csvData)
                    {
                        iter.RemoveAt(0);
                    }
                }
                var ruleInfo = FPGrowth.firstSelections(csvData, minSupport);
                Final final2 = new Final(ruleInfo);
                final2.Owner = this;
                final2.Show();
                this.Hide();
            }
        }
    }
}
