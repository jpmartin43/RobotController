using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotController
{
    public class Program
    {
        static void Main(string[] args)
        {
            var testGrid = InitializeGrid(5, 5);
            var testObstacles = new List<IObstacle>
            {
                new Rock(new [] { 2, 2 }, testGrid),
                new Rock(new [] { 2, 3 }, testGrid),
                new Hole(new [] { 1, 1 }, new [] { 0, 1 }, testGrid),
                new Hole(new [] { 4, 3 }, new [] { 2, 4 }, testGrid),
                new Spinner(270, new [] { 1, 2 }, testGrid),
                new Spinner(90, new [] { 0, 3 }, testGrid)
            };
            PopulateGrid(testObstacles, ref testGrid);

            var robot = new Robot()
            {
                Direction = 2
            };

            // TODO: figure out why the third instruction isn't updating properly
            var instructions = "FFLFFRRFRLFRFFRFFL"; // should end at [4, 3] with direction 2 (180)

            Console.WriteLine("Output result will be in the following format:");
            Console.WriteLine("instruction: [x, y] (direction)");
            Console.WriteLine();

            Console.WriteLine("Starting parameters:");
            Console.WriteLine(" : [{0},{1}] ({2})", robot.Position[0], robot.Position[1], robot.Direction * 90);

            foreach (var i in instructions)
            {
                robot.Move(i.ToString(), testGrid);
                Console.WriteLine("{0}: [{1},{2}] ({3})"
                    ,i , robot.Position[0], robot.Position[1], robot.Direction * 90);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates a grid with the provided width and height
        /// </summary>
        public static Dictionary<int[], IObstacle> InitializeGrid(int width, int height)
        {
            var grid = new Dictionary<int[], IObstacle>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    grid.Add(new[] { x, y }, null);
                }
            }

            return grid;
        }

        public static void PopulateGrid(IEnumerable<IObstacle> obstacles, ref Dictionary<int[], IObstacle> grid)
        {
            foreach (var o in obstacles)
            {
                if (!grid.Any(g => g.Key.SequenceEqual(o.Position))) continue;
                var gridEntry = grid.FirstOrDefault(g => g.Key.SequenceEqual(o.Position));
                grid[gridEntry.Key] = grid[gridEntry.Key] ?? o;
            }
        }
    }
}
