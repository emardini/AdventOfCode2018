using Core;
using System;
using System.Linq;

namespace Day_2_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = DataProvider.Input_2().
                Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            var inputArray = inputLines
                .Select(x => x.ToCharArray()).ToList();

            var grouped = inputArray.Select(x => x.GroupBy(y => y).Select(z => new { z.Key, nb = z.Count() }).ToList()).ToList();

            var count2 = grouped.Where(x => x.Any(z => z.nb == 2)).Count();
            var count3 = grouped.Where(x => x.Any(z => z.nb == 3)).Count();

            Console.WriteLine(count2 * count3);

            Console.ReadKey();

            System.Collections.Generic.IEnumerable<char[]> candidates = (from i1 in inputLines
                              from i2 in inputLines
                              select new { i1, i2, d = LevenshteinDistance(i1, i2) }).AsParallel()
                              .Where(x=> x.d == 1)
                              .Select(x => x.i1)
                              .Take(2)
                              .ToList()
                              .Select(x=> x.ToCharArray())
                              .ToList();
            var chars1 = candidates.Take(1).SelectMany(x => x).ToList();
            var chars2 = candidates.Skip(1).Take(1).SelectMany(x => x).ToList();
            var result = chars1.Zip(chars2, (a, b) => a == b ? a : (char?)null).Where(x => x.HasValue).ToList();
            var commonstring = string.Concat(result);
            Console.WriteLine(commonstring);
            Console.ReadKey();
        }


        // len_s and len_t are the number of characters in string s and t respectively
        public static int LevenshteinDistance(string s, string t)
        {
            var len_s = s.Length;
            var len_t = t.Length;
            if (len_s != len_t) throw new ArgumentException("String lenght does not match");


            /* base case: empty strings */
            if (len_s == 0) return 0;

            int cost;
            /* test if last characters of the strings match */
            if (s[len_s - 1] == t[len_s - 1])
                cost = 0;
            else
                cost = 1;

            /* return minimum of delete char from s, delete char from t, and delete char from both */
            return LevenshteinDistance(s.Substring(0, len_s - 1), t.Substring(0, len_s - 1)) + cost;
        }
    }
}
