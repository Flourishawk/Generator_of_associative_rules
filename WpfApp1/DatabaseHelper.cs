using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    internal class DatabaseHelper
    {
        public static List<double>[] executeReader(string query, string connection)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connection))
                {
                    mySqlConnection.Open();

                    int row = 0;
                    using (MySqlCommand command1 = new MySqlCommand(query, mySqlConnection))
                    {
                        using (MySqlDataReader mySqlReader = command1.ExecuteReader())
                        {
                            while (mySqlReader.Read())
                            {
                                row++;
                            }
                        }
                    }

                    List<double>[] result = new List<double>[row];
                    int a = 0;

                    using (MySqlCommand command2 = new MySqlCommand(query, mySqlConnection))
                    {
                        using (MySqlDataReader mySqlReader2 = command2.ExecuteReader())
                        {
                            while (mySqlReader2.Read())
                            {
                                result[a] = new List<double>();
                                for (int i = 0; i < mySqlReader2.FieldCount; i++)
                                {
                                    result[a].Add(mySqlReader2.GetDouble(i));
                                }
                                a++;
                            }
                        }
                    }

                    return result;
                }
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message, "Помилка");
                return null;
            }
        }

        public static List<string> executeHeader(string query, string connection)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connection))
                {
                    mySqlConnection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                    {
                        MySqlDataReader mySqlReader = command.ExecuteReader();
                        List<string> result = new List<string>();

                        while (mySqlReader.Read())
                        {
                            int i = 0;
                            result.Add(mySqlReader.GetString(i));
                            i++;
                        }
                        mySqlReader.Close();
                        return result;
                    }
                }
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message, "Помилка");
                return null;
            }
        }
    }
}
