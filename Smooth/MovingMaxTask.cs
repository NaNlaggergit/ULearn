using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var window=new Queue<DataPoint>();
		var candidates=new LinkedList<DataPoint>();
		foreach (var point in data)
		{
			window.Enqueue(point);
			while(candidates.Count > 0&& candidates.Last.Value.OriginalY <= point.OriginalY)
			{
				candidates.RemoveLast();
			}
			candidates.AddLast(point);
			if(window.Count > windowWidth)
			{
				var removed = window.Dequeue();
				if (candidates.First.Value == removed)
				{
					candidates.RemoveFirst();
				}
			}
			double cirrentMax=candidates.First.Value.OriginalY;
			yield return point.WithMaxY(cirrentMax);
		}
	}
}