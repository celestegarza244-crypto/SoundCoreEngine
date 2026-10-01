using System.Collections;

namespace SoundCore.CustomStructures;

/// <summary>
/// Adapts System.Collections.Generic.LinkedList&lt;T&gt; (native .NET 10
/// doubly linked nodes) to the IPlaybackQueue&lt;T&gt; interface, so it can
/// be compared side by side with the custom list and with List&lt;T&gt;.
/// </summary>
public class LinkedListAdapter<T> : IPlaybackQueue<T>
{
    private readonly LinkedList<T> _list = new();

    public int Count => _list.Count;
    public bool IsEmpty => _list.Count == 0;

    public void AddToEnd(T value) => _list.AddLast(value);

    public void PlayNext(T value)
    {
        if (_list.First is null)
            _list.AddFirst(value);
        else
            _list.AddAfter(_list.First, value);
    }

    public T AdvanceTrack()
    {
        if (_list.First is null)
            throw new InvalidOperationException(
                "The playback queue is empty. There is no track to advance to.");

        var value = _list.First.Value;
        _list.RemoveFirst();
        return value;
    }

    public void Reverse()
    {
        // LinkedList<T> has no native Reverse(); rebuild it by walking
        // from the last node back to the first.
        var node = _list.Last;
        var reversed = new LinkedList<T>();
        while (node is not null)
        {
            reversed.AddLast(node.Value);
            node = node.Previous;
        }

        _list.Clear();
        foreach (var value in reversed)
            _list.AddLast(value);
    }

    public void InsertSorted(T value, Comparison<T> comparer)
    {
        var node = _list.First;
        while (node is not null && comparer(value, node.Value) >= 0)
            node = node.Next;

        if (node is null)
            _list.AddLast(value);
        else
            _list.AddBefore(node, value);
    }

    public int RemoveDuplicates(Func<T, T, bool> areEqual)
    {
        int removedCount = 0;
        var outer = _list.First;

        while (outer is not null)
        {
            var inner = outer.Next;
            while (inner is not null)
            {
                var nextInner = inner.Next;
                if (areEqual(outer.Value, inner.Value))
                {
                    _list.Remove(inner);
                    removedCount++;
                }
                inner = nextInner;
            }
            outer = outer.Next;
        }

        return removedCount;
    }

    public void Clear() => _list.Clear();

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
