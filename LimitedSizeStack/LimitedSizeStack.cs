using System;
using System.Collections.Generic;
using System.Formats.Tar;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	private readonly int _limit;
	private readonly LinkedList<T> _list =new LinkedList<T>();

	public LimitedSizeStack(int undoLimit)
	{
		if(undoLimit < 0)
			throw new ArgumentOutOfRangeException(nameof(undoLimit));
		_limit = undoLimit;
	}

	public void Push(T item)
	{
		if (_limit == 0)
			return;
		if(_list.Count == _limit)
		{
			_list.RemoveFirst();
		}
        _list.AddLast(item);
    }

	public T Pop()
	{
		var last= _list.Last.Value;
		_list.RemoveLast();
		return last;
	}

	public int Count => _list.Count;
}