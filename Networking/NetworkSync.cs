using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using SoundCore.CustomStructures;
using SoundCore.Models;
using SoundCore.UI;

namespace SoundCore.Networking;

/// <summary>Mensaje de red: la lista completa de canciones (JSON en una sola línea).</summary>
public record StateMessage(string Type, List<Track> Tracks);

/// <summary>
/// Sincroniza la cola de reproducción entre varias computadoras por TCP
/// (topología en estrella: 1 anfitrión + N clientes). NO modifica MainForm:
/// observa su cola por reflexión, detecta cambios y los transmite; al recibir
/// cambios remotos los aplica sobre la misma cola y refresca la tabla.
/// </summary>
public sealed class NetworkSync : IDisposable
{
    private sealed class Peer
    {
        public required TcpClient Client { get; init; }
        public required StreamWriter Writer { get; init; }
        public string Name { get; init; } = "";
    }

    private readonly MainForm _form;
    private readonly FieldInfo _queueField;
    private readonly FieldInfo _nextIdField;
    private readonly MethodInfo _refreshGrid;
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 400 };
    private readonly List<Peer> _peers = new();
    private readonly object _peersLock = new();

    private List<Track> _lastSnapshot = new();
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;

    public bool IsRunning { get; private set; }
    public bool IsHost { get; private set; }
    public int PeerCount { get { lock (_peersLock) return _peers.Count; } }

    public event Action<string>? Log;
    public event Action? PeersChanged;

    public NetworkSync(MainForm form)
    {
        _form = form;
        const BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
        _queueField = typeof(MainForm).GetField("_queue", f)
            ?? throw new InvalidOperationException("No se encontró MainForm._queue");
        _nextIdField = typeof(MainForm).GetField("_nextId", f)
            ?? throw new InvalidOperationException("No se encontró MainForm._nextId");
        _refreshGrid = typeof(MainForm).GetMethod("RefreshGrid", f)
            ?? throw new InvalidOperationException("No se encontró MainForm.RefreshGrid");

        _timer.Tick += (_, _) => DetectLocalChange();
    }

    private IPlaybackQueue<Track> Queue => (IPlaybackQueue<Track>)_queueField.GetValue(_form)!;

    // ------------------------------------------------------------------
    // Arranque / paro
    // ------------------------------------------------------------------
    public void StartHost(int port)
    {
        Stop();
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        IsHost = true;
        IsRunning = true;
        _lastSnapshot = new List<Track>(Queue);
        _timer.Start();
        Log?.Invoke($"Anfitrión escuchando en el puerto {port}.");
        _ = AcceptLoopAsync(_listener, _cts.Token);
    }

    public async Task StartClientAsync(string host, int port)
    {
        Stop();
        _cts = new CancellationTokenSource();
        var client = new TcpClient();
        await client.ConnectAsync(host, port, _cts.Token);
        IsHost = false;
        IsRunning = true;
        _lastSnapshot = new List<Track>(Queue);
        var peer = AddPeer(client, $"{host}:{port}");
        _timer.Start();
        Log?.Invoke($"Conectado a {host}:{port}.");
        _ = ReadLoopAsync(peer, _cts.Token);
    }

    public void Stop()
    {
        _timer.Stop();
        _cts?.Cancel();
        try { _listener?.Stop(); } catch { }
        _listener = null;

        lock (_peersLock)
        {
            foreach (var p in _peers) { try { p.Client.Close(); } catch { } }
            _peers.Clear();
        }

        if (IsRunning) Log?.Invoke("Red detenida.");
        IsRunning = false;
        PeersChanged?.Invoke();
    }

    // ------------------------------------------------------------------
    // Conexiones
    // ------------------------------------------------------------------
    private async Task AcceptLoopAsync(TcpListener listener, CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(ct);
                var name = client.Client.RemoteEndPoint?.ToString() ?? "desconocido";
                var peer = AddPeer(client, name);
                Post(() =>
                {
                    Log?.Invoke($"Se conectó {name}.");
                    // El nuevo equipo recibe de inmediato la lista actual.
                    SendLine(peer, Serialize(new List<Track>(Queue)));
                });
                _ = ReadLoopAsync(peer, ct);
            }
        }
        catch (OperationCanceledException) { }
        catch (ObjectDisposedException) { }
        catch (Exception ex) when (!ct.IsCancellationRequested)
        {
            Post(() => Log?.Invoke("Error al aceptar: " + ex.Message));
        }
    }

    private Peer AddPeer(TcpClient client, string name)
    {
        client.NoDelay = true;
        var peer = new Peer
        {
            Client = client,
            Name = name,
            Writer = new StreamWriter(client.GetStream(), new UTF8Encoding(false)) { AutoFlush = true }
        };
        lock (_peersLock) _peers.Add(peer);
        Post(() => PeersChanged?.Invoke());
        return peer;
    }

    private void RemovePeer(Peer peer)
    {
        bool removed;
        lock (_peersLock) removed = _peers.Remove(peer);
        try { peer.Client.Close(); } catch { }
        if (removed)
        {
            Post(() =>
            {
                Log?.Invoke($"Se desconectó {peer.Name}.");
                PeersChanged?.Invoke();
            });
        }
    }

    private async Task ReadLoopAsync(Peer peer, CancellationToken ct)
    {
        try
        {
            using var reader = new StreamReader(peer.Client.GetStream(), Encoding.UTF8);
            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(ct);
                if (line is null) break; // el otro lado cerró

                StateMessage? msg;
                try { msg = JsonSerializer.Deserialize<StateMessage>(line); }
                catch { continue; }
                if (msg?.Tracks is null) continue;

                Post(() =>
                {
                    ApplyRemote(msg.Tracks);
                    if (IsHost) Broadcast(line, except: peer); // el anfitrión retransmite
                });
            }
        }
        catch (OperationCanceledException) { }
        catch { }
        finally { RemovePeer(peer); }
    }

    // ------------------------------------------------------------------
    // Envío
    // ------------------------------------------------------------------
    private static string Serialize(List<Track> tracks) =>
        JsonSerializer.Serialize(new StateMessage("state", tracks));

    private void SendLine(Peer peer, string line)
    {
        try { lock (peer.Writer) peer.Writer.WriteLine(line); }
        catch { RemovePeer(peer); }
    }

    private void Broadcast(string line, Peer? except = null)
    {
        Peer[] copy;
        lock (_peersLock) copy = _peers.ToArray();
        foreach (var p in copy)
            if (!ReferenceEquals(p, except)) SendLine(p, line);
    }

    // ------------------------------------------------------------------
    // Sincronización con la cola local
    // ------------------------------------------------------------------
    /// <summary>Cada 400 ms compara la cola con la última versión conocida.</summary>
    private void DetectLocalChange()
    {
        var snapshot = new List<Track>(Queue);
        if (snapshot.SequenceEqual(_lastSnapshot)) return; // los records comparan por valor

        _lastSnapshot = snapshot;
        Broadcast(Serialize(snapshot));
    }

    /// <summary>Reemplaza la cola local con la recibida (gana el último cambio).</summary>
    private void ApplyRemote(List<Track> tracks)
    {
        var queue = Queue;
        queue.Clear();
        foreach (var t in tracks) queue.AddToEnd(t);

        // Evita que un Id local choque con los que llegaron de otra PC.
        int maxId = tracks.Count > 0 ? tracks.Max(t => t.Id) : 0;
        int nextId = (int)_nextIdField.GetValue(_form)!;
        if (nextId <= maxId) _nextIdField.SetValue(_form, maxId + 1);

        _refreshGrid.Invoke(_form, null);
        _lastSnapshot = new List<Track>(queue); // no retransmitir lo recién recibido
    }

    private void Post(Action action)
    {
        if (_form.IsDisposed || !_form.IsHandleCreated) return;
        try { _form.BeginInvoke(action); } catch { }
    }

    public void Dispose() => Stop();
}
