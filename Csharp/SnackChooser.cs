using System;
using System.Collections.Generic;

namespace SnackOfTheDay
{
    static class SnackChooser
    {
        static readonly string[] snacks = { "Coffee", "Juice", "Decaf", "Soy Milk", "Milk" };
        static readonly int[] healthWeights = { 1, 6, 2, 3, 2 };

        static int N;
        static int[] cummulativeWeights;
        static int totalWeights;

        //Constructor
        static SnackChooser ()
        {
            N = healthWeights.Length;
            cummulativeWeights = new int[N+1];
            cummulativeWeights[0] = 0;

            for (int i = 1; i <= N; i++)
            {
                cummulativeWeights[i] = cummulativeWeights[i-1] + healthWeights[i-1];
            }

            totalWeights = cummulativeWeights[N-1];
        }

        public static string Choose()
        {

            Random rand = new Random();
            //Generate random number ranging from 0 to totalWeights
            double randomNumber = rand.NextDouble() * (totalWeights + 1);

            for (int i = 0; i < N; i++)
            {
                if (randomNumber >= cummulativeWeights[i] && randomNumber < cummulativeWeights[i + 1])
                {
                    return snacks[i];
                }
            }

            return string.Empty;
        }

        static void Test()
        {
            int loops = 1000000;
            Dictionary<string, int> snackCounts = new Dictionary<string, int>();
            foreach (string snack in snacks)
            {
                snackCounts.Add(snack, 0);
            }

            for (int i = 0; i < loops; i++)
            {
                snackCounts[Choose()]++;
            }

            foreach (string snack in snacks)
            {
                Console.WriteLine("{0} selected {1}%", snack, (double)snackCounts[snack] * 100 / loops);
            }
        }

        static void Main(string[] args)
        {
            //SnackChooser.Test();
            Console.WriteLine("Snack for today: {0}", SnackChooser.Choose());

            Console.ReadLine();
        }
    }
}
