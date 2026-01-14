using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
	private static Point[] Directions =
	{
		new Point(1,0),
		new Point(-1,0),
		new Point(0,1),
		new Point(0,-1),
	};

	public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
	{
		var visited=new HashSet<Point>();
        var queue = new Queue<SinglyLinkedList<Point>>();
		var chestPos=new HashSet<Point>(
			chests.Select(c=>c.Location)
			);
		queue.Enqueue(new SinglyLinkedList<Point>(start));
		visited.Add(start);
		while (queue.Count > 0)
		{
			var path=queue.Dequeue();
			var current = path.Value;
			if(chestPos.Contains(current))
				yield return path;
			foreach(var dir in Directions)
			{
				var next = new Point(
					current.X + dir.X,
					current.Y + dir.Y);
				if(!IsValid(map, next)||visited.Contains(next))
				{
					continue;
				}
				visited.Add(next);
				queue.Enqueue(new SinglyLinkedList<Point>(next, path));
			}
		}
	}
	private static bool IsValid(Map map,Point p)
	{
		return p.X >= 0 && p.Y >= 0 &&
			p.X < map.Dungeon.GetLength(0) &&
			p.Y < map.Dungeon.GetLength(1) &&
			map.Dungeon[p.X, p.Y] == MapCell.Empty;
	}
}