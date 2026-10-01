using System.Collections;

namespace SoundCore.CustomStructures;

/// <summary>
/// Adapts System.Collections.Generic.List&lt;T&gt; (native .NET 10 dynamic
/// array) to the IPlaybackQueue&lt;T&gt; interface. It's included on purpose
/// as the benchmark's "villain": being backed by a contiguous array, a
/// mid-list insertion forces an internal Array.Copy, i.e. O(n).
/// </summary>
public class ListAdapter<T> : IPlaybackQueue<T>
{
    private readonly List<T> _list = new();

    public int Count => _list.Count;
    public bool IsEmpty => _list.Count == 0;

    public void AddToEnd(T value) => _list.Add(value);

    public void PlayNext(T value)
    {
        if (_list.Count == 0)
            _list.Add(value);
        else
            _list.Insert(1, value); // forces a memory shift: internal Array.Copy
    }

    public T AdvanceTrack()
    {
        if (_list.Count == 0)
            throw new InvalidOperationException(
                "The playback queue is empty. There is no track to advance to.");

        var value = _list[0];
        _list.RemoveAt(0);
        return value;
    }

    public void Reverse() => _list.Reverse();

    public void InsertSorted(T value, Comparison<T> comparer)
    {
        int i = 0;
        while (i < _list.Count && comparer(value, _list[i]) >= 0)
            i++;

        _list.Insert(i, value);
    }

    public int RemoveDuplicates(Func<T, T, bool> areEqual)
    {
        int removedCount = 0;
        for (int i = 0; i < _list.Count; i++)
        {
            for (int j = _list.Count - 1; j > i; j--)
            {
                if (areEqual(_list[i], _list[j]))
                {
                    _list.RemoveAt(j);
                    removedCount++;
                }
            }
        }
        return removedCount;
    }

    public void Clear() => _list.Clear();

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
