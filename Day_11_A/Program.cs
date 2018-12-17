using System;
using System.Linq;

namespace Day_11_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var serial = 8979;
            var range = Enumerable.Range(1, 300);
            var cells = (from r1 in range
                         from r2 in range
                         select new { x = r1, y = r2 })
                         .Select(x => new { x.x, x.y, power = ( ( ( (10 + x.x) * x.y + serial ) * ( 10 + x.x ) / 100 ) % 10 ) - 5   })
                         .ToList();

            var searchRange = Enumerable.Range(2, 299);
            var searchCells = (from r1 in searchRange
                               from r2 in searchRange
                               select new { x = r1, y = r2 }).ToList();

            var maxPower = searchCells.AsParallel().Select(s => new { s.x, s.y, totalpower = cells.Where(c => c.x >= s.x - 1 && c.x <= s.x + 1 && c.y >= s.y - 1 && c.y <= s.y + 1).Sum(z => z.power) }).OrderByDescending(x => x.totalpower).FirstOrDefault();
            Console.WriteLine(maxPower);

            Console.ReadKey();

        }
    }
}
