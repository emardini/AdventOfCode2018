using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Extensions
    {
        public static IEnumerable<int> Cycle(this IEnumerable<int> list, int? cycles = null)
        {
            if (cycles.GetValueOrDefault(0) < 0) throw new ArgumentException(nameof(cycles), $"{cycles} should be a positive number");

            var iteration = 0;
            while (cycles == null || iteration < cycles)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
                iteration++;
            }
        }

        public static int ManhattanDistance (int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }


        public static int ParseInt(this string value)
        {
            return int.Parse(value);
        }

        public static IEnumerable<int> ParseInputAsList(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<int>();

            return
               input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
               .Where(x => !string.IsNullOrWhiteSpace(x))
               .Select(x => int.Parse(x));
        }

        public static IEnumerable<int> Scan(this IEnumerable<int> input, Func<int, int, int> next, int seed)
        {
            var state = seed;
            yield return state;
            foreach (var item in input)
            {
                state = next(state, item);
                yield return state;
            }
        }
    }
}
