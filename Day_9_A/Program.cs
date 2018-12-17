using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_9_A
{
    class Program
    {
        static void Main(string[] args)
        {
            new MarbleGame().InsertMarble(6);
        }

        public class MarbleGame
        {
            int currentIndex = 0;
            readonly List<int>  data = new List<int>() { 0 };
            public int WinningScore(int nbPlayers, int nbMarble)
            {
                return 1;
            }

            public void InsertMarble(int value)
            {
                foreach (var index in Enumerable.Range(0, 10))
                {
                    currentIndex = GetNewCurrentIndex().GetValueOrDefault();
                    data.Insert(currentIndex, index);
                    Console.WriteLine(currentIndex);
                }
                Console.ReadKey();
            }

            public int? GetNewCurrentIndex()
            {
                if (data.Count == 0) return null;

                if (data.Count == 1) return 1;

                if (data.Count == 2) return 1;

                if (((currentIndex + 1) / data.Count) > 0 && ((currentIndex + 1) % data.Count) == 0)
                    return data.Count;

                if (((currentIndex + 1) / data.Count) > 0 && ((currentIndex + 1) % data.Count) > 0)
                    return 1;

                return ((currentIndex + 2) % data.Count);
            }
        }
    }
}
