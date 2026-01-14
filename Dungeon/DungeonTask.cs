using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        private static Point[] Directions =
{
        new Point(1,0),
        new Point(-1,0),
        new Point(0,1),
        new Point(0,-1),
    };
        public static MoveDirection[] FindShortestPath(Map map)
        {
            //var pathstoExit = FindPathsToEnd(map, map.InitialPosition, map.Exit)
            //      .ToList();

            // if (pathstoExit.Count == 1)
            // {
            //         var points = pathstoExit[0].Reverse().ToList();
            //         foreach (var chest in map.Chests)
            //         {
            //             if (points.Contains(chest.Location))
            //             {
            //                 return points
            //                     .Zip(points.Skip(1), (from, to) => GetDirection(from, to))
            //                     .ToArray();
            //             }
            //         }

            // }
            var pathsStart = BfsTask
                .FindPaths(map, map.InitialPosition, map.Chests)
                .ToDictionary(p => p.Value, p => p);

            var pathsExit = BfsTask
                .FindPaths(map, map.Exit, map.Chests)
                .ToDictionary(p => p.Value, p => p);

            var bestChest = map.Chests
            .Where(c => pathsStart.ContainsKey(c.Location) && pathsExit.ContainsKey(c.Location))
            .MinBy(c => (pathsStart[c.Location].Count() + pathsExit[c.Location].Count() - 1, -c.Value));

            var bestPath = bestChest == null ? null :
                pathsStart[bestChest.Location]
                    .Reverse()
                    .Concat(pathsExit[bestChest.Location].Skip(1))
                    .ToList();

            if (bestPath == null)
            {
                var pathToExit = BfsTask
                    .FindPaths(map, map.InitialPosition, new[] { new Chest(map.Exit, 0) })
                    .FirstOrDefault();

                if (pathToExit == null)
                    return new MoveDirection[0];

                var points = pathToExit.Reverse().ToList();
                return points
                    .Zip(points.Skip(1), (from, to) => GetDirection(from, to))
                    .ToArray();
            }

            return bestPath
                .Zip(bestPath.Skip(1), (from, to) => GetDirection(from, to))
                .ToArray();
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPathsToEnd(Map map, Point start, Point end)
        {
            var distance = new Dictionary<Point, int>();
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            distance[start] = 0;
            int shortestDistance = int.MaxValue;

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var current = path.Value;
                int currentDistance = distance[current];
                if (currentDistance > shortestDistance)
                    continue;

                if (current == end)
                {
                    shortestDistance = currentDistance;
                    yield return path;
                    continue;
                }
                foreach (var dir in Directions)
                {
                    var next = new Point(
                        current.X + dir.X,
                        current.Y + dir.Y);
                    if (!IsValid(map, next))
                    {
                        continue;
                    }
                    int nextDistance = currentDistance + 1;

                    // Если клетка не посещалась
                    if (!distance.ContainsKey(next))
                    {
                        distance[next] = nextDistance;
                        queue.Enqueue(new SinglyLinkedList<Point>(next, path));
                    }
                    // Если найден альтернативный кратчайший путь
                    else if (distance[next] == nextDistance)
                    {
                        queue.Enqueue(new SinglyLinkedList<Point>(next, path));
                    }
                }
            }
        }

        private static bool IsValid(Map map, Point p)
        {
            return p.X >= 0 && p.Y >= 0 &&
                p.X < map.Dungeon.GetLength(0) &&
                p.Y < map.Dungeon.GetLength(1) &&
                map.Dungeon[p.X, p.Y] == MapCell.Empty;
        }

        private static MoveDirection GetDirection(Point from, Point to)
        {
            if (to.X == from.X + 1) return MoveDirection.Right;
            if (to.X == from.X - 1) return MoveDirection.Left;
            if (to.Y == from.Y + 1) return MoveDirection.Down;
            return MoveDirection.Up;
        }
    }
}