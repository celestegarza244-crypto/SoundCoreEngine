# SoundCore Engine v2.0 — Unit 2 Challenge

A Windows Forms project in **.NET 10 / C# 14** that implements a custom
singly linked list (`SinglyLinkedList<T>`) to manage a DJ's live setlist,
and contrasts it against .NET's `LinkedList<T>` and `List<T>`.

## How to open it

1. Requires Visual Studio 2022 (17.10+) or the **.NET 10** SDK with the
   ".NET desktop development" workload.
2. Open `SoundCoreEngine.csproj` and press F5, or build/run from a
   console on Windows:
   ```
   dotnet build
   dotnet run
   ```
   > Note: Windows Forms only builds/runs on Windows.

## Structure (layered architecture)

```
SoundCoreEngine/
├── Models/
│   └── Track.cs                  // immutable record with DJ metadata
├── CustomStructures/
│   ├── Node.cs                   // generic self-referencing node
│   ├── IPlaybackQueue.cs         // common interface (.NET 10 Contrast)
│   ├── SinglyLinkedList.cs       // ★ the 6 algorithms built from scratch
│   ├── LinkedListAdapter.cs      // wraps the native LinkedList<T>
│   └── ListAdapter.cs            // wraps the native List<T>
├── UI/
│   ├── MainForm.cs               // logic and reactive sync
│   └── MainForm.Designer.cs      // control layout
└── Program.cs
```

## Rubric mapping

| Criterion | % | Where |
|---|---|---|
| Pointers & Nodes | 40% | `Node.cs` + the 6 methods in `SinglyLinkedList.cs`, which only reorient references, with no auxiliary arrays/List |
| .NET 10 Contrast | 20% | `IPlaybackQueue<T>` implemented in parallel by `SinglyLinkedList`, `LinkedListAdapter` and `ListAdapter`; the "Mode" radio buttons swap the active implementation live |
| Real Telemetry | 15% | `BtnBenchmark_Click` + `MeasureMidListInsertion` in `MainForm.cs`, using `Stopwatch` over 25,000 insertions |
| Presentation Bonus | 25% | `MainForm.Designer.cs`: dark-themed UI with a `DataGridView`, DJ controls and a now-playing monitor |

## The 6 required algorithms

| Method | Complexity | Action |
|---|---|---|
| `AddToEnd(T)` | O(1) (tail pointer) | Queues a track at the end of the session |
| `PlayNext(T)` | O(1) | Inserts right after the head (Up Next VIP) |
| `AdvanceTrack()` | O(1) | Dequeues the head |
| `Reverse()` | O(n) time / O(1) space | In-place reversal, 3-pointer technique |
| `InsertSorted(T, cmp)` | O(n) | Sorts by BPM curve |
| `RemoveDuplicates(eq)` | O(n²) | Removes duplicates without HashSet/arrays |

## Covered test cases (CP-01 to CP-07)

- **CP-01** — Register "Track 1" (120 BPM) + "Add to End" → appears at the bottom of the grid.
- **CP-02** — Register "Track VIP" + "Play Next" → appears in row 2 (right behind the head).
- **CP-03** — "Advance Track" → the head starts playing (MessageBox) and leaves the grid.
- **CP-04** — Load tracks with BPM [100,110,120] + "Reverse List" → grid shows [120,110,100], with zero extra memory.
- **CP-05** — Load BPM 128,115,140,120 + "Sort by BPM Curve" → reorders ascending: 115→120→128→140.
- **CP-06** — Load repeated titles + "Remove Duplicates" → keeps the first one, deletes the rest.
- **CP-07** — "Advance Track" on an empty queue → informative `MessageBox`, no unhandled exception.

## Expected benchmark result

With 25,000 mid-list insertions:

| Collection | Complexity | Reason |
|---|---|---|
| Custom List (Nodes) | O(1) | Only reconnects 2 references per insertion |
| `.NET LinkedList<T>` | O(1) | Native doubly linked nodes |
| `.NET List<T>` | O(n) | Backed by a contiguous array → `Array.Copy` |

## Extras añadidos

- **Interfaz en español**: solo se tradujeron los textos visibles de `MainForm.Designer.cs` y `MainForm.cs`.
- **G Player** (`UI/PlayerForm.cs` + `UI/PlayerForm.Designer.cs`): reproductor DJ independiente con logo "G" que se abre junto con el programa
  (play/pausa, detener, anterior/siguiente, barra de progreso, volumen, tempo y lista de reproducción; acepta arrastrar archivos).
  Usa Windows Media Player por COM, sin paquetes NuGet.
