using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Final.xaml
    /// </summary>
    public partial class Final : Window
    {
        Tuple<List<List<double>>, List<List<double>>, int, long,List<double>,List<double>> ruleApriori;
        (List<List<double>>, List<double>, int ,long) ruleFp;
        bool cond;
        public Final()
        {
            InitializeComponent();
        }

        public Final(Tuple<List<List<double>>,List<List<double>>,int,long,List<double>, List<double>> result2)
        {
            InitializeComponent();
            this.ruleApriori = result2;
            if (ruleFp.Item4 < 1000)
            {
                timeText.Content = $"{ ruleApriori.Item4} мс.";
            }
            else if (ruleFp.Item4 >= 1000 && ruleFp.Item4 < 60000)
            {
                timeText.Content = $"{ruleApriori.Item4/1000} с.";
            }
            else if (ruleFp.Item4 >= 60000 && ruleFp.Item4 < 36000000)
            {
                timeText.Content = $"{ruleApriori.Item4/60000} хв.";
            }
            else
            {
                timeText.Content = $"{ruleApriori.Item4/3600000} год.";
            }
            stepText.Content = $"{ruleApriori.Item3} кроків";
            ruleText.Content = $"{ruleApriori.Item1.Count} правил";
            cond = false;
        }

        public Final((List<List<double>>, List<double>, int , long) rule)
        {
            InitializeComponent();
            this.ruleFp = rule;
            if (ruleFp.Item4 < 1000)
            {
                timeText.Content = $"{ruleFp.Item4} мс.";
            }else if(ruleFp.Item4 >= 1000 && ruleFp.Item4 < 60000)
            {
                timeText.Content = $"{ruleFp.Item4/1000} с.";
            }else if (ruleFp.Item4 >= 60000 && ruleFp.Item4 < 36000000)
            {
                timeText.Content = $"{ruleFp.Item4 / 60000} хв.";
            }
            else
            {
                timeText.Content = $"{ruleFp.Item4 / 36000000} год.";
            }
            stepText.Content = $"{ruleFp.Item3} кроків";
            ruleText.Content = $"{ruleFp.Item1.Count} правил";
            cond = true;
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void mouse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainButtonBd_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Hide();
        }

        private async void MainButtonCSV_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (cond)
            {
                await CsvRecorder.CreateRuleFile((ruleFp.Item1, ruleFp.Item2));
            }
            else
            {
                await CsvRecorder.CreateRuleFile2(Tuple.Create(ruleApriori.Item1,ruleApriori.Item2,ruleApriori.Item5,ruleApriori.Item6));
            }
        }
    }
}
