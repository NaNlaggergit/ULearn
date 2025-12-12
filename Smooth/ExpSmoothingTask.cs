using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		double? prev = null;
		foreach (DataPoint point in data)
		{
			double smoothed;
			if (prev == null)
			{
				smoothed = point.OriginalY;
			}
            else
            {
				smoothed = alpha * point.OriginalY + (1 - alpha) * prev.Value;
            }
			prev = smoothed;
            yield return point.WithExpSmoothedY(smoothed);
        }
	}
}
