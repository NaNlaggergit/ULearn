using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask
{
	public static IEnumerable<OwnedLocation> AssignOwners(Map map)
	{
        var queue = new Queue<OwnedLocation>();
        var visited=new HashSet<Point>();
		for(int i = 0; i < map.Players.Length; i++)
		{
			queue.Enqueue(new OwnedLocation(i,map.Players[i],0));
		}
		while(queue.Count > 0)
		{
			var dequeue=queue.Dequeue();
			var point = dequeue.Location;
			if (point.X < 0 || point.X >= map.Maze.GetLength(0) || point.Y < 0 || point.Y >= map.Maze.GetLength(1))
				continue;
			if (map.Maze[point.X, point.Y] is MapCell.Wall)
				continue;
			if (visited.Contains(point))
				continue;
			visited.Add(point);
			yield return dequeue;
			for(var dy=-1;dy <= 1; dy++)
			{
				for(var dx=-1;dx <= 1; dx++)
				{
					if (dx != 0 && dy != 0)
						continue;
                    if (map.Chests.Contains(point))
                        continue;
                    else
					{
						queue.Enqueue(new OwnedLocation(
							dequeue.Owner,
							new Point(point.X + dx, point.Y + dy),
							dequeue.Distance+1));
					}
				}
			}
		}
		yield break;
	}
}