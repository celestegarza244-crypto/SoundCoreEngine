namespace SoundCore.CustomStructures;

/// <summary>
/// Generic self-referencing node: the building block of SinglyLinkedList&lt;T&gt;.
/// Has no dependency on any external collection.
/// </summary>
/// <typeparam name="T">Type of the value stored in the node.</typeparam>
public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}
