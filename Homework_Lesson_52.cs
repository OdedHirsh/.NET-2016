using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_Lesson_52
{
    class Program
    {


        static void Main(string[] args)
        {
            
            #region|תרגיל 4|
            
            Random rnd = new Random();
            Console.Write("How many choclate boxes are there? ");
            int arraySize = int.Parse(Console.ReadLine());
            Console.Write("how many choclates can be in a box max? ");
            int maxNum = int.Parse(Console.ReadLine());
            int[] bigArray = new int[arraySize];
            for (int i = 0; i < bigArray.Length; i++)
            {
                bigArray[i] = rnd.Next(1, maxNum);
            }
            QuickSort(bigArray, 0, bigArray.Length - 1);
            Console.Write("\nHow many students are there? ");
            int studentds = int.Parse(Console.ReadLine());
            Console.WriteLine("These are the quantities:");
            int count = 0;
            foreach (var item in bigArray)
            {
                Console.Write(item + ", ");
                count++;
                if (count % 10 == 0)
                {
                    Console.WriteLine();
                }

            }


            int place = 0;
            int diff = bigArray[bigArray.Length - 1] -bigArray[0];

            for (int i = 0; i < bigArray.Length-studentds; i++)
            {
                if ((bigArray[i+studentds-1]-bigArray[i])<diff)
                {
                    place = i;
                    diff = bigArray[i + studentds-1] - bigArray[i];
                }
            }
            
            Console.WriteLine("you should give students the boxes from {0} to {1}", place, place+studentds-1);

            #endregion

            #region|תרגיל 5|
            Console.Write("Choose Matrix Array Size: ");
            int n = int.Parse(Console.ReadLine());
            int[,] matrixArray = new int[n, n];
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrixArray[i,j] = (i+1)*(j+2);    
                }
            }
            matrixArray[2,6]++;
            Console.WriteLine("Your matrix array is:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrixArray[i,j] + ", ");
                }
                Console.WriteLine();
            }

            Console.Write("Choose number to search:");
            int looking = int.Parse(Console.ReadLine());
            
            lookupNumber(matrixArray,looking);
            #endregion
        }

        static void lookupNumber(int[,] array, int looking)
        {
            int size = array.GetLength(0);
            if (array[0, 0] > looking || array[size - 1, size - 1] < looking)
            {
                Console.WriteLine("number out of range");
                return;
            }

            for (int i = 0; i < size; )
            {
                for (int j = size - 1; j > 0; )
                {
                    if (looking == array[i, j])
                    {
                        Console.WriteLine("FOUND at [{0}][{1}]",i,j);
                        return;
                    }
                    else if (looking > array[i, j])
                    {
                        i++;
                        if (i>9)
                        {
                            Console.WriteLine("number not found");
                            return;
                        }
                    }
                    else
                    {
                        j--;
                        if (j < 0)
                        {
                            Console.WriteLine("number not found");
                            return;
                        }
                    }
                }
            }

        }
        

        /*
        static void BADlookupNumber(int[,] array, int looking)
        {

            int size = array.GetLength(0);
            if (array[0, 0] > looking || array[size - 1, size - 1] < looking)
            {
                Console.WriteLine("number out of range");
                return;
            }

            for (int i = 0; i < size;)
            {
                for (int j = 0; j < size;)
                {
                    if (looking > array[i, j])
                    {
                        i++;
                        j++;
                        continue;
                    }
                    else if (array[i, j] == looking)
                    {
                        Console.WriteLine("FOUND");
                        return;
                    }
                    else
                    {
                        for (; i >= 0;)
                        {
                            for (; j >= 0; j--)
                            {
                                if (array[i, j] == looking)
                                {
                                    Console.WriteLine("FOUND");
                                    return;
                                }
                            }
                            j = size - 1;
                            i--;
                        }
                    }
                }
            }
        }

    */
        #region|QuickSort|
        private static void Exchange(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        public static void QuickSort(int[] array, int init, int end)
        {
            if (init < end)
            {
                int pivot = Partition(array, init, end);
                QuickSort(array, init, pivot - 1);
                QuickSort(array, pivot + 1, end);
            }
        }

        private static int Partition(int[] array, int init, int end)
        {
            int last = array[end];
            int i = init - 1;
            for (int j = init; j < end; j++)
            {
                if (array[j] <= last)
                {
                    i++;
                    Exchange(array, i, j);
                }
            }
            Exchange(array, i + 1, end);
            return i + 1;
        }
        #endregion
    }
}
