using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Windows.Documents;

namespace WpfApp1
{

    internal class CsvRecorder
    {
        /*ReadCsvFile Читає CSV файл для розрядженої матриці наступного формату
        idOrder,milk,meat... (header, який включає строкові значення)
        1,1,0...(data, яка зберігає int значення)
        та повертає кортедж з csvHeader та csvData*/
        
        public static async Task<(List<string>, List<double>[])> ReadCsvFile(
            CSVUpload database,
            string name,
            string path
        )
        {
            Encoding encoding = Encoding.UTF8;


            List<string> csvHeader = new List<string>();// Створення списку для зберігання Headerу csv файлу
            int count = System.IO.File.ReadAllLines(path).Length;
            List<double>[] transaction;

            // Відкриваємо новий потік для читання файлу
            using (var reader = new StreamReader(path, encoding))
            {
                // Створення списку для зберігання Data з csv файлу

                string line = reader.ReadLine();// Зчитуємо перший рядок в якому міститься Header
                var formatLine = line.Split(database.delimiter.Text.ToCharArray()[0]); // Розділяємо рядок на масив, використовуючи символ-роздільник
                transaction = new List<double>[count];


                for (int a = 0; a < count; a++)
                {
                    transaction[a] = new List<double>();
                }

                // Додаємо кожне значення з першого рядка в список заголовків стовпців
                foreach (var value in formatLine)
                {
                    csvHeader.Add(value);
                }

                // Створюємо цикл, який буде читати файл по рядках поки він не закінчиться

                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Replace(database.arrayDelimiter.Text[0].ToString(), "").Replace(database.arrayDelimiter.Text[1].ToString(), "");
                    formatLine = line.Split(database.delimiter.Text.ToCharArray()[0]); // Розділяємо рядок на масив, використовуючи символ-роздільник
                    // Проходимо по кожному елементу з розділеного рядка
                    for (int i = 0; i < formatLine.Count(); i++)
                    {
                        transaction[y].Add(double.Parse(formatLine[i]));// ,то додаємо число до списку csvData
                    }
                    y++;
                }
            }
            MessageBox.Show("Успіх");
            return (csvHeader, transaction);
        }
        /*
        public static async Task<(List<string>, List<double>[])> ReadCsvFile(
    CSVUpload database,
    string name,
    string path
)
        {
            int count = System.IO.File.ReadAllLines(path).Length;
            Encoding encoding = Encoding.UTF8;
            List<string> csvHeader = new List<string>();
            List<List<double>> transaction = new List<List<double>>();

            using (var reader = new StreamReader(path, encoding))
            {
                string line = await reader.ReadLineAsync();
                var formatLine = line.Split(database.delimiter.Text.ToCharArray()[0]);
                int d = 0;
                csvHeader.AddRange(formatLine);

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    d++;

                    line = line.Replace(database.arrayDelimiter.Text[0].ToString(), "").Replace(database.arrayDelimiter.Text[1].ToString(), "");
                    formatLine = line.Split(database.delimiter.Text.ToCharArray()[0]);

                    List<double> rowData = new List<double>();

                    if (d == count / 10)
                    {
                        MessageBox.Show("10%");
                    }
                    else if (d == count / 4)
                    {
                        MessageBox.Show("25%");
                    }
                    else if (d == count / 2)
                    {
                        MessageBox.Show("50%");
                    }
                    else if (d == count * 3 / 4)
                    {
                        MessageBox.Show("75%");
                    }

                    for (int i = 0; i < formatLine.Length; i++)
                    {

                        


                        if (double.TryParse(formatLine[i], out double value))
                        {
                            rowData.Add(value);
                        }
                        else
                        {
                            MessageBox.Show("Значення не число");
                        }
                    }

                    transaction.Add(rowData);
                }
            }

            return (csvHeader, transaction.ToArray());
        }*/


