using System.Diagnostics;
using SoundCore.CustomStructures;
using SoundCore.Models;
using SoundCore.Networking;

namespace SoundCore.UI;

public partial class MainForm : Form
{
    /// <summary>
    /// Polymorphic reference: depending on the selected mode it points to
    /// the custom list or to one of the native .NET 10 adapters. The UI
    /// never needs to know the concrete implementation.
    /// </summary>
    private IPlaybackQueue<Track> _queue = new SinglyLinkedList<Track>();

    private int _nextId = 1;

    public MainForm()
    {
        InitializeComponent();
        RefreshGrid();
    }

    // -----------------------------------------------------------------
    // Mode change: rebuilds the active collection while preserving the
    // current data, so the three structures can be compared side by side.
    // -----------------------------------------------------------------
    private void ModeCheckedChanged(object? sender, EventArgs e)
    {
        if (sender is not RadioButton rb || !rb.Checked) return;

        var currentItems = new List<Track>(_queue);

        _queue = rb.Name switch
        {
            _ when ReferenceEquals(rb, rbLinkedList) => new LinkedListAdapter<Track>(),
            _ when ReferenceEquals(rb, rbListT) => new ListAdapter<Track>(),
            _ => new SinglyLinkedList<Track>()
        };

        foreach (var track in currentItems)
            _queue.AddToEnd(track);

        RefreshGrid();
    }

    // -----------------------------------------------------------------
    // Abrir la ventana de red
    // -----------------------------------------------------------------
    // Ventana de red: NO se abre al iniciar el programa. Se crea la primera vez
    // que se pica el botón y después solo se vuelve a mostrar.
    private NetworkForm? _network;

    private void BtnNetwork_Click(object? sender, EventArgs e)
    {
        if (_network is null || _network.IsDisposed)
            _network = new NetworkForm(this);

        if (!_network.Visible) _network.Show();
        if (_network.WindowState == FormWindowState.Minimized)
            _network.WindowState = FormWindowState.Normal;

        _network.BringToFront();
        _network.Activate();
    }

