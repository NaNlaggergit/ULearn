using Avalonia.Input;
using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var queue=new Queue<DataPoint>();
		double sum = 0;
		foreach(DataPoint point in data)
		{
			queue.Enqueue(point);
			sum += point.OriginalY;
			if(queue.Count > windowWidth)
			{
				var remove=queue.Dequeue();
				sum -= remove.OriginalY;
			}
			double avarage=sum/queue.Count;
			yield return point.WithAvgSmoothedY(avarage);
		}
	}
}