        /*CreateCsvFile зчитує DataBase наступного формату
 idOrder,IdProduct,quantity
  1,1,3
  2,2,1
  та аналогічно переносить у CSV файл */
        public static async Task CreateCsvFile(
            List<string> header,
            List<double>[] transaction,   // масив списків цілих чисел, що містить результати запиту
            DatabaseInfo database,// об'єкт, що містить інформацію з textbox форми
            string name           // назва файлу CSV, у який зберігатимуться результати
        )
        {
            string path = $@"..\..\Export\{name}.csv"; // шлях до файлу CSV, що зберігатиметься
            string transactionsql = $"";

            try
            {
                // створюємо об'єкт записувача, що дозволяє записувати у файл
                Encoding encoding = Encoding.UTF8;

                using (StreamWriter writer = new StreamWriter(path, false, encoding))
                {
                    // записуємо назви колонок у файл
                    for (int y = 0; y < header.Count; y++)
                    {
                        if (y < header.Count - 1)
                        {
                            transactionsql += $"{header[y]}{database.delimiter.Text.ToCharArray()[0]}";
                        }
                        else
                        {
                            transactionsql += $"{header[y]}";
                        }
                    }
                    await writer.WriteLineAsync(transactionsql);
                    transactionsql = "";

                    for (int i = 0; i < transaction.Count(); i++)
                    {
                        for (int j = 0; j < transaction[0].Count(); j++)
                        {

                            if (j < transaction[0].Count() - 1)
                            {

                                transactionsql += $"{transaction[i][j]}{database.delimiter.Text.ToCharArray()[0]}";
                            }
                            else
                            {
                                transactionsql += $"{transaction[i][j]}";
                            }
                        }
                        // записуємо значення кожної колонки у файл
                        await writer.WriteLineAsync(transactionsql);
                        transactionsql = "";
                    }
                }
            }
            catch (IOException ex)// якщо сталася помилка вводу-виводу, то виводимо повідомлення про помилку
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        public static async Task CreateCsvFile(
            List<string> header,
            List<double>[] transaction,   // масив списків цілих чисел, що містить результати запиту
            CSVUpload database,// об'єкт, що містить інформацію з textbox форми
            string name           // назва файлу CSV, у який зберігатимуться результати
        )
        {
            string path = $@"..\..\Export\{name}.csv"; // шлях до файлу CSV, що зберігатиметься
            string transactionsql = $"";

            try
            {
                // створюємо об'єкт записувача, що дозволяє записувати у файл
                Encoding encoding = Encoding.UTF8;

                using (StreamWriter writer = new StreamWriter(path, false, encoding))
                {
                    // записуємо назви колонок у файл
                    for (int y = 0; y < header.Count; y++)
                    {
                        if (y < header.Count - 1)
                        {
                            transactionsql += $"{header[y]},";
                        }
                        else
                        {
                            transactionsql += $"{header[y]}";
                        }
                    }
                    await writer.WriteLineAsync(transactionsql);
                    transactionsql = "";

                    for (int i = 0; i < transaction.Count() - 1; i++)
                    {
                        for (int j = 1; j < transaction[0].Count(); j++)
                        {

                            if (j < transaction[0].Count() - 1)
                            {

                                transactionsql += $"{transaction[i][j]},";
                            }
                            else
                            {
                                transactionsql += $"{transaction[i][j]}";
                            }
                        }
                        // записуємо значення кожної колонки у файл
                        await writer.WriteLineAsync(transactionsql);
                        transactionsql = "";
                    }
                }
            }
            catch (IOException ex)// якщо сталася помилка вводу-виводу, то виводимо повідомлення про помилку
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        /*CreateFormatCsvFile зчитує 
         DataBase наступного формату
         idOrder,IdProduct,quantity
          1,1,3
          2,2,1
          та переносить у CSV файл з наступним форматуванням
          fkOrder,fkProducts
          1,[1,2,3]
          2,[1,2]
         */
        public static async Task CreateFormatCsvFile(
            List<double>[] result,
            DatabaseInfo database,
            string name
        )
        {
            List<string> header = new List<string>() { $"{database.nameColumn1.Text}", $"{database.nameColumn2.Text}" };
            List<double> productId = new List<double>();

            for (int i = 0; i < result.Count() - 1; i++)
            {
                productId.Add(result[i][1]);
            }

            List<double> uniqueValue = productId.Distinct().ToList(); // отримуємо унікальні значення з першого масиву та зберігаємо їх у змінну uniqueValue
            uniqueValue.Sort();


            string path = $@"..\..\Export\{name}.csv"; // шлях до файлу, в який ми зберігаємо дані
            string transaction = $"";// змінна, яка містить рядок, що буде додаватися в кінець кожного рядка в файлі
            Encoding encoding = Encoding.UTF8;
            //QuickSort.Quicksort(result, 0, result.Count()-2);
            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false, encoding))
                {
                    for (int y = 0; y < header.Count; y++)
                    {
                        if (y < header.Count - 1)
                        {
                            transaction += $"{header[y]},";
                        }
                        else
                        {
                            transaction += $"{header[y]}";
                        }
                    }
                    await writer.WriteLineAsync(transaction);
                    transaction = "";

                    // асинхронно записуємо рядок у файл
                    bool isIdTransaction = false;// змінна, яка допомагає визначити, чи було вже додано Header до поточного рядка

                    // проходимося по всіх елементах списку result[0]
                    for (int i = 0; i < result.Count(); i++)
                    {
                        if (i < result.Count() - 1)// якщо це не останній елемент
                        {
                            if (isIdTransaction == false)//Якщо ще не вказано idТранзакції
                            {

                                if (database.arrayDelimiter.SelectedIndex == 0)// якщо будуть використовуватись символи []
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}[";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)// якщо будуть використовуватись символи {}
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}{{";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}";// якщо не будуть використовуватись символи масивів
                                }

                                isIdTransaction = true;//визначаємо, що IdOrder вже заповнений
                            }

                            //Якщо відбуваэться зміна замовлення (оскільки воно відсортовано, тобто далі вже не буде йти поточне замовлення, тож це кінець масиву IdProduct)
                            if (result[i + 1][0] > result[i][0])
                            {


                                if (database.arrayDelimiter.SelectedIndex == 0)
                                {
                                    transaction += $"{result[i][1]}]";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)
                                {
                                    transaction += $"{result[i][1]}}}";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i][1]}";
                                }
                                // асинхронно записуємо рядок у файл
                                await writer.WriteLineAsync(transaction);
                                transaction = $"";
                                isIdTransaction = false;
                            }
                            //В іншому випадку продовжуємо додавати до рядка
                            else
                            {
                                transaction += $"{result[i][1]}{database.delimiter.Text}";
                            }
                        }
                        //якщо це останный елемент масиву result
                        else
                        {
                            //перевіряємо, чи записаний у нас заголовок (на випадок, якщо останнє замовлення включає у себе лише 1 продукт)
                            if (isIdTransaction == false)
                            {
                                if (database.arrayDelimiter.SelectedIndex == 0)
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}[";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}{{";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i][0]}{database.delimiter.Text}";
                                }

