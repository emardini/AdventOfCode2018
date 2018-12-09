using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day_5_A
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(React(new StringBuilder(DataProvider.Input_5())).Length);
            Console.ReadKey();

            var react = upper.Select(x => React(new StringBuilder(DataProvider.Input_5()).Replace(x, string.Empty).Replace(x.ToLower(), string.Empty)))
                .AsParallel()
                .ToList()
                .OrderBy(x => x.Length)
                .FirstOrDefault();

            Console.WriteLine(react.Length);
            Console.ReadKey();
        }

        private static List<string> upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(x => new string(new[] { x })).ToList();
        private static List<string> lower = upper.Select(x => x.ToLower()).ToList();
        private static List<string> combinations = upper.Zip(lower, (a, b) => $"{a}{b}").
                Union(upper.Zip(lower, (a, b) => $"{b}{a}"))
                .ToList();
        public static StringBuilder React(StringBuilder builder)
        {
            var prevLen = builder.Length;
            var newLen = 0;
            while (newLen < prevLen)
            {
                prevLen = builder.Length;
                foreach (var combination in combinations)
                {
                    builder.Replace(combination, string.Empty);
                }
                newLen = builder.Length;
            }

            return builder;
        }
    }
}
