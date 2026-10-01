namespace SoundCore.CustomStructures;

/// <summary>
/// Common contract that lets the app swap, at runtime, under the same
/// interface, between the custom SinglyLinkedList&lt;T&gt; and the native
/// .NET 10 implementations (LinkedList&lt;T&gt; and List&lt;T&gt;).
/// This enables the ".NET 10 Contrast" requirement (20% of the grade)
/// without duplicating logic in the UI layer.
/// </summary>
public interface IPlaybackQueue<T> : IEnumerable<T>
{
    int Count { get; }
    bool IsEmpty { get; }

    /// <summary>Queues a track at the end of the music session.</summary>
    void AddToEnd(T value);

    /// <summary>Inserts immediately after the head (Up Next VIP).</summary>
    void PlayNext(T value);

    /// <summary>Dequeues the currently playing track (head) and updates the head.</summary>
    T AdvanceTrack();

    /// <summary>Reverses the whole setlist.</summary>
    void Reverse();

    /// <summary>Inserts keeping the list sorted according to the harmonic energy curve (comparator).</summary>
    void InsertSorted(T value, Comparison<T> comparer);

    /// <summary>Removes repeated tracks without using HashSets or auxiliary arrays. Returns how many were removed.</summary>
    int RemoveDuplicates(Func<T, T, bool> areEqual);

    void Clear();
}