                                isIdTransaction = true;
                            }

                            if (database.arrayDelimiter.SelectedIndex == 0)
                            {
                                transaction += $"{result[i][1]}]";
                            }
                            else if (database.arrayDelimiter.SelectedIndex == 1)
                            {
                                transaction += $"{result[i][1]}}}";
                            }
                            else if (database.arrayDelimiter.SelectedIndex == 2)
                            {
                                transaction += $"{result[i][1]}";
                            }

                            // асинхронно записуємо рядок у файл
                            await writer.WriteLineAsync(transaction);
                            transaction = $"";
                            isIdTransaction = false;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }




        public static async Task CreateFormatCsvFile(
          List<double>[] result,
          List<string> header,
          CSVUpload database,
          string name
      )
        {

            List<double> productId = new List<double>();

            for (int i = 0; i < result.Count() - 1; i++)
            {
                productId.Add(result[i][1]);
            }

            List<double> uniqueValue = productId.Distinct().ToList(); // отримуємо унікальні значення з першого масиву та зберігаємо їх у змінну uniqueValue
            uniqueValue.Sort();


            string path = $@"..\..\Export\{name}.csv"; // шлях до файлу, в який ми зберігаємо дані
            string transaction = $"";// змінна, яка містить рядок, що буде додаватися в кінець кожного рядка в файлі
            Encoding encoding = Encoding.UTF8;
            //QuickSort.Quicksort(result, 0, result.Count()-2);
            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false, encoding))
                {
                    for (int y = 0; y < header.Count; y++)
                    {
                        if (y < header.Count - 1)
                        {
                            transaction += $"{header[y]},";
                        }
                        else
                        {
                            transaction += $"{header[y]}";
                        }
                    }
                    await writer.WriteLineAsync(transaction);
                    transaction = "";

                    // асинхронно записуємо рядок у файл
                    bool isIdTransaction = false;// змінна, яка допомагає визначити, чи було вже додано Header до поточного рядка

                    // проходимося по всіх елементах списку result[0]
                    for (int i = 1; i < result.Count(); i++)
                    {
                        if (i < result.Count() - 1)// якщо це не останній елемент
                        {
                            if (isIdTransaction == false)//Якщо ще не вказано idТранзакції
                            {

                                if (database.arrayDelimiter.SelectedIndex == 0)// якщо будуть використовуватись символи []
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}[";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)// якщо будуть використовуватись символи {}
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}{{";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}";// якщо не будуть використовуватись символи масивів
                                }

                                isIdTransaction = true;//визначаємо, що IdOrder вже заповнений
                            }

                            //Якщо відбуваэться зміна замовлення (оскільки воно відсортовано, тобто далі вже не буде йти поточне замовлення, тож це кінець масиву IdProduct)
                            if (result[i - 1][0] < result[i][0])
                            {

                                if (database.arrayDelimiter.SelectedIndex == 0)
                                {
                                    transaction += $"{result[i - 1][1]}]";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)
                                {
                                    transaction += $"{result[i - 1][1]}}}";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i - 1][1]}";
                                }
                                // асинхронно записуємо рядок у файл
                                await writer.WriteLineAsync(transaction);
                                transaction = $"";
                                isIdTransaction = false;
                            }
                            //В іншому випадку продовжуємо додавати до рядка
                            else
                            {
                                transaction += $"{result[i][1]}{database.delimiter.Text}";
                            }
                        }
                        //якщо це останный елемент масиву result
                        else
                        {
                            //перевіряємо, чи записаний у нас заголовок (на випадок, якщо останнє замовлення включає у себе лише 1 продукт)
                            if (isIdTransaction == false)
                            {
                                if (database.arrayDelimiter.SelectedIndex == 0)
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}[";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 1)
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}{{";
                                }
                                else if (database.arrayDelimiter.SelectedIndex == 2)
                                {
                                    transaction += $"{result[i - 1][0]}{database.delimiter.Text}";
                                }

                                isIdTransaction = true;
                            }

                            if (database.arrayDelimiter.SelectedIndex == 0)
                            {
                                transaction += $"{result[i - 1][1]}]";
                            }
                            else if (database.arrayDelimiter.SelectedIndex == 1)
                            {
                                transaction += $"{result[i - 1][1]}}}";
                            }
                            else if (database.arrayDelimiter.SelectedIndex == 2)
                            {
                                transaction += $"{result[i - 1][1]}";
                            }

                            // асинхронно записуємо рядок у файл
                            await writer.WriteLineAsync(transaction);
                            transaction = $"";
                            isIdTransaction = false;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }
        /*CreateFormatCsvFileWithDischarge зчитує DataBase наступного формату
        idOrder,IdProduct
        1,1
        2,2
        3,3
        та переносить у CSV файл з наступним форматуванням
        fkOrder,1,2,3
        1,1,1,1
        2,1,1,0
        3,0,0,1
        */
        public static async Task ConvertToSparse(
            List<double>[] result,
            DatabaseInfo database,
            string name
        )
        {
            List<double> uniqueValue = new List<double>();
            for (int i = 0; i < result.Count(); i++)
            {
                for (int a = 1; a < result[i].Count; a++)
                {
                    uniqueValue.Add(result[i][a]);
                }
            }

            uniqueValue = uniqueValue.Distinct().ToList();//унікальні значення IdProduct
            uniqueValue.Sort();//відсортовані унікальні значення IdProduct

            string path = $@"..\..\Export\" + name + ".csv";
            string transaction = $"";

            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    //Додаємо Header
                    //Переписуємо усі унікальні значення IdProduct у Header
                    transaction += $"{database.nameColumn1.Text}{database.delimiter.Text}";
                    for (int i = 0; i < uniqueValue.Count; i++)
                    {
                        if (i < uniqueValue.Count - 1)
                        {
                            transaction += $"{uniqueValue[i]}{database.delimiter.Text}";
                        }
                        else
                        {
                            transaction += $"{uniqueValue[i]}";
                        }
                    }
                    // асинхронно записуємо рядок у файл
                    await writer.WriteLineAsync(transaction);
                    transaction = "";
                    bool isProduct = false;

                    //перебираємо усі IdOrder
                    for (int i = 1; i <= result.Count(); i++)
                    {

                        transaction += $"{result[i - 1][0]}{database.delimiter.Text}";
                        for (int y2 = 0; y2 < uniqueValue.Count; y2++)
                        {

                            //Якщо це не останнє значення IdOrder
                            if (y2 < uniqueValue.Count - 1)
                            {
                                //Якщо значення Header ще не було записано
                                //Якщо є різниця між минулим та наступним IdOrder,значить закінчуємо запис минулого ордера та переносимо у CSV

                                for (int y = 1; y < result[i - 1].Count(); y++)
                                {
                                    //MessageBox.Show($"UN {uniqueValue[y2]} == RE {result[i - 1][y]}");
                                    if (uniqueValue[y2] == result[i - 1][y])
                                    {
                                        isProduct = true;
                                        break;
                                    }
                                }

                                if (isProduct)
                                {
                                    transaction += $"1{database.delimiter.Text}";
                                    isProduct = false;
                                }
                                else
                                {
                                    transaction += $"0{database.delimiter.Text}";
                                }
                                // асинхронно записуємо рядок у файл
                            }
                            else
                            {
                                //Перевіряємо, чи заповнений початок рядка

                                //перебираємо усі унікальні значення
                                for (int y = 1; y < result[i - 1].Count(); y++)
                                {
                                    //MessageBox.Show($"UN^ {uniqueValue[y2]} == RE^ {result[i - 1][y]}");
                                    //якщо цього значення у IdOrder немає, додаємо 0 та збільшуємо значення пробігу
                                    if (uniqueValue[y2] == result[i - 1][y])
                                    {
                                        isProduct = true;
                                        break;
                                    }
                                    //якщо це значення у IdOrder є, додаємо 1 та збільшуємо значення пробігу
                                }

                                if (isProduct)
                                {
                                    transaction += $"1";
                                    isProduct = false;
                                }
                                else
                                {
                                    transaction += $"0";
                                }
                            }
                            // асинхронно записуємо рядок у файл
                        }
                        await writer.WriteLineAsync(transaction);
                        transaction = "";
                        //Якщо різниці між поточним та минулим немає, заповнюємо рядок
                    }
                    //Якщо це останнє значення IdOrder
                }
                MessageBox.Show("Успіх");
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }


        public static async Task ConvertToSparse(
           List<double>[] result,
           CSVUpload database,
           string name
        )
        {

            List<double> uniqueValue = new List<double>();
            for (int i = 0; i < result.Count() - 1; i++)
            {
                for (int a = 1; a < result[i].Count; a++)
                {
                    uniqueValue.Add(result[i][a]);
                }
            }
            uniqueValue = uniqueValue.Distinct().ToList();//унікальні значення IdProduct
            uniqueValue.Sort();//відсортовані унікальні значення IdProduct

            string path = $@"..\..\Export\" + name + ".csv";
            string transaction = $"";

            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    //Додаємо Header
                    //Переписуємо усі унікальні значення IdProduct у Header
                    transaction += $"idTransaction{database.delimiter.Text}";
                    for (int i = 0; i < uniqueValue.Count; i++)
                    {
                        if (i < uniqueValue.Count - 1)
                        {
                            transaction += $"{uniqueValue[i]}{database.delimiter.Text}";
                        }
                        else
                        {
                            transaction += $"{uniqueValue[i]}";
                        }
                    }

                    // асинхронно записуємо рядок у файл
                    await writer.WriteLineAsync(transaction);
                    transaction = "";
                    bool isProduct = false;

                    //перебираємо усі IdOrder
                    for (int i = 1; i < result.Count(); i++)
                    {
                        /*
                        if (i == result.Count() / 10)
                        {
                            MessageBox.Show("10%");
                        }
                        else if (i == result.Count() / 4)
                        {
                            MessageBox.Show("25%");
                        }
                        else if (i == result.Count() / 2)
                        {
                            MessageBox.Show("50%");
                        }
                        else if (i == result.Count() * 3 / 4)
                        {
                            MessageBox.Show("75%");
                        }*/

                        transaction += $"{result[i - 1][0]}{database.delimiter.Text}";
                        for (int y2 = 0; y2 < uniqueValue.Count; y2++)
                        {
                            //Якщо це не останнє значення IdOrder
                            if (y2 < uniqueValue.Count - 1)
                            {
                                //Якщо значення Header ще не було записано
                                //Якщо є різниця між минулим та наступним IdOrder,значить закінчуємо запис минулого ордера та переносимо у CSV

                                for (int y = 1; y < result[i - 1].Count(); y++)
                                {
                                    //MessageBox.Show($"UN {uniqueValue[y2]} == RE {result[i - 1][y]}");
                                    if (uniqueValue[y2] == result[i - 1][y])
                                    {
                                        isProduct = true;
                                        break;
                                    }
                                }

                                if (isProduct)
                                {
                                    transaction += $"1{database.delimiter.Text}";
                                    isProduct = false;
                                }
                                else
                                {
                                    transaction += $"0{database.delimiter.Text}";
                                }
                                // асинхронно записуємо рядок у файл
                            }
                            else
                            {
                                //Перевіряємо, чи заповнений початок рядка

                                //перебираємо усі унікальні значення
                                for (int y = 1; y < result[i - 1].Count(); y++)
                                {
                                    //MessageBox.Show($"UN^ {uniqueValue[y2]} == RE^ {result[i - 1][y]}");
                                    //якщо цього значення у IdOrder немає, додаємо 0 та збільшуємо значення пробігу
                                    if (uniqueValue[y2] == result[i - 1][y])
                                    {
                                        isProduct = true;
                                        break;
                                    }
                                    //якщо це значення у IdOrder є, додаємо 1 та збільшуємо значення пробігу
                                }

                                if (isProduct)
                                {
                                    transaction += $"1";
                                    isProduct = false;
                                }
                                else
                                {
                                    transaction += $"0";
                                }
                            }
                            // асинхронно записуємо рядок у файл
                        }
                        await writer.WriteLineAsync(transaction);
                        transaction = "";
                        //Якщо різниці між поточним та минулим немає, заповнюємо рядок
                    }
                    //Якщо це останнє значення IdOrder
                }
                MessageBox.Show("Успіх");
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }


        public static async Task<List<double>[]> ConvertToRegular(
            List<double>[] csvData,   // масив списків цілих чисел, що містить результати запиту
            DatabaseInfo database,
            string name// об'єкт, що містить інформацію з textbox форми
        )
        {


            string path = $@"..\..\Export\" + name + ".csv";
            string transaction = $"";
            int a = csvData.Count();
            List<double>[] regularCsvData = new List<double>[a];
            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    //Додаємо Header
                    //Переписуємо усі унікальні значення IdProduct у Header
                    transaction += $"{database.nameColumn1.Text}{database.delimiter.Text}idItem";
                    // асинхронно записуємо рядок у файл
                    await writer.WriteLineAsync(transaction);
                    transaction = "";
                    int match = 0;
                    
                    
                    //перебираємо усі IdOrder
                    for (int i = 0; i < csvData.Count(); i++)
                    {
                        int supermatch = 0;
                        transaction += $"{csvData[i][0]}{database.delimiter.Text}[";
                        for (int y2 = 1; y2 < csvData[i].Count(); y2++)
                        {

                            //Якщо це не останнє значення IdOrder
                            if (csvData[i][y2] > 0)
                            {
                                match++;
                            }
                        }
                        regularCsvData[i] = new List<double>(match);
                        for (int y2 = 0; y2 < csvData[i].Count(); y2++)
                        {
                            
                            if (csvData[i][y2] != 0 && supermatch != match)
                            {
                                regularCsvData[i].Add(csvData[i][y2]);
                                transaction += $"{csvData[i][y2]}{database.delimiter.Text}";
                                supermatch++;
                            }
                            else if (csvData[i][y2] != 0)
                            {
                                regularCsvData[i].Add(csvData[i][y2]);
                                transaction += $"{csvData[i][y2]}";
                                supermatch++;
                            }
                            match = 0;

                        }
                        transaction += "]";
                        await writer.WriteLineAsync(transaction);
                        transaction = "";
                    }

                    //Якщо різниці між поточним та минулим немає, заповнюємо рядок
                }
                //Якщо це останнє значення IdOrder
                

                MessageBox.Show("Успіх");
                return regularCsvData;
            }

            catch (IOException ex)// якщо сталася помилка вводу-виводу, то виводимо повідомлення про помилку
            {
                MessageBox.Show(ex.Message, "Помилка");
                return null;
            }
        }

        public static async Task ConvertToRegular(
           List<string> csvHeader,
           List<double>[] csvData,   // масив списків цілих чисел, що містить результати запиту
           CSVUpload database,
           string name// об'єкт, що містить інформацію з textbox форми
       )
        {
            List<double> uniqueValue = new List<double>();

            for (int i = 1; i < csvHeader.Count(); i++)
            {

                uniqueValue.Add(double.Parse(csvHeader[i]));
            }

            string path = $@"..\..\Export\" + name + ".csv";
            string transaction = $"";

            try
            {
                // відкриваємо файл для записування і вказуємо, що потрібно замінити його повністю
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    //Додаємо Header
                    //Переписуємо усі унікальні значення IdProduct у Header
                    transaction += $"idTransaction{database.delimiter.Text}";
                    for (int i = 0; i < uniqueValue.Count; i++)
                    {
                        if (i < uniqueValue.Count - 1)
                        {
                            transaction += $"{uniqueValue[i]}{database.delimiter.Text}";
                        }
                        else
                        {
                            transaction += $"{uniqueValue[i]}";
                        }
                    }

                    // асинхронно записуємо рядок у файл
                    await writer.WriteLineAsync(transaction);
                    transaction = "";
                    int match = 0;
                    //перебираємо усі IdOrder
                    for (int i = 0; i < csvData.Count() - 1; i++)
                    {

                        transaction += $"{csvData[i][0]}{database.delimiter.Text}[";
                        for (int y2 = 1; y2 < csvData[i].Count(); y2++)
                        {
                            //Якщо це не останнє значення IdOrder
                            if (csvData[i][y2] > 0)
                            {
                                match++;
                            }
                        }

                        for (int y2 = 1; y2 < csvData[i].Count(); y2++)
                        {
                            //Якщо це не останнє значення IdOrder
                            if (csvData[i][y2] > 0 && match > 1)
                            {
                                transaction += $"{uniqueValue[y2 - 1]}{database.delimiter.Text}";
                                match--;
                            }
                            else if (csvData[i][y2] > 0)
                            {
                                transaction += $"{uniqueValue[y2 - 1]}";
                                match--;
                            }


                        }
                        transaction += "]";
                        await writer.WriteLineAsync(transaction);
                        transaction = "";
                    }

                    //Якщо різниці між поточним та минулим немає, заповнюємо рядок
                }
                //Якщо це останнє значення IdOrder

                MessageBox.Show("Успіх");
            }

            catch (IOException ex)// якщо сталася помилка вводу-виводу, то виводимо повідомлення про помилку
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        public static List<double>[] CleaningTransaction(List<double>[] result)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                int size=0;
                for (int i = 1; i < result.Count()/*-1*/; i++)
                {
                    if (Convert.ToInt32(result[i - 1][0]) < Convert.ToInt32(result[i][0]))
                    {
                        size++;
                    }
                }
                size++;

                List<double>[] csvData = new List<double>[size];

                for (int i = 0; i < csvData.Count(); i++)
                {
                    csvData[i] = new List<double>();
                }

                int a=0;
                bool isIdTransaction = false;// змінна, яка допомагає визначити, чи було вже додано Header до поточного рядка
                // проходимося по всіх елементах списку result[0]
                for (int i = 1; i < result.Count()/*-1*/; i++)
                {
                    
                    if (isIdTransaction == false)
                    { 
                        csvData[a].Add(result[i - 1][0]);
                        isIdTransaction = true;
                    }

                    if (Convert.ToInt32(result[i - 1][0]) == Convert.ToInt32(result[i][0]))
                    {
                        csvData[a].Add(result[i - 1][1]);
                    }
                    else
                    {
                        csvData[a].Add(result[i - 1][1]);
                        a++;
                        isIdTransaction= false;
                    }

                    if (i == result.Count()-1/*-2*/)
                    {
                        csvData[a].Add(result[i][1]);
                    }
                }


                return csvData;

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
                return null;
            }


        }

        public static async Task CreateRuleFile((List<List<double>>, List<double>) input)
        {
            string path = $@"..\..\Export\RuleFP-Growth.csv"; // шлях до файлу CSV, що зберігатиметься
            string transactionsql = $"";
            try
            {
                // створюємо об'єкт записувача, що дозволяє записувати у файл
                Encoding encoding = Encoding.UTF8;

                using (StreamWriter writer = new StreamWriter(path, false, encoding))
                {
                    // записуємо назви колонок у файл
                    for (int y = 0; y < input.Item1.Count(); y++)
                    {

                        transactionsql = $"Rule: {string.Join(",", input.Item1[y])} with support: {input.Item2[y]}";
                        await writer.WriteLineAsync(transactionsql);
                    }
                }
            }
            catch (IOException ex)// якщо сталася помилка вводу-виводу, то виводимо повідомлення про помилку
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }

        public static async Task CreateRuleFile2(Tuple<List<List<double>>, List<List<double>>, List<double>, List<double>> input)
        {
            try
            {
                string path = @"..\..\Export\RuleApriori.csv";
                string transaction = "";
                Encoding encoding = Encoding.UTF8;

                using(StreamWriter writer =  new StreamWriter(path, false, encoding))
                {
                    for(int y=0; y < input.Item1.Count(); y++)
                    {
                        transaction = $"Antecedent: {string.Join((","), input.Item1[y])} Consequent: {string.Join((","), input.Item2[y])} with support" +
                            $"{input.Item3[y]} and confidence {input.Item4[y]}";
                        await writer .WriteLineAsync(transaction);
                    }
                }
            }catch(IOException ex)
            {
                MessageBox.Show(ex.Message, "Помилка");
            }
        }
    }
}
