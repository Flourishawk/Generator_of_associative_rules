using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace WpfApp1
{
    internal class QuickSort
    {
        // Розділяє масив на дві частини таким чином, щоб у лівій частині елементи були менші за елементи правої частини
        private static int Partition(List<double>[] array, int start, int end)
        {
            // Встановлюємо індекс маркера на початкове значення
            int marker = start;
            // Проходимо по всіх елементах масиву
            for (int i = start; i < end; i++)
            {
                // Якщо поточний елемент менший за останній елемент масиву

                    if (array[i][0] < array[end][0])
                    {
                        for (int j = 0; j <= array[i].Count() - 1; j++)
                        {
                            // Міняємо місцями елементи в масивах, використовуючи деконструкцію кортежів
                            (array[marker][j], array[i][j]) = (array[i][j], array[marker][j]);
                        }
                        // Міняємо місцями поточний елемент і елемент маркера

                        marker += 1;
                    }
                
            }
            // Міняємо місцями елемент маркера та останній елемент масиву
           
                // Міняємо місцями елементи в масивах, використовуючи деконструкцію кортежів
                (array[marker][0], array[end][0]) = (array[end][0], array[marker][0]);
                (array[marker][1], array[end][1]) = (array[end][1], array[marker][1]);

            // Повертаємо індекс маркера
            return marker;
        }
        /*
        // Сортує масив в порядку зростання за допомогою алгоритму швидкого сортування
        public static void Quicksort(List<double>[] array, int start, int end)
        {
            // Якщо індекс початкового елемента більший або рівний індексу кінцевого елемента, то вихід з функції
            if (start >= end)
                return;
            // Розділяємо масив на дві частини і повертаємо індекс маркера
            int pivot = Partition(array, start, end);
            Quicksort(array, start, pivot - 1);
            Quicksort(array, pivot + 1, end);
        }*/

        public static void Quicksort(List<double>[] array, int start, int end)
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(start);
            stack.Push(end);

            while (stack.Count > 0)
            {
                end = stack.Pop();
                start = stack.Pop();

                int pivot = Partition(array, start, end);

                if (pivot - 1 > start)
                {
                    stack.Push(start);
                    stack.Push(pivot - 1);
                }

                if (pivot + 1 < end)
                {
                    stack.Push(pivot + 1);
                    stack.Push(end);
                }
            }
        }

        //використовувати тільки для нерозряджених матриць
        public static void QuicksortContinue(List<double>[] array)
        {
            double a = 0;
            // Цикл для перевірки елементів з однаковим значенням
            for (int y = 0; y < array.Count()-1; y++)
            {
                for (int x = 0; x < array.Count() - 2; x++)
                {
                    // Цикл для проходження по списку та порівняння значень сусідніх елементів
                    if (array[x][0] == array[x + 1][0])//Якщо у 2 строках однаковий айді замовлення
                    {
                        if (array[x][1] > array[x + 1][1])// Якщо другий елемент менший за попередній
                        {
                            // Міняємо місцями елементи в масивах, використовуючи деконструкцію кортежів
                            a = array[x][1];
                            (array[x][1], array[x + 1][1]) = (array[x + 1][1], a);
                        }
                    }
                }
            }
            
        }
        
    }
}
