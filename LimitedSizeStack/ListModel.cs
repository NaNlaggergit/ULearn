using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LimitedSizeStack;

public interface Actions<T>;

public class Delete<T> : Actions<T>
{
    public int Index;
    public T Value;
    public Delete(int index, T value)
    {
        Index = index;
        Value = value;
    }
}
public class Add<T> : Actions<T>
{
    public T Value;
    public Add(T value)
    {
        Value = value;
    }
}

public class ListModel<TItem>
{

    public List<TItem> Items { get; }
	public int UndoLimit;
    public LimitedSizeStack<Actions<TItem>> History;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
		
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
        History = new LimitedSizeStack<Actions<TItem>>(undoLimit);
    }

	public void AddItem(TItem item)
	{
		Items.Add(item);
		History.Push(new Add<TItem>(item));
	}

	public void RemoveItem(int index)
	{
		History.Push(new Delete<TItem>(index, Items[index]));
		Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
		if(History.Count > 0)
			return true;
		return false;
	}

	public void Undo()
	{
		if (CanUndo())
		{
			var action= History.Pop();
			if (action is Add<TItem>)
			{
				Items.RemoveAt(Items.Count - 1);
            }
			if(action is Delete<TItem>)
			{
				var delete = (Delete<TItem>)action;
				Items.Insert(delete.Index, delete.Value);
			}
		}
	}
}