using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_3_B
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = DataProvider.Input_3();

            var squaresList = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    var splitId = x.Split("@", StringSplitOptions.RemoveEmptyEntries);

                    var split = splitId[1].Split(":", StringSplitOptions.RemoveEmptyEntries);
                    var coordinates = split[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var size = split[1].Split("x", StringSplitOptions.RemoveEmptyEntries);

                    return new { id = splitId[0], x = int.Parse(coordinates[0]), y = int.Parse(coordinates[1]), w = int.Parse(size[0]), h = int.Parse(size[1]) };
                });

            var coordinatesList = squaresList.Select(x => new { x.id, coordinates = ToCoordinates(x.x, x.y, x.w, x.h).Select(z => $"({z.x},{z.y})").ToList() }).ToList();

            var result = (string)null;
            foreach (var r in coordinatesList)
            {
                if (coordinatesList.All(x => r.id== x.id || !x.coordinates.Intersect(r.coordinates).Any()))
                {
                    result = r.id;
                }

            }
            
            Console.WriteLine(result);
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