    // Al cerrar el programa principal, cierra también la ventana de red
    // (así se detiene cualquier conexión que siga activa).
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        if (_network is { IsDisposed: false }) _network.Close();
        base.OnFormClosed(e);
    }

    // -----------------------------------------------------------------
    // Queue actions
    // -----------------------------------------------------------------
    private void BtnAddToEnd_Click(object? sender, EventArgs e)
    {
        if (!TryReadForm(out var track)) return;
        _queue.AddToEnd(track);
        RefreshGrid();
        ClearForm();
    }

    private void BtnPlayNext_Click(object? sender, EventArgs e)
    {
        if (!TryReadForm(out var track)) return;
        _queue.PlayNext(track);
        RefreshGrid();
        ClearForm();
    }

    // CP-07: Error Handling — advancing with an empty queue must show
    // an informative MessageBox, without crashing the application.
    private void BtnAdvanceTrack_Click(object? sender, EventArgs e)
    {
        try
        {
            var track = _queue.AdvanceTrack();
            RefreshGrid();
            MessageBox.Show(this, $"Reproduciendo ahora:\n{track}", "Avanzar Canción",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, "La cola está vacía. Agrega canciones antes de avanzar.", "Cola Vacía",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnReverse_Click(object? sender, EventArgs e)
    {
        _queue.Reverse();
        RefreshGrid();
    }

    private void BtnSortByBpm_Click(object? sender, EventArgs e)
    {
        var tracks = new List<Track>(_queue);
        _queue.Clear();

        foreach (var track in tracks)
            _queue.InsertSorted(track, (a, b) => a.Bpm.CompareTo(b.Bpm));

        RefreshGrid();
    }

    private void BtnRemoveDuplicates_Click(object? sender, EventArgs e)
    {
        int removedCount = _queue.RemoveDuplicates((a, b) =>
            string.Equals(a.Title, b.Title, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(a.Artist, b.Artist, StringComparison.OrdinalIgnoreCase));

        RefreshGrid();

        MessageBox.Show(this,
            removedCount > 0
                ? $"Se eliminaron {removedCount} canción(es) duplicada(s)."
                : "No se encontraron duplicados.",
            "Quitar Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // -----------------------------------------------------------------
    // Benchmark: "Live Duel" — 25,000 mid-list insertions (right after
    // the head) against the three collections.
    // -----------------------------------------------------------------
    private void BtnBenchmark_Click(object? sender, EventArgs e)
    {
        const int n = 25_000;

        UseWaitCursor = true;
        Application.DoEvents();

        long msCustom = MeasureMidListInsertion(new SinglyLinkedList<Track>(), n);
        long msLinkedList = MeasureMidListInsertion(new LinkedListAdapter<Track>(), n);
        long msListT = MeasureMidListInsertion(new ListAdapter<Track>(), n);

        UseWaitCursor = false;

        string message =
            "=== BENCHMARK DE ESTRÉS: 25,000 INSERCIONES A MITAD DE LISTA ===\n\n" +
            $"Lista Personalizada (Nodos) .. {msCustom,6} ms   ->  O(1) reconexión de punteros\n" +
            $".NET LinkedList<T> .......... {msLinkedList,6} ms   ->  O(1) nativa (nodos doblemente enlazados)\n" +
            $".NET List<T> ................. {msListT,6} ms   ->  O(n) cuello de botella (Array.Copy)\n\n" +
            "Justificación científica:\n" +
            "La Lista Personalizada y LinkedList<T> solo reorientan 2 referencias de memoria\n" +
            "por inserción (O(1)); nunca mueven el resto de los nodos. List<T>,\n" +
            "al estar respaldada por un arreglo contiguo, debe desplazar cada elemento\n" +
            "posterior al punto de inserción en memoria (O(n)), lo que explica su\n" +
            "rendimiento decreciente conforme crece la cola.";

        MessageBox.Show(this, message, "Resultados de Telemetría",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static long MeasureMidListInsertion(IPlaybackQueue<Track> queue, int n)
    {
        // Seed the queue so a real "mid-list" position (right behind the
        // head) already exists before the first timed insertion.
        queue.AddToEnd(new Track(0, "Seed A", "N/A", 120, 180));
        queue.AddToEnd(new Track(-1, "Seed B", "N/A", 120, 180));

        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < n; i++)
            queue.PlayNext(new Track(i, $"Track {i}", "Bench", 120, 180));

        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    // -----------------------------------------------------------------
    // Reactive sync of the right-hand panel
    // -----------------------------------------------------------------
    private void RefreshGrid()
    {
        var snapshot = new List<Track>(_queue); // walked via IEnumerable<T> (yield return)

        dgvPlaylist.DataSource = null;
        dgvPlaylist.DataSource = snapshot;

        if (dgvPlaylist.Columns["Id"] is { } colId) colId.HeaderText = "Id";
        if (dgvPlaylist.Columns["Title"] is { } colTitle) colTitle.HeaderText = "Título";
        if (dgvPlaylist.Columns["Artist"] is { } colArtist) colArtist.HeaderText = "Artista";
        if (dgvPlaylist.Columns["Bpm"] is { } colBpm) colBpm.HeaderText = "BPM";
        if (dgvPlaylist.Columns["DurationSeconds"] is { } colDur) colDur.HeaderText = "Duración (s)";

        lblNowPlaying.Text = snapshot.Count > 0
            ? $"▶ Reproduciendo: \"{snapshot[0].Title}\" - {snapshot[0].Artist} ({snapshot[0].Bpm} BPM)"
            : "▶ Reproduciendo: (cola vacía)";

        lblTotalQueue.Text = $"Total en cola: {snapshot.Count} canciones";

        int totalSeconds = 0;
        foreach (var t in snapshot) totalSeconds += t.DurationSeconds;
        var ts = TimeSpan.FromSeconds(totalSeconds);
        lblTotalTime.Text = $"Tiempo total: {(int)ts.TotalMinutes}:{ts.Seconds:D2}";
    }

    // -----------------------------------------------------------------
    // Registration form validation
    // -----------------------------------------------------------------
    private bool TryReadForm(out Track track)
    {
        track = default!;

        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            MessageBox.Show(this, "El título es obligatorio.", "Faltan Datos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTitle.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtArtist.Text))
        {
            MessageBox.Show(this, "El artista es obligatorio.", "Faltan Datos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtArtist.Focus();
            return false;
        }

        track = new Track(
            _nextId++,
            txtTitle.Text.Trim(),
            txtArtist.Text.Trim(),
            (int)numBpm.Value,
            (int)numDuration.Value);

        return true;
    }

    private void ClearForm()
    {
        txtTitle.Clear();
        txtArtist.Clear();
        numBpm.Value = 120;
        numDuration.Value = 180;
        txtTitle.Focus();
    }
}
