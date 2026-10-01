using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SoundCore.UI;

/// <summary>
/// Reproductor DJ "G". Es una ventana independiente que se abre junto con
/// el programa principal. Usa Windows Media Player (COM, enlace tardío), por
/// lo que no requiere paquetes NuGet ni cambios en el proyecto.
/// Reproduce mp3, wav, wma, m4a, aac, etc.
/// </summary>
public partial class PlayerForm : Form
{
    // ---- Color del logo "G" (el resto de la paleta está en el Designer) ----
    private static readonly Color AccentColor = Color.FromArgb(94, 234, 212);

    // ---- Motor de audio ----
    private dynamic _wmp = null!;
    private bool _ready;

    // ---- Estado ----
    private readonly List<string> _files = new();
    private int _index = -1;
    private bool _wantPlaying;
    private bool _hasStarted;
    private bool _seeking;

    public PlayerForm()
    {
        // Todo el diseño (controles, colores, posiciones) vive en
        // PlayerForm.Designer.cs -> InitializeComponent().
        InitializeComponent();

        // Visual Studio ejecuta este constructor al abrir el diseñador. En ese
        // modo no se arranca el audio, el temporizador ni se tocan propiedades
        // del formulario (para que el Diseñador no las guarde por error).
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            return;

        // Lo que el Diseñador no puede representar se configura aquí.
        Icon = CreateGIcon();
        MinimumSize = SizeFromClientSize(new Size(340, 600));

        var wa = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1280, 720);
        StartPosition = FormStartPosition.Manual;
        Location = new Point(Math.Max(wa.Left, wa.Right - Width - 20), wa.Top + 40);

        LayoutTransportButtons();
        tmr.Start();

