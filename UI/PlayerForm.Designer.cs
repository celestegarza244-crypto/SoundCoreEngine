#nullable disable
namespace SoundCore.UI;

partial class PlayerForm
{
    private System.ComponentModel.IContainer components = null;

    // ---- Controles ----
    private Panel pnlLogo;
    private Label lblBrand, lblSub, lblTrack, lblTime;
    private Label lblVolume, lblTempo, lblList;
    private TrackBar trkSeek, trkVolume, trkTempo;
    private Button btnPrev, btnPlay, btnStop, btnNext, btnOpen;
    private Button btnResetTempo, btnClear;
    private ListBox lstTracks;
    private System.Windows.Forms.Timer tmr;
    private ToolTip tip;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Método requerido por el Diseñador. Solo contiene asignaciones simples
    /// (sin foreach, lambdas, variables locales ni llamadas a métodos propios)
    /// para que Visual Studio pueda mostrar el diseño sin errores.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tip = new ToolTip(components);
        btnPrev = new Button();
        btnPlay = new Button();
        btnStop = new Button();
        btnNext = new Button();
        btnOpen = new Button();
        tmr = new System.Windows.Forms.Timer(components);
        pnlLogo = new Panel();
        lblBrand = new Label();
        lblSub = new Label();
        lblTrack = new Label();
        trkSeek = new TrackBar();
        lblTime = new Label();
        lblVolume = new Label();
        trkVolume = new TrackBar();
        lblTempo = new Label();
        btnResetTempo = new Button();
        trkTempo = new TrackBar();
        lblList = new Label();
        lstTracks = new ListBox();
        btnClear = new Button();
        ((System.ComponentModel.ISupportInitialize)trkSeek).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trkVolume).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trkTempo).BeginInit();
        SuspendLayout();
        // 
        // btnPrev
        // 
        btnPrev.BackColor = Color.FromArgb(45, 49, 60);
        btnPrev.FlatStyle = FlatStyle.Flat;
        btnPrev.Font = new Font("Segoe UI Symbol", 14F);
        btnPrev.ForeColor = Color.FromArgb(226, 232, 240);
        btnPrev.Location = new Point(25, 265);
        btnPrev.Margin = new Padding(4, 4, 4, 4);
        btnPrev.Name = "btnPrev";
        btnPrev.Size = new Size(90, 58);
        btnPrev.TabIndex = 6;
        btnPrev.Text = "⏮";
        tip.SetToolTip(btnPrev, "Canción anterior");
        btnPrev.UseVisualStyleBackColor = false;
        btnPrev.Click += BtnPrev_Click;
        // 
        // btnPlay
        // 
        btnPlay.BackColor = Color.FromArgb(56, 189, 148);
        btnPlay.FlatStyle = FlatStyle.Flat;
        btnPlay.Font = new Font("Segoe UI Symbol", 14F);
        btnPlay.ForeColor = Color.Black;
        btnPlay.Location = new Point(128, 265);
        btnPlay.Margin = new Padding(4, 4, 4, 4);
        btnPlay.Name = "btnPlay";
        btnPlay.Size = new Size(90, 58);
        btnPlay.TabIndex = 7;
        btnPlay.Text = "▶";
        tip.SetToolTip(btnPlay, "Reproducir / Pausa");
        btnPlay.UseVisualStyleBackColor = false;
        btnPlay.Click += BtnPlay_Click;
        // 
        // btnStop
        // 
        btnStop.BackColor = Color.FromArgb(45, 49, 60);
        btnStop.FlatStyle = FlatStyle.Flat;
        btnStop.Font = new Font("Segoe UI Symbol", 14F);
        btnStop.ForeColor = Color.FromArgb(226, 232, 240);
        btnStop.Location = new Point(230, 265);
        btnStop.Margin = new Padding(4, 4, 4, 4);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(90, 58);
        btnStop.TabIndex = 8;
        btnStop.Text = "⏹";
        tip.SetToolTip(btnStop, "Detener");
        btnStop.UseVisualStyleBackColor = false;
        btnStop.Click += BtnStop_Click;
        // 
        // btnNext
        // 
        btnNext.BackColor = Color.FromArgb(45, 49, 60);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Segoe UI Symbol", 14F);
        btnNext.ForeColor = Color.FromArgb(226, 232, 240);
        btnNext.Location = new Point(332, 265);
        btnNext.Margin = new Padding(4, 4, 4, 4);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(90, 58);
        btnNext.TabIndex = 9;
        btnNext.Text = "⏭";
        tip.SetToolTip(btnNext, "Canción siguiente");
        btnNext.UseVisualStyleBackColor = false;
        btnNext.Click += BtnNext_Click;
        // 
        // btnOpen
        // 
        btnOpen.BackColor = Color.FromArgb(45, 49, 60);
        btnOpen.FlatStyle = FlatStyle.Flat;
        btnOpen.Font = new Font("Segoe UI Symbol", 14F);
        btnOpen.ForeColor = Color.FromArgb(226, 232, 240);
        btnOpen.Location = new Point(435, 265);
        btnOpen.Margin = new Padding(4, 4, 4, 4);
        btnOpen.Name = "btnOpen";
        btnOpen.Size = new Size(90, 58);
        btnOpen.TabIndex = 10;
        btnOpen.Text = "📂";
        tip.SetToolTip(btnOpen, "Abrir canciones");
        btnOpen.UseVisualStyleBackColor = false;
        btnOpen.Click += BtnOpen_Click;
        // 
        // tmr
        // 
        tmr.Interval = 250;
        tmr.Tick += Tmr_Tick;
        // 
        // pnlLogo
        // 
        pnlLogo.BackColor = Color.FromArgb(24, 26, 32);
        pnlLogo.Location = new Point(25, 19);
        pnlLogo.Margin = new Padding(4, 4, 4, 4);
        pnlLogo.Name = "pnlLogo";
        pnlLogo.Size = new Size(90, 90);
        pnlLogo.TabIndex = 0;
        pnlLogo.Paint += PnlLogo_Paint;
        // 
        // lblBrand
        // 
        lblBrand.AutoSize = true;
        lblBrand.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblBrand.ForeColor = Color.FromArgb(94, 234, 212);
        lblBrand.Location = new Point(128, 22);
        lblBrand.Margin = new Padding(4, 0, 4, 0);
        lblBrand.Name = "lblBrand";
        lblBrand.Size = new Size(154, 41);
        lblBrand.TabIndex = 1;
        lblBrand.Text = "G PLAYER";
        // 
        // lblSub
        // 
        lblSub.AutoSize = true;
        lblSub.ForeColor = Color.FromArgb(226, 232, 240);
        lblSub.Location = new Point(131, 70);
        lblSub.Margin = new Padding(4, 0, 4, 0);
        lblSub.Name = "lblSub";
        lblSub.Size = new Size(112, 20);
        lblSub.TabIndex = 2;
        lblSub.Text = "Reproductor DJ";
        // 
        // lblTrack
        // 
        lblTrack.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblTrack.AutoEllipsis = true;
        lblTrack.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTrack.ForeColor = Color.FromArgb(226, 232, 240);
        lblTrack.Location = new Point(25, 131);
        lblTrack.Margin = new Padding(4, 0, 4, 0);
        lblTrack.Name = "lblTrack";
        lblTrack.Size = new Size(500, 30);
        lblTrack.TabIndex = 3;
        lblTrack.Text = "Sin canción cargada";
        // 
        // trkSeek
        // 
        trkSeek.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trkSeek.BackColor = Color.FromArgb(24, 26, 32);
        trkSeek.Location = new Point(12, 165);
        trkSeek.Margin = new Padding(4, 4, 4, 4);
        trkSeek.Maximum = 1000;
        trkSeek.Name = "trkSeek";
        trkSeek.Size = new Size(525, 56);
        trkSeek.TabIndex = 4;
        trkSeek.TickStyle = TickStyle.None;
        trkSeek.MouseDown += TrkSeek_MouseDown;
        trkSeek.MouseUp += TrkSeek_MouseUp;
        // 
        // lblTime
        // 
        lblTime.AutoSize = true;
        lblTime.ForeColor = Color.FromArgb(94, 234, 212);
        lblTime.Location = new Point(25, 230);
        lblTime.Margin = new Padding(4, 0, 4, 0);
        lblTime.Name = "lblTime";
        lblTime.Size = new Size(93, 20);
        lblTime.TabIndex = 5;
        lblTime.Text = "00:00 / 00:00";
        // 
        // lblVolume
        // 
        lblVolume.AutoSize = true;
        lblVolume.ForeColor = Color.FromArgb(226, 232, 240);
        lblVolume.Location = new Point(25, 345);
        lblVolume.Margin = new Padding(4, 0, 4, 0);
        lblVolume.Name = "lblVolume";
        lblVolume.Size = new Size(127, 20);
        lblVolume.TabIndex = 11;
        lblVolume.Text = "🔊 Volumen: 80%";
        // 
        // trkVolume
        // 
        trkVolume.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trkVolume.BackColor = Color.FromArgb(24, 26, 32);
        trkVolume.Location = new Point(12, 372);
        trkVolume.Margin = new Padding(4, 4, 4, 4);
        trkVolume.Maximum = 100;
        trkVolume.Name = "trkVolume";
        trkVolume.Size = new Size(525, 56);
        trkVolume.TabIndex = 12;
        trkVolume.TickFrequency = 10;
        trkVolume.Value = 80;
        trkVolume.ValueChanged += TrkVolume_ValueChanged;
        // 
        // lblTempo
        // 
        lblTempo.AutoSize = true;
        lblTempo.ForeColor = Color.FromArgb(226, 232, 240);
        lblTempo.Location = new Point(25, 445);
        lblTempo.Margin = new Padding(4, 0, 4, 0);
        lblTempo.Name = "lblTempo";
        lblTempo.Size = new Size(128, 20);
        lblTempo.TabIndex = 13;
        lblTempo.Text = "🎚 Tiempo: 100%";
        // 
        // btnResetTempo
        // 
        btnResetTempo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnResetTempo.BackColor = Color.FromArgb(45, 49, 60);
        btnResetTempo.FlatStyle = FlatStyle.Flat;
        btnResetTempo.ForeColor = Color.FromArgb(226, 232, 240);
        btnResetTempo.Location = new Point(400, 439);
        btnResetTempo.Margin = new Padding(4, 4, 4, 4);
        btnResetTempo.Name = "btnResetTempo";
        btnResetTempo.Size = new Size(125, 32);
        btnResetTempo.TabIndex = 14;
        btnResetTempo.Text = "↺ Restablecer";
        btnResetTempo.UseVisualStyleBackColor = false;
        btnResetTempo.Click += BtnResetTempo_Click;
        // 
        // trkTempo
        // 
        trkTempo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        trkTempo.BackColor = Color.FromArgb(24, 26, 32);
        trkTempo.Location = new Point(12, 475);
        trkTempo.Margin = new Padding(4, 4, 4, 4);
        trkTempo.Maximum = 150;
        trkTempo.Minimum = 50;
        trkTempo.Name = "trkTempo";
        trkTempo.Size = new Size(525, 56);
        trkTempo.TabIndex = 15;
        trkTempo.TickFrequency = 10;
        trkTempo.Value = 100;
        trkTempo.ValueChanged += TrkTempo_ValueChanged;
        // 
        // lblList
        // 
        lblList.AutoSize = true;
        lblList.ForeColor = Color.FromArgb(94, 234, 212);
        lblList.Location = new Point(25, 545);
        lblList.Margin = new Padding(4, 0, 4, 0);
        lblList.Name = "lblList";
        lblList.Size = new Size(200, 20);
        lblList.TabIndex = 16;
        lblList.Text = "[ LISTA DE REPRODUCCIÓN ]";
        // 
        // lstTracks
        // 
        lstTracks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstTracks.BackColor = Color.FromArgb(34, 37, 46);
        lstTracks.BorderStyle = BorderStyle.FixedSingle;
        lstTracks.ForeColor = Color.FromArgb(226, 232, 240);
        lstTracks.IntegralHeight = false;
        lstTracks.Location = new Point(25, 575);
        lstTracks.Margin = new Padding(4, 4, 4, 4);
        lstTracks.Name = "lstTracks";
        lstTracks.Size = new Size(500, 137);
        lstTracks.TabIndex = 17;
        lstTracks.DoubleClick += LstTracks_DoubleClick;
        // 
        // btnClear
        // 
        btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        btnClear.BackColor = Color.FromArgb(45, 49, 60);
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.ForeColor = Color.FromArgb(226, 232, 240);
        btnClear.Location = new Point(25, 722);
        btnClear.Margin = new Padding(4, 4, 4, 4);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(500, 38);
        btnClear.TabIndex = 18;
        btnClear.Text = "🗑 Limpiar lista";
        btnClear.UseVisualStyleBackColor = false;
        btnClear.Click += BtnClear_Click;
        // 
        // PlayerForm
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(24, 26, 32);
        ClientSize = new Size(550, 775);
        Controls.Add(lblTime);
        Controls.Add(lblVolume);
        Controls.Add(lblTempo);
        Controls.Add(btnResetTempo);
        Controls.Add(lblList);
        Controls.Add(pnlLogo);
        Controls.Add(lblBrand);
        Controls.Add(lblSub);
        Controls.Add(lblTrack);
        Controls.Add(trkSeek);
        Controls.Add(btnPrev);
        Controls.Add(btnPlay);
        Controls.Add(btnStop);
        Controls.Add(btnNext);
        Controls.Add(btnOpen);
        Controls.Add(trkVolume);
        Controls.Add(trkTempo);
        Controls.Add(lstTracks);
        Controls.Add(btnClear);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(226, 232, 240);
        Margin = new Padding(4, 4, 4, 4);
        Name = "PlayerForm";
        Text = "G Player — Reproductor DJ";
        DragDrop += PlayerForm_DragDrop;
        DragEnter += PlayerForm_DragEnter;
        Resize += PlayerForm_Resize;
        ((System.ComponentModel.ISupportInitialize)trkSeek).EndInit();
        ((System.ComponentModel.ISupportInitialize)trkVolume).EndInit();
        ((System.ComponentModel.ISupportInitialize)trkTempo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
