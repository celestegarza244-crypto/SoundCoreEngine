using System.Net;
using System.Net.Sockets;
using SoundCore.UI;

namespace SoundCore.Networking;

/// <summary>Ventana extra (como PlayerForm) para conectar varias computadoras.</summary>
public sealed class NetworkForm : Form
{
    private readonly NetworkSync _sync;
    private readonly RadioButton _rbHost = new() { Text = "Ser anfitrión (servidor)", Checked = true, AutoSize = true, Location = new Point(15, 15) };
    private readonly RadioButton _rbClient = new() { Text = "Unirme a otra PC (cliente)", AutoSize = true, Location = new Point(15, 40) };
    private readonly Label _lblIp = new() { Text = "IP del anfitrión:", AutoSize = true, Location = new Point(15, 73) };
    private readonly TextBox _txtIp = new() { Text = "192.168.1.", Location = new Point(120, 70), Width = 140, Enabled = false, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
    private readonly Label _lblPort = new() { Text = "Puerto:", AutoSize = true, Location = new Point(275, 73), Anchor = AnchorStyles.Top | AnchorStyles.Right };
    private readonly NumericUpDown _numPort = new() { Minimum = 1024, Maximum = 65535, Value = 5050, Location = new Point(325, 70), Width = 70, Anchor = AnchorStyles.Top | AnchorStyles.Right };
    private readonly Button _btn = new() { Text = "Conectar", Location = new Point(15, 105), Width = 380, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
    private readonly Label _lblStatus = new() { Text = "Sin conexión", AutoSize = true, Location = new Point(15, 148), ForeColor = Color.DimGray };
    private readonly Label _lblIps = new() { AutoSize = true, Location = new Point(15, 170) };
    private readonly ListBox _log = new() { Location = new Point(15, 210), Size = new Size(380, 130), IntegralHeight = false, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };

    public NetworkForm(MainForm main)
    {
        _sync = new NetworkSync(main);

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;

        Text = "SoundCore — Red";
        ClientSize = new Size(410, 355);
        FormBorderStyle = FormBorderStyle.Sizable;   // redimensionable
        MaximizeBox = true;
        MinimumSize = SizeFromClientSize(new Size(340, 300));
        StartPosition = FormStartPosition.Manual;
        Location = new Point(main.Left + main.Width + 10, main.Top);

        Controls.AddRange(new Control[] { _rbHost, _rbClient, _lblIp, _txtIp, _lblPort, _numPort, _btn, _lblStatus, _lblIps, _log });
        _lblIps.Text = "Mi(s) IP: " + string.Join(", ", GetLocalIps());

        _rbClient.CheckedChanged += (_, _) => _txtIp.Enabled = _rbClient.Checked;
        _btn.Click += async (_, _) => await ToggleAsync();
        _sync.Log += msg => { _log.Items.Add($"[{DateTime.Now:HH:mm:ss}] {msg}"); _log.TopIndex = _log.Items.Count - 1; };
        _sync.PeersChanged += UpdateStatus;

        FormClosing += (_, e) =>
        {
            if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); return; }
            _sync.Dispose();
        };
    }

    private async Task ToggleAsync()
    {
        if (_sync.IsRunning) { _sync.Stop(); SetIdle(); return; }

        _btn.Enabled = false;
        try
        {
            int port = (int)_numPort.Value;
            if (_rbHost.Checked) _sync.StartHost(port);
            else await _sync.StartClientAsync(_txtIp.Text.Trim(), port);

            _btn.Text = "Desconectar";
            _rbHost.Enabled = _rbClient.Enabled = _txtIp.Enabled = _numPort.Enabled = false;
            UpdateStatus();
        }
        catch (Exception ex)
        {
            _sync.Stop();
            SetIdle();
            MessageBox.Show(this, "No se pudo conectar:\n" + ex.Message, "Red",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { _btn.Enabled = true; }
    }

    private void SetIdle()
    {
        _btn.Text = "Conectar";
        _rbHost.Enabled = _rbClient.Enabled = _numPort.Enabled = true;
        _txtIp.Enabled = _rbClient.Checked;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (!_sync.IsRunning) { _lblStatus.Text = "Sin conexión"; _lblStatus.ForeColor = Color.DimGray; return; }
        _lblStatus.ForeColor = Color.SeaGreen;
        _lblStatus.Text = _sync.IsHost
            ? $"Anfitrión activo — {_sync.PeerCount} equipo(s) conectado(s)"
            : (_sync.PeerCount > 0 ? "Conectado al anfitrión" : "Desconectado del anfitrión");
        if (!_sync.IsHost && _sync.PeerCount == 0) _lblStatus.ForeColor = Color.Firebrick;
    }

    private static IEnumerable<string> GetLocalIps() =>
        Dns.GetHostAddresses(Dns.GetHostName())
           .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
           .Select(a => a.ToString());
}
