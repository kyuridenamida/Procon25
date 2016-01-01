namespace ProconVisualizer
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOpenProblem = new System.Windows.Forms.Button();
            this.buttonParserSelect = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.buttonSolverASelect = new System.Windows.Forms.Button();
            this.buttonRunSolverB = new System.Windows.Forms.Button();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.buttonParse = new System.Windows.Forms.Button();
            this.trackBarResult = new System.Windows.Forms.TrackBar();
            this.textBoxProblemFile = new System.Windows.Forms.TextBox();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSolverPathD = new System.Windows.Forms.TextBox();
            this.buttonSolverDSelect = new System.Windows.Forms.Button();
            this.textBoxSolverPathC = new System.Windows.Forms.TextBox();
            this.buttonSolverCSelect = new System.Windows.Forms.Button();
            this.textBoxSolverPathB = new System.Windows.Forms.TextBox();
            this.buttonSolverBSelect = new System.Windows.Forms.Button();
            this.textBoxSolverPathA = new System.Windows.Forms.TextBox();
            this.textBoxParserPath = new System.Windows.Forms.TextBox();
            this.buttonHTTPGET = new System.Windows.Forms.Button();
            this.checkBoxGrid = new System.Windows.Forms.CheckBox();
            this.checkBoxNumber = new System.Windows.Forms.CheckBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxForPractice = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoRetry = new System.Windows.Forms.CheckBox();
            this.maskedtextBoxProblemID = new System.Windows.Forms.MaskedTextBox();
            this.labelPOSTURL = new System.Windows.Forms.Label();
            this.labelGETURL = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonPing = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPlayerID = new System.Windows.Forms.TextBox();
            this.textBoxServerDomain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.listViewAnswer = new System.Windows.Forms.ListView();
            this.AnswerID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SelectCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SwapCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalCost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Submitted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCopyAnswer = new System.Windows.Forms.Button();
            this.listViewBadCommand = new System.Windows.Forms.ListView();
            this.Enabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ID1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ID2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RelationDir = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonBadListClear = new System.Windows.Forms.Button();
            this.buttonRunSolverA = new System.Windows.Forms.Button();
            this.buttonRunSolverC = new System.Windows.Forms.Button();
            this.buttonRunSolverD = new System.Windows.Forms.Button();
            this.openFileDialog4 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog5 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog6 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenProblem
            // 
            this.buttonOpenProblem.Location = new System.Drawing.Point(8, 25);
            this.buttonOpenProblem.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenProblem.Name = "buttonOpenProblem";
            this.buttonOpenProblem.Size = new System.Drawing.Size(163, 29);
            this.buttonOpenProblem.TabIndex = 0;
            this.buttonOpenProblem.Text = "問題ファイル選択";
            this.buttonOpenProblem.UseVisualStyleBackColor = true;
            this.buttonOpenProblem.Click += new System.EventHandler(this.button_OpenProblem);
            // 
            // buttonParserSelect
            // 
            this.buttonParserSelect.Location = new System.Drawing.Point(8, 59);
            this.buttonParserSelect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonParserSelect.Name = "buttonParserSelect";
            this.buttonParserSelect.Size = new System.Drawing.Size(163, 29);
            this.buttonParserSelect.TabIndex = 4;
            this.buttonParserSelect.Text = "復元プログラム選択";
            this.buttonParserSelect.UseVisualStyleBackColor = true;
            this.buttonParserSelect.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "Program";
            this.openFileDialog2.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog2_FileOk);
            // 
            // buttonSolverASelect
            // 
            this.buttonSolverASelect.Location = new System.Drawing.Point(8, 92);
            this.buttonSolverASelect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSolverASelect.Name = "buttonSolverASelect";
            this.buttonSolverASelect.Size = new System.Drawing.Size(163, 29);
            this.buttonSolverASelect.TabIndex = 8;
            this.buttonSolverASelect.Text = "ソルバA";
            this.buttonSolverASelect.UseVisualStyleBackColor = true;
            this.buttonSolverASelect.Click += new System.EventHandler(this.buttonSolverASelect_Click);
            // 
            // buttonRunSolverB
            // 
            this.buttonRunSolverB.Location = new System.Drawing.Point(212, 422);
            this.buttonRunSolverB.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRunSolverB.Name = "buttonRunSolverB";
            this.buttonRunSolverB.Size = new System.Drawing.Size(171, 79);
            this.buttonRunSolverB.TabIndex = 9;
            this.buttonRunSolverB.Text = "ソルバBで解く";
            this.buttonRunSolverB.UseVisualStyleBackColor = true;
            this.buttonRunSolverB.Click += new System.EventHandler(this.buttonRunSolverB_Click);
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            this.openFileDialog3.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog3_FileOk);
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(1676, 803);
            this.buttonParse.Margin = new System.Windows.Forms.Padding(4);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(197, 59);
            this.buttonParse.TabIndex = 14;
            this.buttonParse.Text = "復元する(R)";
            this.buttonParse.UseVisualStyleBackColor = true;
            this.buttonParse.Click += new System.EventHandler(this.button6_Click);
            // 
            // trackBarResult
            // 
            this.trackBarResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarResult.Enabled = false;
            this.trackBarResult.Location = new System.Drawing.Point(982, 808);
            this.trackBarResult.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarResult.Name = "trackBarResult";
            this.trackBarResult.Size = new System.Drawing.Size(345, 56);
            this.trackBarResult.TabIndex = 18;
            this.trackBarResult.Scroll += new System.EventHandler(this.trackBarResult_Scroll);
            this.trackBarResult.ValueChanged += new System.EventHandler(this.trackBarResult_ValueChanged);
            // 
            // textBoxProblemFile
            // 
            this.textBoxProblemFile.Location = new System.Drawing.Point(179, 30);
            this.textBoxProblemFile.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProblemFile.Name = "textBoxProblemFile";
            this.textBoxProblemFile.Size = new System.Drawing.Size(557, 22);
            this.textBoxProblemFile.TabIndex = 17;
            this.textBoxProblemFile.TextChanged += new System.EventHandler(this.textBoxProblemFile_TextChanged);
            this.textBoxProblemFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxProblemFile_KeyDown);
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxSource.Location = new System.Drawing.Point(3, 511);
            this.pictureBoxSource.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(201, 154);
            this.pictureBoxSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSource.TabIndex = 19;
            this.pictureBoxSource.TabStop = false;
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxResult.Location = new System.Drawing.Point(986, 41);
            this.pictureBoxResult.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(887, 754);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 21;
            this.pictureBoxResult.TabStop = false;
            this.pictureBoxResult.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxResult_Paint);
            this.pictureBoxResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxResult_MouseDown);
            this.pictureBoxResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxResult_MouseMove);
            this.pictureBoxResult.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxResult_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSolverPathD);
            this.groupBox1.Controls.Add(this.buttonSolverDSelect);
            this.groupBox1.Controls.Add(this.textBoxSolverPathC);
            this.groupBox1.Controls.Add(this.buttonSolverCSelect);
            this.groupBox1.Controls.Add(this.textBoxSolverPathB);
            this.groupBox1.Controls.Add(this.buttonSolverBSelect);
            this.groupBox1.Controls.Add(this.textBoxSolverPathA);
            this.groupBox1.Controls.Add(this.textBoxParserPath);
            this.groupBox1.Controls.Add(this.buttonOpenProblem);
            this.groupBox1.Controls.Add(this.textBoxProblemFile);
            this.groupBox1.Controls.Add(this.buttonParserSelect);
            this.groupBox1.Controls.Add(this.buttonSolverASelect);
            this.groupBox1.Location = new System.Drawing.Point(24, 190);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(745, 224);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ソルバー設定";
            // 
            // textBoxSolverPathD
            // 
            this.textBoxSolverPathD.Location = new System.Drawing.Point(179, 189);
            this.textBoxSolverPathD.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSolverPathD.Name = "textBoxSolverPathD";
            this.textBoxSolverPathD.Size = new System.Drawing.Size(557, 22);
            this.textBoxSolverPathD.TabIndex = 30;
            // 
            // buttonSolverDSelect
            // 
            this.buttonSolverDSelect.Location = new System.Drawing.Point(8, 187);
            this.buttonSolverDSelect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSolverDSelect.Name = "buttonSolverDSelect";
            this.buttonSolverDSelect.Size = new System.Drawing.Size(163, 29);
            this.buttonSolverDSelect.TabIndex = 29;
            this.buttonSolverDSelect.Text = "ソルバD";
            this.buttonSolverDSelect.UseVisualStyleBackColor = true;
            this.buttonSolverDSelect.Click += new System.EventHandler(this.buttonSolverDSelect_Click);
            // 
            // textBoxSolverPathC
            // 
            this.textBoxSolverPathC.Location = new System.Drawing.Point(179, 159);
            this.textBoxSolverPathC.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSolverPathC.Name = "textBoxSolverPathC";
            this.textBoxSolverPathC.Size = new System.Drawing.Size(557, 22);
            this.textBoxSolverPathC.TabIndex = 28;
            // 
            // buttonSolverCSelect
            // 
            this.buttonSolverCSelect.Location = new System.Drawing.Point(8, 155);
            this.buttonSolverCSelect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSolverCSelect.Name = "buttonSolverCSelect";
            this.buttonSolverCSelect.Size = new System.Drawing.Size(163, 29);
            this.buttonSolverCSelect.TabIndex = 27;
            this.buttonSolverCSelect.Text = "ソルバC";
            this.buttonSolverCSelect.UseVisualStyleBackColor = true;
            this.buttonSolverCSelect.Click += new System.EventHandler(this.buttonSolverCSelect_Click);
            // 
            // textBoxSolverPathB
            // 
            this.textBoxSolverPathB.Location = new System.Drawing.Point(179, 129);
            this.textBoxSolverPathB.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSolverPathB.Name = "textBoxSolverPathB";
            this.textBoxSolverPathB.Size = new System.Drawing.Size(557, 22);
            this.textBoxSolverPathB.TabIndex = 26;
            // 
            // buttonSolverBSelect
            // 
            this.buttonSolverBSelect.Location = new System.Drawing.Point(8, 122);
            this.buttonSolverBSelect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSolverBSelect.Name = "buttonSolverBSelect";
            this.buttonSolverBSelect.Size = new System.Drawing.Size(163, 29);
            this.buttonSolverBSelect.TabIndex = 25;
            this.buttonSolverBSelect.Text = "ソルバB";
            this.buttonSolverBSelect.UseVisualStyleBackColor = true;
            this.buttonSolverBSelect.Click += new System.EventHandler(this.buttonSolverBSelect_Click);
            // 
            // textBoxSolverPathA
            // 
            this.textBoxSolverPathA.Location = new System.Drawing.Point(179, 94);
            this.textBoxSolverPathA.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSolverPathA.Name = "textBoxSolverPathA";
            this.textBoxSolverPathA.Size = new System.Drawing.Size(557, 22);
            this.textBoxSolverPathA.TabIndex = 24;
            // 
            // textBoxParserPath
            // 
            this.textBoxParserPath.Location = new System.Drawing.Point(179, 64);
            this.textBoxParserPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxParserPath.Name = "textBoxParserPath";
            this.textBoxParserPath.Size = new System.Drawing.Size(557, 22);
            this.textBoxParserPath.TabIndex = 23;
            this.textBoxParserPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxParserPath_KeyDown);
            // 
            // buttonHTTPGET
            // 
            this.buttonHTTPGET.Location = new System.Drawing.Point(603, 119);
            this.buttonHTTPGET.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHTTPGET.Name = "buttonHTTPGET";
            this.buttonHTTPGET.Size = new System.Drawing.Size(116, 36);
            this.buttonHTTPGET.TabIndex = 25;
            this.buttonHTTPGET.Text = "問題取得";
            this.buttonHTTPGET.UseVisualStyleBackColor = true;
            this.buttonHTTPGET.Click += new System.EventHandler(this.buttonHTTPGET_Click);
            // 
            // checkBoxGrid
            // 
            this.checkBoxGrid.AutoSize = true;
            this.checkBoxGrid.Location = new System.Drawing.Point(1773, 13);
            this.checkBoxGrid.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxGrid.Name = "checkBoxGrid";
            this.checkBoxGrid.Size = new System.Drawing.Size(100, 19);
            this.checkBoxGrid.TabIndex = 28;
            this.checkBoxGrid.Text = "グリッド表示";
            this.checkBoxGrid.UseVisualStyleBackColor = true;
            this.checkBoxGrid.CheckedChanged += new System.EventHandler(this.checkBoxGrid_CheckedChanged);
            // 
            // checkBoxNumber
            // 
            this.checkBoxNumber.AutoSize = true;
            this.checkBoxNumber.Location = new System.Drawing.Point(1676, 13);
            this.checkBoxNumber.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxNumber.Name = "checkBoxNumber";
            this.checkBoxNumber.Size = new System.Drawing.Size(89, 19);
            this.checkBoxNumber.TabIndex = 27;
            this.checkBoxNumber.Text = "数字表示";
            this.checkBoxNumber.UseVisualStyleBackColor = true;
            this.checkBoxNumber.CheckedChanged += new System.EventHandler(this.checkBoxNumber_CheckedChanged);
            // 
            // textBoxLog
            // 
            this.textBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLog.Location = new System.Drawing.Point(3, 673);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(971, 191);
            this.textBoxLog.TabIndex = 25;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxForPractice);
            this.groupBox2.Controls.Add(this.checkBoxAutoRetry);
            this.groupBox2.Controls.Add(this.maskedtextBoxProblemID);
            this.groupBox2.Controls.Add(this.buttonHTTPGET);
            this.groupBox2.Controls.Add(this.labelPOSTURL);
            this.groupBox2.Controls.Add(this.labelGETURL);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonPing);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxPlayerID);
            this.groupBox2.Controls.Add(this.textBoxServerDomain);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(24, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(745, 168);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "サーバ設定";
            // 
            // checkBoxForPractice
            // 
            this.checkBoxForPractice.AutoSize = true;
            this.checkBoxForPractice.Location = new System.Drawing.Point(115, 0);
            this.checkBoxForPractice.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxForPractice.Name = "checkBoxForPractice";
            this.checkBoxForPractice.Size = new System.Drawing.Size(104, 19);
            this.checkBoxForPractice.TabIndex = 29;
            this.checkBoxForPractice.Text = "練習場仕様";
            this.checkBoxForPractice.UseVisualStyleBackColor = true;
            this.checkBoxForPractice.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBoxAutoRetry
            // 
            this.checkBoxAutoRetry.AutoSize = true;
            this.checkBoxAutoRetry.Location = new System.Drawing.Point(612, 98);
            this.checkBoxAutoRetry.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAutoRetry.Name = "checkBoxAutoRetry";
            this.checkBoxAutoRetry.Size = new System.Drawing.Size(100, 19);
            this.checkBoxAutoRetry.TabIndex = 29;
            this.checkBoxAutoRetry.Text = "自動リトライ";
            this.checkBoxAutoRetry.UseVisualStyleBackColor = true;
            // 
            // maskedtextBoxProblemID
            // 
            this.maskedtextBoxProblemID.Location = new System.Drawing.Point(661, 60);
            this.maskedtextBoxProblemID.Margin = new System.Windows.Forms.Padding(4);
            this.maskedtextBoxProblemID.Mask = "99";
            this.maskedtextBoxProblemID.Name = "maskedtextBoxProblemID";
            this.maskedtextBoxProblemID.Size = new System.Drawing.Size(56, 22);
            this.maskedtextBoxProblemID.TabIndex = 31;
            this.maskedtextBoxProblemID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedtextBoxProblemID.TextChanged += new System.EventHandler(this.maskedtextBoxProblemID_TextChanged);
            // 
            // labelPOSTURL
            // 
            this.labelPOSTURL.AutoSize = true;
            this.labelPOSTURL.Location = new System.Drawing.Point(119, 136);
            this.labelPOSTURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPOSTURL.Name = "labelPOSTURL";
            this.labelPOSTURL.Size = new System.Drawing.Size(34, 15);
            this.labelPOSTURL.TabIndex = 30;
            this.labelPOSTURL.Text = "URL";
            // 
            // labelGETURL
            // 
            this.labelGETURL.AutoSize = true;
            this.labelGETURL.Location = new System.Drawing.Point(119, 102);
            this.labelGETURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGETURL.Name = "labelGETURL";
            this.labelGETURL.Size = new System.Drawing.Size(34, 15);
            this.labelGETURL.TabIndex = 29;
            this.labelGETURL.Text = "URL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 136);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 15);
            this.label6.TabIndex = 28;
            this.label6.Text = "問題提出URL";
            // 
            // buttonPing
            // 
            this.buttonPing.Location = new System.Drawing.Point(603, 26);
            this.buttonPing.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(116, 29);
            this.buttonPing.TabIndex = 27;
            this.buttonPing.Text = "Ping";
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(600, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 15);
            this.label5.TabIndex = 27;
            this.label5.Text = "問題ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "プレイヤーID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "サーバドメイン";
            // 
            // textBoxPlayerID
            // 
            this.textBoxPlayerID.Location = new System.Drawing.Point(115, 60);
            this.textBoxPlayerID.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPlayerID.Name = "textBoxPlayerID";
            this.textBoxPlayerID.Size = new System.Drawing.Size(456, 22);
            this.textBoxPlayerID.TabIndex = 23;
            this.textBoxPlayerID.TextChanged += new System.EventHandler(this.textBoxPlayerID_TextChanged);
            // 
            // textBoxServerDomain
            // 
            this.textBoxServerDomain.Location = new System.Drawing.Point(115, 29);
            this.textBoxServerDomain.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxServerDomain.Name = "textBoxServerDomain";
            this.textBoxServerDomain.Size = new System.Drawing.Size(456, 22);
            this.textBoxServerDomain.TabIndex = 17;
            this.textBoxServerDomain.TextChanged += new System.EventHandler(this.textBoxServerDomain_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "問題取得URL";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Enabled = false;
            this.buttonSubmit.Location = new System.Drawing.Point(699, 575);
            this.buttonSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(70, 90);
            this.buttonSubmit.TabIndex = 28;
            this.buttonSubmit.Text = "提出する";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // listViewAnswer
            // 
            this.listViewAnswer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AnswerID,
            this.SelectCount,
            this.SwapCount,
            this.TotalCost,
            this.Submitted});
            this.listViewAnswer.FullRowSelect = true;
            this.listViewAnswer.GridLines = true;
            this.listViewAnswer.HideSelection = false;
            this.listViewAnswer.Location = new System.Drawing.Point(212, 511);
            this.listViewAnswer.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAnswer.MultiSelect = false;
            this.listViewAnswer.Name = "listViewAnswer";
            this.listViewAnswer.Size = new System.Drawing.Size(479, 154);
            this.listViewAnswer.TabIndex = 29;
            this.listViewAnswer.UseCompatibleStateImageBehavior = false;
            this.listViewAnswer.View = System.Windows.Forms.View.Details;
            this.listViewAnswer.SelectedIndexChanged += new System.EventHandler(this.listViewAnswer_SelectedIndexChanged);
            // 
            // AnswerID
            // 
            this.AnswerID.Text = "ID";
            this.AnswerID.Width = 32;
            // 
            // SelectCount
            // 
            this.SelectCount.Text = "選択回数";
            this.SelectCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SelectCount.Width = 87;
            // 
            // SwapCount
            // 
            this.SwapCount.Text = "交換回数";
            this.SwapCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SwapCount.Width = 91;
            // 
            // TotalCost
            // 
            this.TotalCost.Text = "合計コスト";
            this.TotalCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalCost.Width = 100;
            // 
            // Submitted
            // 
            this.Submitted.Text = "提出結果";
            this.Submitted.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Submitted.Width = 151;
            // 
            // buttonCopyAnswer
            // 
            this.buttonCopyAnswer.Enabled = false;
            this.buttonCopyAnswer.Location = new System.Drawing.Point(699, 509);
            this.buttonCopyAnswer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCopyAnswer.Name = "buttonCopyAnswer";
            this.buttonCopyAnswer.Size = new System.Drawing.Size(70, 58);
            this.buttonCopyAnswer.TabIndex = 30;
            this.buttonCopyAnswer.Text = "コピー";
            this.buttonCopyAnswer.UseVisualStyleBackColor = true;
            this.buttonCopyAnswer.Click += new System.EventHandler(this.buttonCopyAnswer_Click);
            // 
            // listViewBadCommand
            // 
            this.listViewBadCommand.CheckBoxes = true;
            this.listViewBadCommand.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Enabled,
            this.ID1,
            this.ID2,
            this.RelationDir});
            this.listViewBadCommand.FullRowSelect = true;
            this.listViewBadCommand.GridLines = true;
            this.listViewBadCommand.HideSelection = false;
            this.listViewBadCommand.Location = new System.Drawing.Point(777, 47);
            this.listViewBadCommand.Margin = new System.Windows.Forms.Padding(4);
            this.listViewBadCommand.MultiSelect = false;
            this.listViewBadCommand.Name = "listViewBadCommand";
            this.listViewBadCommand.Size = new System.Drawing.Size(206, 618);
            this.listViewBadCommand.TabIndex = 31;
            this.listViewBadCommand.UseCompatibleStateImageBehavior = false;
            this.listViewBadCommand.View = System.Windows.Forms.View.Details;
            // 
            // Enabled
            // 
            this.Enabled.Text = "有効";
            this.Enabled.Width = 40;
            // 
            // ID1
            // 
            this.ID1.Text = "ID1";
            this.ID1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ID1.Width = 55;
            // 
            // ID2
            // 
            this.ID2.Text = "ID2";
            this.ID2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ID2.Width = 55;
            // 
            // RelationDir
            // 
            this.RelationDir.Text = "向き";
            this.RelationDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RelationDir.Width = 55;
            // 
            // buttonBadListClear
            // 
            this.buttonBadListClear.Location = new System.Drawing.Point(777, 9);
            this.buttonBadListClear.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBadListClear.Name = "buttonBadListClear";
            this.buttonBadListClear.Size = new System.Drawing.Size(201, 30);
            this.buttonBadListClear.TabIndex = 32;
            this.buttonBadListClear.Text = "リストクリア";
            this.buttonBadListClear.UseVisualStyleBackColor = true;
            this.buttonBadListClear.Click += new System.EventHandler(this.buttonBadListClear_Click);
            // 
            // buttonRunSolverA
            // 
            this.buttonRunSolverA.Location = new System.Drawing.Point(24, 422);
            this.buttonRunSolverA.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRunSolverA.Name = "buttonRunSolverA";
            this.buttonRunSolverA.Size = new System.Drawing.Size(171, 79);
            this.buttonRunSolverA.TabIndex = 33;
            this.buttonRunSolverA.Text = "ソルバAで解く";
            this.buttonRunSolverA.UseVisualStyleBackColor = true;
            this.buttonRunSolverA.Click += new System.EventHandler(this.buttonRunSolverA_Click);
            // 
            // buttonRunSolverC
            // 
            this.buttonRunSolverC.Location = new System.Drawing.Point(398, 422);
            this.buttonRunSolverC.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRunSolverC.Name = "buttonRunSolverC";
            this.buttonRunSolverC.Size = new System.Drawing.Size(171, 79);
            this.buttonRunSolverC.TabIndex = 34;
            this.buttonRunSolverC.Text = "ソルバCで解く";
            this.buttonRunSolverC.UseVisualStyleBackColor = true;
            this.buttonRunSolverC.Click += new System.EventHandler(this.buttonRunSolverC_Click);
            // 
            // buttonRunSolverD
            // 
            this.buttonRunSolverD.Location = new System.Drawing.Point(589, 422);
            this.buttonRunSolverD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRunSolverD.Name = "buttonRunSolverD";
            this.buttonRunSolverD.Size = new System.Drawing.Size(171, 79);
            this.buttonRunSolverD.TabIndex = 35;
            this.buttonRunSolverD.Text = "ソルバDで解く";
            this.buttonRunSolverD.UseVisualStyleBackColor = true;
            this.buttonRunSolverD.Click += new System.EventHandler(this.buttonRunSolverD_Click);
            // 
            // openFileDialog4
            // 
            this.openFileDialog4.FileName = "openFileDialog4";
            this.openFileDialog4.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog4_FileOk);
            // 
            // openFileDialog5
            // 
            this.openFileDialog5.FileName = "openFileDialog5";
            this.openFileDialog5.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog5_FileOk);
            // 
            // openFileDialog6
            // 
            this.openFileDialog6.FileName = "openFileDialog6";
            this.openFileDialog6.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog6_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1882, 875);
            this.Controls.Add(this.buttonRunSolverD);
            this.Controls.Add(this.buttonRunSolverC);
            this.Controls.Add(this.buttonRunSolverA);
            this.Controls.Add(this.buttonBadListClear);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.checkBoxGrid);
            this.Controls.Add(this.listViewBadCommand);
            this.Controls.Add(this.checkBoxNumber);
            this.Controls.Add(this.buttonCopyAnswer);
            this.Controls.Add(this.listViewAnswer);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBoxSource);
            this.Controls.Add(this.buttonParse);
            this.Controls.Add(this.buttonRunSolverB);
            this.Controls.Add(this.trackBarResult);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1920, 920);
            this.MinimumSize = new System.Drawing.Size(1900, 920);
            this.Name = "MainForm";
            this.Text = "PRO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenProblem;
        private System.Windows.Forms.Button buttonParserSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button buttonSolverASelect;
        private System.Windows.Forms.Button buttonRunSolverB;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.Button buttonParse;
        private System.Windows.Forms.TrackBar trackBarResult;
        private System.Windows.Forms.TextBox textBoxProblemFile;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.TextBox textBoxParserPath;
        private System.Windows.Forms.TextBox textBoxSolverPathA;
        private System.Windows.Forms.CheckBox checkBoxNumber;
        private System.Windows.Forms.CheckBox checkBoxGrid;
        private System.Windows.Forms.Button buttonHTTPGET;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPlayerID;
        private System.Windows.Forms.TextBox textBoxServerDomain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelGETURL;
        private System.Windows.Forms.Label labelPOSTURL;
        private System.Windows.Forms.MaskedTextBox maskedtextBoxProblemID;
        private System.Windows.Forms.CheckBox checkBoxAutoRetry;
        private System.Windows.Forms.CheckBox checkBoxForPractice;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.ListView listViewAnswer;
        private System.Windows.Forms.ColumnHeader AnswerID;
        private System.Windows.Forms.ColumnHeader SelectCount;
        private System.Windows.Forms.ColumnHeader SwapCount;
        private System.Windows.Forms.ColumnHeader TotalCost;
        private System.Windows.Forms.ColumnHeader Submitted;
        private System.Windows.Forms.Button buttonCopyAnswer;
        private System.Windows.Forms.ListView listViewBadCommand;
        private System.Windows.Forms.ColumnHeader Enabled;
        private System.Windows.Forms.ColumnHeader ID1;
        private System.Windows.Forms.ColumnHeader ID2;
        private System.Windows.Forms.ColumnHeader RelationDir;
        private System.Windows.Forms.Button buttonBadListClear;
        private System.Windows.Forms.TextBox textBoxSolverPathC;
        private System.Windows.Forms.Button buttonSolverCSelect;
        private System.Windows.Forms.TextBox textBoxSolverPathB;
        private System.Windows.Forms.Button buttonSolverBSelect;
        private System.Windows.Forms.TextBox textBoxSolverPathD;
        private System.Windows.Forms.Button buttonSolverDSelect;
        private System.Windows.Forms.Button buttonRunSolverA;
        private System.Windows.Forms.Button buttonRunSolverC;
        private System.Windows.Forms.Button buttonRunSolverD;
        private System.Windows.Forms.OpenFileDialog openFileDialog4;
        private System.Windows.Forms.OpenFileDialog openFileDialog5;
        private System.Windows.Forms.OpenFileDialog openFileDialog6;
    }
}

