using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_3_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = DataProvider.Input_3();

            var squaresList = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split("@", StringSplitOptions.RemoveEmptyEntries)[1])
                .Select(x =>
                {
                    var split = x.Split(":", StringSplitOptions.RemoveEmptyEntries);
                    var coordinates = split[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var size = split[1].Split("x", StringSplitOptions.RemoveEmptyEntries);

                    return new { x = int.Parse(coordinates[0]), y = int.Parse(coordinates[1]), w = int.Parse(size[0]), h = int.Parse(size[1]) };
                });

            var coordinatesList = squaresList.SelectMany(x => ToCoordinates(x.x, x.y, x.w, x.h));

             var overlaps = coordinatesList.GroupBy(x=> $"{x.x},{x.y}")
                .Where(x=> x.Count() > 1)
                .Count();

            Console.WriteLine(overlaps);
            Console.ReadKey();

        }

        private static IEnumerable<(int x, int y)> ToCoordinates(int l, int t, int w, int h)
        {
            for (var width = 0; width < w; width++)
                for (var height = 0; height < h; height++)
                {
                   var result = (l + width, t + height);
                   yield return result;
                }
        }

    }
}
