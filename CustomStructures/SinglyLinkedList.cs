using System.Collections;

namespace SoundCore.CustomStructures;

/// <summary>
/// Generic, self-referencing singly linked list. Manipulates references
/// in live memory (Node&lt;T&gt;) without relying on external collections
/// or auxiliary arrays (Array, List&lt;T&gt;, etc.). It has zero knowledge
/// of Windows Forms: 100% reusable (Decoupling).
/// </summary>
/// <typeparam name="T">Type of data stored (e.g. Track).</typeparam>
public class SinglyLinkedList<T> : IPlaybackQueue<T>
{
    private Node<T>? _head;
    private Node<T>? _tail;
    private int _count;

    public int Count => _count;
    public bool IsEmpty => _head is null;

    /// <summary>Value currently at the head (the "now playing" track), or default if empty.</summary>
    public T? Head => _head is null ? default : _head.Value;

    // -----------------------------------------------------------------
    // 1. AddToEnd(T) — O(1) thanks to the persistent tail pointer.
    //    Queues a track at the end of the music session.
    // -----------------------------------------------------------------
    public void AddToEnd(T value)
    {
        var newNode = new Node<T>(value);

        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            _tail!.Next = newNode;
            _tail = newNode;
        }

        _count++;
    }

    // -----------------------------------------------------------------
    // 2. PlayNext(T) — O(1)
    //    Inserts immediately after the head ("Up Next VIP").
    // -----------------------------------------------------------------
    public void PlayNext(T value)
    {
        var newNode = new Node<T>(value);

        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
            _count++;
            return;
        }

        newNode.Next = _head.Next;
        _head.Next = newNode;

        if (ReferenceEquals(_tail, _head))
            _tail = newNode;

        _count++;
    }

    // -----------------------------------------------------------------
    // 3. AdvanceTrack() — O(1)
    //    Dequeues the currently playing track and updates the head.
    // -----------------------------------------------------------------
    public T AdvanceTrack()
    {
        if (_head is null)
            throw new InvalidOperationException(
                "The playback queue is empty. There is no track to advance to.");

        var value = _head.Value;
        _head = _head.Next;

        if (_head is null)
            _tail = null;

        _count--;
        return value;
    }

    // -----------------------------------------------------------------
    // 4. Reverse() — O(n) time, O(1) space, strictly in-place.
    //    Classic 3-pointer technique: previous, current, next.
    //    Strict rule: no temporary lists are created and values are not
    //    merely swapped; memory pointers are reoriented instead.
    // -----------------------------------------------------------------
    public void Reverse()
    {
        Node<T>? previous = null;
        Node<T>? current = _head;
        Node<T>? next;

        _tail = _head; // the old head becomes the new tail

        while (current is not null)
        {
            next = current.Next;        // 1. Save the rest
            current.Next = previous;    // 2. Flip the pointer
            previous = current;         // 3. Move previous forward
            current = next;             // 4. Move current forward
        }

        _head = previous;               // 5. New head
    }

    // -----------------------------------------------------------------
    // 5. InsertSorted(T, cmp) — O(n)
    //    Sorts the list according to the harmonic energy curve (e.g. BPM).
    // -----------------------------------------------------------------
    public void InsertSorted(T value, Comparison<T> comparer)
    {
        var newNode = new Node<T>(value);

        if (_head is null || comparer(value, _head.Value) < 0)
        {
            newNode.Next = _head;
            _head = newNode;
            _tail ??= newNode;
            _count++;
            return;
        }

        var current = _head;
        while (current.Next is not null && comparer(value, current.Next.Value) >= 0)
        {
            current = current.Next;
        }

        newNode.Next = current.Next;
        current.Next = newNode;

        if (ReferenceEquals(current, _tail))
            _tail = newNode;

        _count++;
    }

    // -----------------------------------------------------------------
    // 6. RemoveDuplicates(eq) — O(n²)
    //    Removes repeated tracks without using HashSets or auxiliary
    //    arrays. Keeps the first occurrence.
    // -----------------------------------------------------------------
    public int RemoveDuplicates(Func<T, T, bool> areEqual)
    {
        int removedCount = 0;

        var current = _head;
        while (current is not null)
        {
            var runner = current;
            while (runner.Next is not null)
            {
                if (areEqual(current.Value, runner.Next.Value))
                {
                    runner.Next = runner.Next.Next;
                    _count--;
                    removedCount++;

                    if (runner.Next is null)
                        _tail = runner;
                }
                else
                {
                    runner = runner.Next;
                }
            }
            current = current.Next;
        }

        return removedCount;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    // -----------------------------------------------------------------
    // Data Binding: IEnumerable<T> implemented with yield return so it
    // can be bound directly to the Windows Forms DataGridView.
    // -----------------------------------------------------------------
    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;
        while (current is not null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
