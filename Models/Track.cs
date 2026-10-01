namespace SoundCore.Models;

/// <summary>
/// Immutable record holding the DJ metadata of a music track.
/// This is the basic building block that will travel inside each Node&lt;T&gt;.
/// </summary>
public record Track(
    int Id,
    string Title,
    string Artist,
    int Bpm,
    int DurationSeconds)
{
    public override string ToString() =>
        $"[{Id:D3}] {Title} - {Artist} | {Bpm} BPM";
}
