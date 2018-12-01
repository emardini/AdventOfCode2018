using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_1_B
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = DataProvider.Input();
            var inputAsList = input.ParseInputAsList().ToList();

            var repeatedFreq = (int?)null;
            var frequencies = inputAsList
                                .Cycle()
                                .Scan((a, b) => a + b, 0);
            var frequencyTable = new Dictionary<int, bool>();
            foreach (var freq in frequencies)
            {
                if (frequencyTable.ContainsKey(freq))
                {
                    repeatedFreq = freq;
                    break;
                }
                frequencyTable.Add(freq, true);
            }

            Console.WriteLine(repeatedFreq);

            Console.ReadKey();
        }
    }
}
