using Core;
using System;
using System.Linq;

namespace Day_6_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var coordinates = DataProvider.Input_6()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x=> !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .Select((c, i) => new { x = c[0].ParseInt(), y = c[1].ParseInt()})
                .ToList();

            var left = coordinates.Min(x => x.x);
            var right = coordinates.Max(x => x.x);
            var top = coordinates.Min(x => x.y);
            var buttom = coordinates.Max(x=> x.y);

            var cellsUniverse = (
                    from x in Enumerable.Range(left, right)
                    from y in Enumerable.Range(top, buttom)
                    select new { x, y }).
                    ToList();

            var cellsUniverseWithDistance = (
                    from cell in cellsUniverse
                    from coord in coordinates
                    select new { cell, coord, distance = Extensions.ManhattanDistance(cell.x, cell.y, coord.x, coord.y) }).ToList();

            var shortestPaths = cellsUniverseWithDistance.GroupBy(x => x.cell)
                .Select(x => new { cell = x.Key, minDistance = x.Min(y => y.distance) })
                .ToList();
            var cellsWithShortestPath = (from d in cellsUniverseWithDistance
                    join s in shortestPaths
                     on new { d.cell, d.distance } equals new { s.cell, distance = s.minDistance }
                    select d).ToList();

            var conflictingCells = cellsWithShortestPath.GroupBy(x => x.cell)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();

            var excludeConflictingList = (from d in cellsWithShortestPath
                                join c in conflictingCells
                                on d.cell equals c
                                select d).ToList();



            var cellsWithoutConflicts = cellsWithShortestPath.Except(excludeConflictingList).ToList();
            var coordRemaining = cellsWithoutConflicts.GroupBy(x => x.coord).Select(x => x.Key).Count();

            var unboundedCoordinates = cellsWithoutConflicts.GroupBy(x => x.coord)
               .Select(x => new
               {
                   coord = x.Key,
                   left = x.Select(c => c.cell.x).Min(),
                   right = x.Select(c => c.cell.x).Max(),
                   top = x.Select(c => c.cell.y).Min(),
                   buttom = x.Select(c => c.cell.y).Max()
               }).
               Where(x => x.left == left || x.right == right || x.top == top || x.buttom == buttom)
               .Select(x=> x.coord)
               .ToList();

            var boundedCoordinates1 = cellsWithoutConflicts.GroupBy(x => x.coord)
               .Select(x => new
               {
                   coord = x.Key,
                   left = x.Select(c => c.cell.x).Min(),
                   right = x.Select(c => c.cell.x).Max(),
                   top = x.Select(c => c.cell.y).Min(),
                   buttom = x.Select(c => c.cell.y).Max()
               }).
               Where(x => x.left > left && x.right < right && x.top > top && x.buttom < buttom)
               .Select(x => x.coord)
               .ToList();

            var cellsWithBoundaries1 = (from d in cellsWithoutConflicts
                                         join c in boundedCoordinates1
                                         on d.coord equals c
                                         select d).ToList();
            var nbOfBoundedCells = cellsWithBoundaries1.Count();
            var nbOfBoundedCoordinates = cellsWithBoundaries1.GroupBy(x => x.coord).Count();

            var maxArea = cellsWithBoundaries1.GroupBy(x => x.coord)
                .Select(x => new { coord = x.Key, nb = x.Count()}).OrderByDescending(x=> x.nb).FirstOrDefault();
            var cells = cellsWithoutConflicts.Where(x => x.coord == maxArea.coord).ToList();
            var test = cells.Where(x => x.cell.x == left || x.cell.x == right || x.cell.y == top || x.cell.y == buttom).ToList();
        

            Console.WriteLine(maxArea.nb);


            var maxRegion = cellsUniverseWithDistance.GroupBy(x => x.cell).Where(x => x.Select(y => y.distance).Sum() < 10000).Select(x => x.Key).Count();


            Console.ReadKey();
        }
    }


}
