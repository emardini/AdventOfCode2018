using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_4_A
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input1 = DataProvider.Input_4().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x);

            var input = input1.AsDataEntry()
               .ToList();


            var a = input.Select(x => new { GaurdId = x.GuardId, ElapsedMinutesSleep = (x.EndTime - x.StartTime).TotalMinutes })
                .GroupBy(x => x.GaurdId)
                .Select(x => new { x.Key, nb = x.Sum(y => y.ElapsedMinutesSleep) })
                .OrderByDescending(x => x.nb)
                .FirstOrDefault();

            Console.WriteLine(a.Key);

            var b = input.Where(x => x.GuardId == a.Key).ToList();
            var c = b
                .SelectMany(x => ExpandDateSequence(x.StartTime, x.EndTime), (parent, child) => new { parent.GuardId, Time = child });

             var d = c.GroupBy(x => x.Time.Minute)
                .Select(x => new { x.Key, nb = x.Count() })
                .OrderByDescending(x => x.nb)
                .FirstOrDefault();


            Console.WriteLine(a.Key * d.Key);

            var allguards = input
                .SelectMany(x => ExpandDateSequence(x.StartTime, x.EndTime), (parent, child) => new { parent.GuardId, Time = child })
                .GroupBy(x => new { x.GuardId, x.Time.Minute })
                .Select(x => new { x.Key, nbb = x.Count() })
                .OrderByDescending(x => x.nbb)
                .FirstOrDefault();

            Console.WriteLine(allguards.Key.GuardId*allguards.Key.Minute);

            Console.ReadKey();
        }

        private static IEnumerable<DateTime> ExpandDateSequence(DateTime start, DateTime end)
        {
            var elapsedMinutes = (end - start).TotalMinutes;

            for (int index = 0; index < elapsedMinutes; index++)
            {
                yield return start.AddMinutes(index);
            }
        }
    }



    public static class Day4Extensions
    {
        public static DateTime ExtractTime(this string line)
        {
            var entryParts = line.Substring(1, line.Length - 1).Split("]", StringSplitOptions.RemoveEmptyEntries);
            return DateTime.Parse(entryParts[0]);
        }

        public static IEnumerable<DataEntry> AsDataEntry(this IEnumerable<string> entries)
        {
            var lines = entries.ToList();

            var cont = 0;
            while (cont < entries.Count()-1)
            {
                var beginTurnLine = lines[cont];
                int? guardId = ExtractGuardId(beginTurnLine);
                if (!guardId.HasValue) throw new Exception();

                var workLines = new List<string>();
                while (cont < entries.Count()-1)
                {
                    cont++;
                    if (IsGuardLine(lines[cont]))
                    {
                        System.Diagnostics.Debug.Assert(!lines[cont - 1].Contains("asleep"));
                        break;
                    }
                    workLines.Add(lines[cont]);
                }
                var asleepTimes = workLines.Where((x, i) => i % 2 == 0);
                var awakeTimes = workLines.Where((x, i) => i % 2 != 0);
                var ranges = asleepTimes.Zip(awakeTimes, (a, b) => new DataEntry { GuardId = guardId, StartTime = a.ExtractTime(), EndTime = b.ExtractTime() });

                foreach (var entry in ranges)
                {
                    yield return entry;
                }
            }
        }

        public static bool IsGuardLine(string workLine)
        {
            return regexGuard.IsMatch(workLine);
        }

        public static Regex regexGuard = new Regex(@"Guard #([0-9]+) begins shift");

        public static int? ExtractGuardId(string beginTurnLine)
        {
            return regexGuard.IsMatch(beginTurnLine) ?
                int.Parse(regexGuard.Match(beginTurnLine).Groups[1].Captures[0].Value)
                : (int?)null;
        }
    }
}