        InitEngine();
    }

    // =================================================================
    // Eventos de la interfaz (conectados desde el Designer)
    // =================================================================
    private void BtnPrev_Click(object? sender, EventArgs e) => Skip(-1, wrap: true);
    private void BtnPlay_Click(object? sender, EventArgs e) => TogglePlay();
    private void BtnStop_Click(object? sender, EventArgs e) => StopPlayback();
    private void BtnNext_Click(object? sender, EventArgs e) => Skip(+1, wrap: true);
    private void BtnOpen_Click(object? sender, EventArgs e) => OpenFiles();
    private void BtnClear_Click(object? sender, EventArgs e) => ClearList();
    private void BtnResetTempo_Click(object? sender, EventArgs e) => trkTempo.Value = 100;

    private void TrkSeek_MouseDown(object? sender, MouseEventArgs e) => _seeking = true;

    private void TrkVolume_ValueChanged(object? sender, EventArgs e)
    {
        lblVolume.Text = $"🔊 Volumen: {trkVolume.Value}%";
        if (_ready) _wmp.settings.volume = trkVolume.Value;
    }

    private void TrkTempo_ValueChanged(object? sender, EventArgs e)
    {
        ApplyRate();

        int state = 0;
        if (_ready)
        {
            try { state = (int)_wmp.playState; } catch { /* sin medio */ }
        }
        RefreshTempoLabel(state);
    }

    private void LstTracks_DoubleClick(object? sender, EventArgs e)
    {
        if (lstTracks.SelectedIndex >= 0) PlayIndex(lstTracks.SelectedIndex);
    }

    private void PlayerForm_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true) e.Effect = DragDropEffects.Copy;
    }

    private void PlayerForm_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] dropped) AddFiles(dropped);
    }

    // Los 5 botones de transporte se reparten a lo ancho de la ventana.
    private void PlayerForm_Resize(object? sender, EventArgs e) => LayoutTransportButtons();

    /// <summary>Reparte los botones ⏮ ▶ ⏹ ⏭ 📂 en todo el ancho disponible.</summary>
    private void LayoutTransportButtons()
    {
        if (btnPrev is null || btnOpen is null) return;

        var buttons = new[] { btnPrev, btnPlay, btnStop, btnNext, btnOpen };
        const int margin = 20, gap = 10;
        int w = Math.Max(40, (ClientSize.Width - 2 * margin - gap * (buttons.Length - 1)) / buttons.Length);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Width = w;
            buttons[i].Left = margin + i * (w + gap);
        }
    }

    private static void DrawG(Graphics g, Rectangle bounds, float fontSize)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        using var circle = new SolidBrush(AccentColor);
        g.FillEllipse(circle, bounds);

        using var font = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("G", font, Brushes.Black, bounds, sf);
    }

    private void PnlLogo_Paint(object? sender, PaintEventArgs e) =>
        DrawG(e.Graphics, new Rectangle(1, 1, pnlLogo.Width - 3, pnlLogo.Height - 3), 46F);

    private static Icon CreateGIcon()
    {
        using var bmp = new Bitmap(64, 64);
        using (var g = Graphics.FromImage(bmp))
            DrawG(g, new Rectangle(1, 1, 61, 61), 40F);
        return Icon.FromHandle(bmp.GetHicon());
    }

    // =================================================================
    // Motor de audio (Windows Media Player, enlace tardío)
    // =================================================================
    private void InitEngine()
    {
        try
        {
            var type = Type.GetTypeFromProgID("WMPlayer.OCX.7")
                       ?? throw new InvalidOperationException("Windows Media Player no está disponible.");
            _wmp = Activator.CreateInstance(type)!;
            _wmp.settings.autoStart = false;
            _wmp.settings.volume = trkVolume.Value;
            _ready = true;
        }
        catch (Exception)
        {
            _ready = false;
            foreach (var b in new[] { btnPrev, btnPlay, btnStop, btnNext, btnOpen })
                b.Enabled = false;

            Shown += (_, _) => MessageBox.Show(this,
                "No se pudo iniciar el motor de audio (Windows Media Player).\n" +
                "Verifica que Windows Media Player esté instalado y habilitado en Windows.",
                "G Player", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ---- Tempo (velocidad de reproducción) ----
    // Windows Media Player puede ignorar o reiniciar la velocidad mientras abre
    // una canción. Por eso se verifica leyendo el valor de vuelta y se vuelve a
    // aplicar mientras la canción suena (ver EnsureRate / Tmr_Tick).
    private bool _rateApplied = true;

    private void ApplyRate()
    {
        if (!_ready) return;

        double wanted = trkTempo.Value / 100.0;
        try
        {
            _wmp.settings.rate = wanted;
            double actual = _wmp.settings.rate;
            _rateApplied = Math.Abs(actual - wanted) < 0.01;
        }
        catch
        {
            _rateApplied = false;   // todavía no hay medio cargado
        }
    }

    /// <summary>Si la canción suena con otra velocidad a la pedida, la corrige.</summary>
    private void EnsureRate()
    {
        double wanted = trkTempo.Value / 100.0;
        try
        {
            double current = _wmp.settings.rate;
            if (Math.Abs(current - wanted) > 0.01) ApplyRate();
            else _rateApplied = true;
        }
        catch
        {
            _rateApplied = false;
        }
    }

    /// <summary>✔ = velocidad aplicada, ⚠ = el motor no la aceptó (solo mientras suena).</summary>
    private void RefreshTempoLabel(int state)
    {
        string mark = state == 3 ? (_rateApplied ? " ✔" : " ⚠") : "";
        lblTempo.Text = $"🎚 Tempo: {trkTempo.Value}%{mark}";
    }

    // =================================================================
    // Lista y reproducción
    // =================================================================
    private void OpenFiles()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Selecciona las canciones",
            Multiselect = true,
            Filter = "Audio (*.mp3;*.wav;*.wma;*.m4a;*.aac;*.flac;*.ogg)|*.mp3;*.wav;*.wma;*.m4a;*.aac;*.flac;*.ogg|Todos los archivos (*.*)|*.*"
        };

        if (dlg.ShowDialog(this) == DialogResult.OK)
            AddFiles(dlg.FileNames);
    }

    private void AddFiles(IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            if (!File.Exists(path)) continue;
            _files.Add(path);
            lstTracks.Items.Add(Path.GetFileNameWithoutExtension(path));
        }

        if (_index < 0 && _files.Count > 0)
        {
            _index = 0;
            lstTracks.SelectedIndex = 0;
            lblTrack.Text = Path.GetFileNameWithoutExtension(_files[0]);
        }
    }

    private void ClearList()
    {
        StopPlayback();
        _files.Clear();
        lstTracks.Items.Clear();
        _index = -1;
        lblTrack.Text = "Sin canción cargada";
    }

    private void PlayIndex(int index)
    {
        if (!_ready || index < 0 || index >= _files.Count) return;

        _index = index;
        _hasStarted = false;
        _wantPlaying = true;

        lstTracks.SelectedIndex = index;
        lblTrack.Text = Path.GetFileNameWithoutExtension(_files[index]);
        btnPlay.Text = "⏸";

        try
        {
            _rateApplied = false;   // se reaplica cuando la canción empiece a sonar
            _wmp.URL = _files[index];
            _wmp.controls.play();
            ApplyRate();
        }
        catch (Exception)
        {
            MessageBox.Show(this, "No se pudo reproducir este archivo.", "G Player",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            StopPlayback();
        }
    }

    private void TogglePlay()
    {
        if (!_ready) return;

        if (_files.Count == 0)
        {
            OpenFiles();
            if (_files.Count == 0) return;
        }

        int state = (int)_wmp.playState; // 3 = reproduciendo, 2 = en pausa

        if (state == 3)
        {
            _wmp.controls.pause();
            _wantPlaying = false;
            btnPlay.Text = "▶";
        }
        else if (state == 2)
        {
            _wmp.controls.play();
            _wantPlaying = true;
            btnPlay.Text = "⏸";
        }
        else
        {
            PlayIndex(_index >= 0 ? _index : 0);
        }
    }

    private void StopPlayback()
    {
        _wantPlaying = false;
        _hasStarted = false;
        btnPlay.Text = "▶";

        if (_ready)
        {
            try { _wmp.controls.stop(); } catch { /* nada que detener */ }
        }

        trkSeek.Value = 0;
        lblTime.Text = "00:00 / 00:00";
    }

    private void Skip(int step, bool wrap)
    {
        if (_files.Count == 0) return;

        int next = (_index < 0 ? 0 : _index) + step;

        if (next >= _files.Count)
        {
            if (!wrap) { StopPlayback(); return; }
            next = 0;
        }
        else if (next < 0)
        {
            next = _files.Count - 1;
        }

        PlayIndex(next);
    }

    // =================================================================
    // Barra de progreso y avance automático
    // =================================================================
    private void TrkSeek_MouseUp(object? sender, MouseEventArgs e)
    {
        try
        {
            if (_ready && _wmp.currentMedia != null)
            {
                double duration = _wmp.currentMedia.duration;
                if (duration > 0)
                    _wmp.controls.currentPosition = duration * trkSeek.Value / trkSeek.Maximum;
            }
        }
        catch { /* sin medio cargado */ }
        finally
        {
            _seeking = false;
        }
    }

    private void Tmr_Tick(object? sender, EventArgs e)
    {
        if (!_ready) return;

        try
        {
            int state = (int)_wmp.playState;

            if (state == 3)
            {
                _hasStarted = true;
                EnsureRate();   // reaplica el tempo si el motor lo reinició
            }
            RefreshTempoLabel(state);

            // Fin de canción: pasa a la siguiente (sin dar la vuelta a la lista)
            if (_wantPlaying && _hasStarted && (state == 1 || state == 8))
            {
                Skip(+1, wrap: false);
                return;
            }

            if (_wmp.currentMedia == null) return;

            double duration = _wmp.currentMedia.duration;
            double position = _wmp.controls.currentPosition;

            if (duration > 0 && !_seeking)
            {
                int value = (int)Math.Clamp(position / duration * trkSeek.Maximum, 0, trkSeek.Maximum);
                trkSeek.Value = value;
            }

            lblTime.Text = $"{FormatTime(position)} / {FormatTime(duration)}";
        }
        catch { /* el motor aún no está listo */ }
    }

    private static string FormatTime(double seconds)
    {
        if (double.IsNaN(seconds) || seconds < 0) seconds = 0;
        var ts = TimeSpan.FromSeconds(seconds);
        return $"{(int)ts.TotalMinutes:D2}:{ts.Seconds:D2}";
    }

    // =================================================================
    // Limpieza
    // =================================================================
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        tmr.Stop();

        if (_ready)
        {
            try
            {
                _wmp.controls.stop();
                _wmp.close();
                Marshal.FinalReleaseComObject((object)_wmp);
            }
            catch { /* ya liberado */ }
            _ready = false;
        }

        base.OnFormClosed(e);
    }
}
