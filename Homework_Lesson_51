using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Lesson_51
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Console.Write("Please put in array size: ");
            int arraySize = int.Parse(Console.ReadLine());
            Console.Write("The random numbers will be from 1 to: ");
            int maxNum = int.Parse(Console.ReadLine());
            int[] bigArray = new int[arraySize];
            
            Console.WriteLine("\nThese are your numbers:");

            for (int i = 0; i < bigArray.Length; i++)
            {
                bigArray[i] = rnd.Next(0,500);
                Console.Write(bigArray[i] + ", ");
                if (i%10==0 && i!=0)
                {
                    Console.WriteLine();
                }
            }
            Console.Write("\n\nPlease advise what SUM we are looking: ");
            int sum = int.Parse(Console.ReadLine());
                        
            Dictionary<int, int> search = new Dictionary<int, int>();
            if (sum > maxNum * 2)
            {
                Console.WriteLine("The SUM you chose is biger than the max number * 2 so bye bye");
            }
            else
            {
                for (int i = 0; i < bigArray.Length - 1; i++)
                {
                    try
                    {
                        if (search.ContainsKey(sum - bigArray[i]))
                        {
                            Console.WriteLine("We found the SUM of {0} by {1} at [{2}] and {3} at [{4}]", sum, (sum - bigArray[i]), search[sum - bigArray[i]],bigArray[i],i);
                            
                        }
                        else
                        {
                            search.Add(bigArray[i], i);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error at [{2}] - the number {0} was already entered at [{1}]", bigArray[i],search[bigArray[i]],i);
                    }
                }
            }
            Console.WriteLine("Finish");
           
        }

        
    }
}
