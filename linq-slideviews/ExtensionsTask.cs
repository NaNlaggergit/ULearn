using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		int count = 0;
		List<double> sortedList = new List<double>();
        foreach (var item in items)
        {
			sortedList.Add(item);
			count++;
        }
		if (count > 0)
		{
			bool even = count % 2 == 0;
			sortedList.Sort();
			return !even ?
				sortedList.ElementAt(count / 2) :
				(sortedList.ElementAt(count / 2) + sortedList.ElementAt((count / 2) - 1)) / 2;
		}
		else throw new InvalidOperationException();
			
    }

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		var iterator = items.GetEnumerator();
		iterator.MoveNext();
		var past = iterator.Current;
		while (iterator.MoveNext())
		{
			yield return (past, iterator.Current);
			past =iterator.Current;
		}
	}
}