#nullable disable
namespace SoundCore.UI;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private Label lblAppTitle;
    private Button btnNetwork;

    // --- Panel de registro ---
    private GroupBox grpRegister;
    private Label lblTitle, lblArtist, lblBpm, lblDuration;
    private TextBox txtTitle, txtArtist;
    private NumericUpDown numBpm, numDuration;

    // --- Modo de colección ---
    private GroupBox grpMode;
    private RadioButton rbCustomList, rbLinkedList, rbListT;

    // --- Acciones de la cola ---
    private GroupBox grpActions;
    private Button btnAddToEnd, btnPlayNext, btnAdvanceTrack;
    private Button btnReverse, btnSortByBpm, btnRemoveDuplicates;

    // --- Lista en vivo ---
    private GroupBox grpPlaylist;
    private Label lblNowPlaying, lblTotalQueue, lblTotalTime;
    private DataGridView dgvPlaylist;

    // --- Benchmark ---
    private GroupBox grpBenchmark;
    private Button btnBenchmark;
    private Label lblBenchmarkInfo;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Método requerido por el Diseñador. Solo contiene asignaciones simples
    /// (sin foreach, variables locales ni llamadas a métodos propios) para que
    /// Visual Studio pueda abrir y mostrar el diseño sin errores.
    /// </summary>
    private void InitializeComponent()
    {
        lblAppTitle = new Label();
        btnNetwork = new Button();
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblArtist = new Label();
        txtArtist = new TextBox();
        lblBpm = new Label();
        numBpm = new NumericUpDown();
        lblDuration = new Label();
        numDuration = new NumericUpDown();
        grpRegister = new GroupBox();
        rbCustomList = new RadioButton();
        rbLinkedList = new RadioButton();
        rbListT = new RadioButton();
        grpMode = new GroupBox();
        btnAddToEnd = new Button();
        btnPlayNext = new Button();
        btnAdvanceTrack = new Button();
        btnReverse = new Button();
        btnSortByBpm = new Button();
        btnRemoveDuplicates = new Button();
        grpActions = new GroupBox();
        lblNowPlaying = new Label();
        dgvPlaylist = new DataGridView();
        lblTotalQueue = new Label();
        lblTotalTime = new Label();
        grpPlaylist = new GroupBox();
        btnBenchmark = new Button();
        lblBenchmarkInfo = new Label();
        grpBenchmark = new GroupBox();
        ((System.ComponentModel.ISupportInitialize)numBpm).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDuration).BeginInit();
        grpRegister.SuspendLayout();
        grpMode.SuspendLayout();
        grpActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPlaylist).BeginInit();
        grpPlaylist.SuspendLayout();
        grpBenchmark.SuspendLayout();
        SuspendLayout();
        // 
        // lblAppTitle
        // 
        lblAppTitle.AutoSize = true;
        lblAppTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblAppTitle.ForeColor = Color.FromArgb(94, 234, 212);
        lblAppTitle.Location = new Point(20, 15);
        lblAppTitle.Name = "lblAppTitle";
        lblAppTitle.Size = new Size(563, 32);
        lblAppTitle.TabIndex = 0;
        lblAppTitle.Text = "🎧  SoundCore Engine — Controlador de Set DJ";
        // 
        // btnNetwork
        // 
        btnNetwork.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNetwork.BackColor = Color.FromArgb(45, 49, 60);
        btnNetwork.FlatStyle = FlatStyle.Flat;
        btnNetwork.ForeColor = Color.FromArgb(226, 232, 240);
        btnNetwork.Location = new Point(990, 12);
        btnNetwork.Name = "btnNetwork";
        btnNetwork.Size = new Size(170, 32);
        btnNetwork.TabIndex = 1;
        btnNetwork.Text = "🌐 Conexión de Red";
        btnNetwork.UseVisualStyleBackColor = false;
        btnNetwork.Click += BtnNetwork_Click;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.ForeColor = Color.FromArgb(226, 232, 240);
        lblTitle.Location = new Point(15, 30);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(50, 20);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Título:";
        // 
        // txtTitle
        // 
        txtTitle.BackColor = Color.FromArgb(34, 37, 46);
        txtTitle.ForeColor = Color.FromArgb(226, 232, 240);
        txtTitle.Location = new Point(100, 27);
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(240, 27);
        txtTitle.TabIndex = 1;
        // 
        // lblArtist
        // 
        lblArtist.AutoSize = true;
        lblArtist.ForeColor = Color.FromArgb(226, 232, 240);
        lblArtist.Location = new Point(15, 62);
        lblArtist.Name = "lblArtist";
        lblArtist.Size = new Size(55, 20);
        lblArtist.TabIndex = 2;
        lblArtist.Text = "Artista:";
        // 
        // txtArtist
        // 
        txtArtist.BackColor = Color.FromArgb(34, 37, 46);
        txtArtist.ForeColor = Color.FromArgb(226, 232, 240);
        txtArtist.Location = new Point(100, 59);
        txtArtist.Name = "txtArtist";
        txtArtist.Size = new Size(240, 27);
        txtArtist.TabIndex = 3;
        // 
        // lblBpm
        // 
        lblBpm.AutoSize = true;
        lblBpm.ForeColor = Color.FromArgb(226, 232, 240);
        lblBpm.Location = new Point(15, 94);
        lblBpm.Name = "lblBpm";
        lblBpm.Size = new Size(42, 20);
        lblBpm.TabIndex = 4;
        lblBpm.Text = "BPM:";
        // 
        // numBpm
        // 
        numBpm.BackColor = Color.FromArgb(34, 37, 46);
        numBpm.ForeColor = Color.FromArgb(226, 232, 240);
        numBpm.Location = new Point(100, 91);
        numBpm.Maximum = new decimal(new int[] { 220, 0, 0, 0 });
        numBpm.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
        numBpm.Name = "numBpm";
        numBpm.Size = new Size(90, 27);
        numBpm.TabIndex = 5;
        numBpm.Value = new decimal(new int[] { 120, 0, 0, 0 });
        // 
        // lblDuration
        // 
        lblDuration.AutoSize = true;
        lblDuration.ForeColor = Color.FromArgb(226, 232, 240);
        lblDuration.Location = new Point(210, 94);
        lblDuration.Name = "lblDuration";
        lblDuration.Size = new Size(92, 20);
        lblDuration.TabIndex = 6;
        lblDuration.Text = "Duración (s):";
        // 
        // numDuration
        // 
        numDuration.BackColor = Color.FromArgb(34, 37, 46);
        numDuration.ForeColor = Color.FromArgb(226, 232, 240);
        numDuration.Location = new Point(300, 91);
        numDuration.Maximum = new decimal(new int[] { 900, 0, 0, 0 });
        numDuration.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
        numDuration.Name = "numDuration";
        numDuration.Size = new Size(54, 27);
        numDuration.TabIndex = 7;
        numDuration.Value = new decimal(new int[] { 180, 0, 0, 0 });
        // 
        // grpRegister
        // 
        grpRegister.Controls.Add(lblTitle);
        grpRegister.Controls.Add(txtTitle);
        grpRegister.Controls.Add(lblArtist);
        grpRegister.Controls.Add(txtArtist);
        grpRegister.Controls.Add(lblBpm);
        grpRegister.Controls.Add(numBpm);
        grpRegister.Controls.Add(lblDuration);
        grpRegister.Controls.Add(numDuration);
        grpRegister.ForeColor = Color.FromArgb(94, 234, 212);
        grpRegister.Location = new Point(20, 55);
        grpRegister.Name = "grpRegister";
        grpRegister.Size = new Size(360, 150);
        grpRegister.TabIndex = 2;
        grpRegister.TabStop = false;
        grpRegister.Text = "[ REGISTRO ]";
        // 
        // rbCustomList
        // 
        rbCustomList.AutoSize = true;
        rbCustomList.Checked = true;
        rbCustomList.ForeColor = Color.FromArgb(226, 232, 240);
        rbCustomList.Location = new Point(15, 25);
        rbCustomList.Name = "rbCustomList";
        rbCustomList.Size = new Size(327, 24);
        rbCustomList.TabIndex = 0;
        rbCustomList.TabStop = true;
        rbCustomList.Text = "Lista Enlazada Simple Personalizada (Nodos)";
        rbCustomList.CheckedChanged += ModeCheckedChanged;
        // 
        // rbLinkedList
        // 
        rbLinkedList.AutoSize = true;
        rbLinkedList.ForeColor = Color.FromArgb(226, 232, 240);
        rbLinkedList.Location = new Point(15, 52);
        rbLinkedList.Name = "rbLinkedList";
        rbLinkedList.Size = new Size(157, 24);
        rbLinkedList.TabIndex = 1;
        rbLinkedList.Text = ".NET LinkedList<T>";
        rbLinkedList.CheckedChanged += ModeCheckedChanged;
        // 
        // rbListT
        // 
        rbListT.AutoSize = true;
        rbListT.ForeColor = Color.FromArgb(226, 232, 240);
        rbListT.Location = new Point(15, 79);
        rbListT.Name = "rbListT";
        rbListT.Size = new Size(114, 24);
        rbListT.TabIndex = 2;
        rbListT.Text = ".NET List<T>";
        rbListT.CheckedChanged += ModeCheckedChanged;
        // 
        // grpMode
        // 
        grpMode.Controls.Add(rbCustomList);
        grpMode.Controls.Add(rbLinkedList);
        grpMode.Controls.Add(rbListT);
        grpMode.ForeColor = Color.FromArgb(94, 234, 212);
        grpMode.Location = new Point(20, 215);
        grpMode.Name = "grpMode";
        grpMode.Size = new Size(360, 110);
        grpMode.TabIndex = 3;
        grpMode.TabStop = false;
        grpMode.Text = "[ MODO DE COLECCIÓN ]";
        // 
        // btnAddToEnd
        // 
        btnAddToEnd.BackColor = Color.FromArgb(45, 49, 60);
        btnAddToEnd.FlatStyle = FlatStyle.Flat;
        btnAddToEnd.ForeColor = Color.FromArgb(226, 232, 240);
        btnAddToEnd.Location = new Point(15, 25);
        btnAddToEnd.Name = "btnAddToEnd";
        btnAddToEnd.Padding = new Padding(8, 0, 0, 0);
        btnAddToEnd.Size = new Size(325, 32);
        btnAddToEnd.TabIndex = 0;
        btnAddToEnd.Text = "➕ Agregar al Final";
        btnAddToEnd.TextAlign = ContentAlignment.MiddleLeft;
        btnAddToEnd.UseVisualStyleBackColor = false;
        btnAddToEnd.Click += BtnAddToEnd_Click;
        // 
        // btnPlayNext
        // 
        btnPlayNext.BackColor = Color.FromArgb(45, 49, 60);
        btnPlayNext.FlatStyle = FlatStyle.Flat;
        btnPlayNext.ForeColor = Color.FromArgb(226, 232, 240);
        btnPlayNext.Location = new Point(15, 65);
        btnPlayNext.Name = "btnPlayNext";
        btnPlayNext.Padding = new Padding(8, 0, 0, 0);
        btnPlayNext.Size = new Size(325, 32);
        btnPlayNext.TabIndex = 1;
        btnPlayNext.Text = "⏭ Reproducir Siguiente (Up Next)";
        btnPlayNext.TextAlign = ContentAlignment.MiddleLeft;
        btnPlayNext.UseVisualStyleBackColor = false;
        btnPlayNext.Click += BtnPlayNext_Click;
        // 
        // btnAdvanceTrack
        // 
        btnAdvanceTrack.BackColor = Color.FromArgb(45, 49, 60);
        btnAdvanceTrack.FlatStyle = FlatStyle.Flat;
        btnAdvanceTrack.ForeColor = Color.FromArgb(226, 232, 240);
        btnAdvanceTrack.Location = new Point(15, 105);
        btnAdvanceTrack.Name = "btnAdvanceTrack";
        btnAdvanceTrack.Padding = new Padding(8, 0, 0, 0);
        btnAdvanceTrack.Size = new Size(325, 32);
        btnAdvanceTrack.TabIndex = 2;
        btnAdvanceTrack.Text = "▶ Avanzar Canción";
        btnAdvanceTrack.TextAlign = ContentAlignment.MiddleLeft;
        btnAdvanceTrack.UseVisualStyleBackColor = false;
        btnAdvanceTrack.Click += BtnAdvanceTrack_Click;
        // 
        // btnReverse
        // 
        btnReverse.BackColor = Color.FromArgb(45, 49, 60);
        btnReverse.FlatStyle = FlatStyle.Flat;
        btnReverse.ForeColor = Color.FromArgb(226, 232, 240);
        btnReverse.Location = new Point(15, 145);
        btnReverse.Name = "btnReverse";
        btnReverse.Padding = new Padding(8, 0, 0, 0);
        btnReverse.Size = new Size(325, 32);
        btnReverse.TabIndex = 3;
        btnReverse.Text = "🔃 Invertir Lista (En el Lugar)";
        btnReverse.TextAlign = ContentAlignment.MiddleLeft;
        btnReverse.UseVisualStyleBackColor = false;
        btnReverse.Click += BtnReverse_Click;
        // 
        // btnSortByBpm
        // 
        btnSortByBpm.BackColor = Color.FromArgb(45, 49, 60);
        btnSortByBpm.FlatStyle = FlatStyle.Flat;
        btnSortByBpm.ForeColor = Color.FromArgb(226, 232, 240);
        btnSortByBpm.Location = new Point(15, 185);
        btnSortByBpm.Name = "btnSortByBpm";
        btnSortByBpm.Padding = new Padding(8, 0, 0, 0);
        btnSortByBpm.Size = new Size(325, 32);
        btnSortByBpm.TabIndex = 4;
        btnSortByBpm.Text = "⚡ Ordenar por Curva de BPM";
        btnSortByBpm.TextAlign = ContentAlignment.MiddleLeft;
        btnSortByBpm.UseVisualStyleBackColor = false;
        btnSortByBpm.Click += BtnSortByBpm_Click;
        // 
        // btnRemoveDuplicates
        // 
        btnRemoveDuplicates.BackColor = Color.FromArgb(45, 49, 60);
        btnRemoveDuplicates.FlatStyle = FlatStyle.Flat;
        btnRemoveDuplicates.ForeColor = Color.FromArgb(226, 232, 240);
        btnRemoveDuplicates.Location = new Point(15, 225);
        btnRemoveDuplicates.Name = "btnRemoveDuplicates";
        btnRemoveDuplicates.Padding = new Padding(8, 0, 0, 0);
        btnRemoveDuplicates.Size = new Size(325, 32);
        btnRemoveDuplicates.TabIndex = 5;
        btnRemoveDuplicates.Text = "\U0001f9f9 Quitar Duplicados";
        btnRemoveDuplicates.TextAlign = ContentAlignment.MiddleLeft;
        btnRemoveDuplicates.UseVisualStyleBackColor = false;
        btnRemoveDuplicates.Click += BtnRemoveDuplicates_Click;
        // 
        // grpActions
        // 
        grpActions.Controls.Add(btnAddToEnd);
        grpActions.Controls.Add(btnPlayNext);
        grpActions.Controls.Add(btnAdvanceTrack);
        grpActions.Controls.Add(btnReverse);
        grpActions.Controls.Add(btnSortByBpm);
        grpActions.Controls.Add(btnRemoveDuplicates);
        grpActions.ForeColor = Color.FromArgb(94, 234, 212);
        grpActions.Location = new Point(20, 335);
        grpActions.Name = "grpActions";
        grpActions.Size = new Size(360, 270);
        grpActions.TabIndex = 4;
        grpActions.TabStop = false;
        grpActions.Text = "[ ACCIONES DE LA COLA ]";
        // 
        // lblNowPlaying
        // 
        lblNowPlaying.AutoSize = true;
        lblNowPlaying.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblNowPlaying.ForeColor = Color.FromArgb(226, 232, 240);
        lblNowPlaying.Location = new Point(15, 28);
        lblNowPlaying.Name = "lblNowPlaying";
        lblNowPlaying.Size = new Size(252, 23);
        lblNowPlaying.TabIndex = 0;
        lblNowPlaying.Text = "▶ Reproduciendo: (cola vacía)";
        // 
        // dgvPlaylist
        // 
        dgvPlaylist.AllowUserToAddRows = false;
        dgvPlaylist.AllowUserToDeleteRows = false;
        dgvPlaylist.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPlaylist.BackgroundColor = Color.FromArgb(34, 37, 46);
        dgvPlaylist.ColumnHeadersHeight = 29;
        dgvPlaylist.Location = new Point(15, 55);
        dgvPlaylist.Name = "dgvPlaylist";
        dgvPlaylist.ReadOnly = true;
        dgvPlaylist.RowHeadersVisible = false;
        dgvPlaylist.RowHeadersWidth = 51;
        dgvPlaylist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPlaylist.Size = new Size(729, 860);
        dgvPlaylist.TabIndex = 0;
        // 
        // lblTotalQueue
        // 
        lblTotalQueue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotalQueue.AutoSize = true;
        lblTotalQueue.ForeColor = Color.FromArgb(226, 232, 240);
        lblTotalQueue.Location = new Point(15, 925);
        lblTotalQueue.Name = "lblTotalQueue";
        lblTotalQueue.Size = new Size(178, 20);
        lblTotalQueue.TabIndex = 1;
        lblTotalQueue.Text = "Total en cola: 0 canciones";
        // 
        // lblTotalTime
        // 
        lblTotalTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotalTime.AutoSize = true;
        lblTotalTime.ForeColor = Color.FromArgb(226, 232, 240);
        lblTotalTime.Location = new Point(220, 925);
        lblTotalTime.Name = "lblTotalTime";
        lblTotalTime.Size = new Size(129, 20);
        lblTotalTime.TabIndex = 2;
        lblTotalTime.Text = "Tiempo total: 0:00";
        // 
        // grpPlaylist
        // 
        grpPlaylist.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpPlaylist.Controls.Add(lblNowPlaying);
        grpPlaylist.Controls.Add(dgvPlaylist);
        grpPlaylist.Controls.Add(lblTotalQueue);
        grpPlaylist.Controls.Add(lblTotalTime);
        grpPlaylist.ForeColor = Color.FromArgb(94, 234, 212);
        grpPlaylist.Location = new Point(400, 55);
        grpPlaylist.Name = "grpPlaylist";
        grpPlaylist.Size = new Size(760, 540);
        grpPlaylist.TabIndex = 5;
        grpPlaylist.TabStop = false;
        grpPlaylist.Text = "[ LISTA EN VIVO ]";
        // 
        // btnBenchmark
        // 
        btnBenchmark.BackColor = Color.FromArgb(56, 189, 148);
        btnBenchmark.FlatStyle = FlatStyle.Flat;
        btnBenchmark.ForeColor = Color.Black;
        btnBenchmark.Location = new Point(15, 30);
        btnBenchmark.Name = "btnBenchmark";
        btnBenchmark.Size = new Size(230, 40);
        btnBenchmark.TabIndex = 0;
        btnBenchmark.Text = "🚀 Ejecutar 25,000 Inserciones";
        btnBenchmark.UseVisualStyleBackColor = false;
        btnBenchmark.Click += BtnBenchmark_Click;
        // 
        // lblBenchmarkInfo
        // 
        lblBenchmarkInfo.AutoSize = true;
        lblBenchmarkInfo.ForeColor = Color.FromArgb(226, 232, 240);
        lblBenchmarkInfo.Location = new Point(260, 25);
        lblBenchmarkInfo.Name = "lblBenchmarkInfo";
        lblBenchmarkInfo.Size = new Size(472, 40);
        lblBenchmarkInfo.TabIndex = 1;
        lblBenchmarkInfo.Text = "Mide con Stopwatch el impacto de 25,000 inserciones a mitad de lista\ny justifica la curva de rendimiento de cada colección.";
        // 
        // grpBenchmark
        // 
        grpBenchmark.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpBenchmark.Controls.Add(btnBenchmark);
        grpBenchmark.Controls.Add(lblBenchmarkInfo);
        grpBenchmark.ForeColor = Color.FromArgb(94, 234, 212);
        grpBenchmark.Location = new Point(400, 605);
        grpBenchmark.Name = "grpBenchmark";
        grpBenchmark.Size = new Size(760, 100);
        grpBenchmark.TabIndex = 6;
        grpBenchmark.TabStop = false;
        grpBenchmark.Text = "[ TELEMETRÍA DE BENCHMARK ]";
        // 
        // MainForm
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(24, 26, 32);
        ClientSize = new Size(1180, 760);
        Controls.Add(lblAppTitle);
        Controls.Add(btnNetwork);
        Controls.Add(grpRegister);
        Controls.Add(grpMode);
        Controls.Add(grpActions);
        Controls.Add(grpPlaylist);
        Controls.Add(grpBenchmark);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(226, 232, 240);
        MinimumSize = new Size(1000, 650);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SoundCore Engine v2.0 - Controlador de Set DJ [TecNM Monclova]";
        ((System.ComponentModel.ISupportInitialize)numBpm).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDuration).EndInit();
        grpRegister.ResumeLayout(false);
        grpRegister.PerformLayout();
        grpMode.ResumeLayout(false);
        grpMode.PerformLayout();
        grpActions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPlaylist).EndInit();
        grpPlaylist.ResumeLayout(false);
        grpPlaylist.PerformLayout();
        grpBenchmark.ResumeLayout(false);
        grpBenchmark.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
