using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_1_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = DataProvider.Input_1();
            var inputAsList = input.ParseInputAsList().ToList();


            Console.WriteLine(inputAsList.Sum());

            Console.ReadKey();
        }
    }
}
