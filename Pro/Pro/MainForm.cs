using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Net;

namespace ProconVisualizer
{
    public partial class MainForm : Form
    {
        public Ppm.Information problemInfo; // Problem Information
        private Bitmap resultImage = null;
        private Rectangle resultImageRectangle;
        bool mouseDown = false;
        private Point mouseDownPoint;
        private Point mouseMovePoint;
        public int[] array;
        public int[] array_r;
        public int[] decide_log;

        string prob_name = ""; // 問題パス
        string parser_name = ""; // 画像解析(=パース)プログラムパス
        string solverNameA = "";
        string solverNameB = "";
        string solverNameC = "";
        string solverNameD = "";

        List<string> answerList;

        // ログテキストボックス
        public void appendLog(string logText)
        {
            textBoxLog.SelectionStart = textBoxLog.Text.Length;
            textBoxLog.SelectionLength = 0;
            textBoxLog.SelectedText = "[" + System.DateTime.Now.ToString() + "] " + logText + "\r\n";
            textBoxLog.ScrollToCaret();
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            initWrite();
        }

        private void initWrite()
        {
            string fileName = "init.ini";
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding("Shift_JIS"));
            writer.WriteLine(prob_name);
            writer.WriteLine(parser_name);
            writer.WriteLine(solverNameA);
            writer.WriteLine(solverNameB);
            writer.WriteLine(solverNameC);
            writer.WriteLine(solverNameD);
            writer.WriteLine(textBoxServerDomain.Text);
            writer.WriteLine(textBoxPlayerID.Text);
            writer.WriteLine(maskedtextBoxProblemID.Text);
            writer.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            answerList = new List<string>();

            string fileName = "init.ini";
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("UTF-8"));

                textBoxProblemFile.Text = prob_name = sr.ReadLine();
                textBoxParserPath.Text = parser_name = sr.ReadLine();
                textBoxSolverPathA.Text = solverNameA = sr.ReadLine();
                textBoxSolverPathB.Text = solverNameB = sr.ReadLine();
                textBoxSolverPathC.Text = solverNameC = sr.ReadLine();
                textBoxSolverPathD.Text = solverNameD = sr.ReadLine();
                textBoxServerDomain.Text = sr.ReadLine();
                textBoxPlayerID.Text = sr.ReadLine();
                maskedtextBoxProblemID.Text = sr.ReadLine();
            
                labelGETURL.Text = getProblemGETURL().ToString();
                labelPOSTURL.Text = getAnswerPOSTURL().ToString();

                sr.Close();
                if (File.Exists(prob_name))
                {
                    readingImg();
                }
            }
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            Pipe.Kill();
        }

        // =============================================================================================
        // HTTPクライアント
        // =============================================================================================
        public string getProblemSavePath()
        {
            //return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), getProblemFileName());
            return getProblemFileName();
        }

        public int getProblemID()
        {
            try
            {
                return int.Parse(maskedtextBoxProblemID.Text);
            }
            catch
            {
                appendLog("問題IDが正しくありません。");
                return -1;
            }
        }

        public string getProblemFileName(bool withExt = true)
        {
            int id = getProblemID();
            if (maskedtextBoxProblemID.Text.Length > 0 && id != -1)
            {
                if (withExt)
                {
                    return string.Format("prob{0:D2}.ppm", id);
                }
                else
                {
                    return string.Format("prob{0:D2}", id);
                }
            }
            return "";
        }

        public Uri getProblemGETURL()
        {
            try
            {
                var serverDomain = textBoxServerDomain.Text;
                if (checkBoxForPractice.Checked)
                {
                    return new Uri("http://" + serverDomain + "/static/" + getProblemFileName(false) + "/" + getProblemFileName());
                }
                else
                {
                    return new Uri("http://" + serverDomain + "/problem/" + getProblemFileName());
                }
            }
            catch
            {
                return new Uri("http://example.com/");
            }
        }

        public Uri getAnswerPOSTURL()
        {
            try
            {
                var serverDomain = textBoxServerDomain.Text;
                if (checkBoxForPractice.Checked)
                {
                    return new Uri("http://" + serverDomain + "/solve/json/" + getProblemID().ToString());
                }
                else
                {
                    return new Uri("http://" + serverDomain + "/SubmitAnswer");
                }
            }
            catch
            {
                return new Uri("http://example.com/");
            }
        }

        public async Task<string> HTTPPOSTAnswer(string ans)
        {
            try
            {
                appendLog("提出します。");
                using (var wc = new WebClient())
                {
                    string serverDomain = textBoxServerDomain.Text;
                    string problemId = maskedtextBoxProblemID.Text;
                    string playerid = textBoxPlayerID.Text;

                    var nvc = new NameValueCollection();
                    if (checkBoxForPractice.Checked)
                    {
                        string[] a = playerid.Split(',');
                        nvc.Add("username", a[0]);
                        nvc.Add("passwd", a[1]);
                        nvc.Add("answer_text ", ans);
                    }
                    else
                    {
                        nvc.Add("playerid", playerid);
                        nvc.Add("problemid", problemId);
                        nvc.Add("answer", ans);
                    }
                    byte[] res = await wc.UploadValuesTaskAsync(getAnswerPOSTURL(), nvc);
                    string result = Encoding.UTF8.GetString(res);
                    return result;
                }
            }
            catch (Exception e)
            {
                appendLog(e.ToString());
                appendLog("提出に失敗しました。");
                return null;
            }

        }

        public async Task<string> HTTPGETProblem()
        {
            try
            {
                string savePath = getProblemSavePath();
                appendLog("問題取得を開始します。");
                using (var wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(getProblemGETURL(), savePath);
                    appendLog("問題取得完了。");
                    appendLog(savePath);
                }
                return savePath;
            }
            catch(Exception e)
            {
                appendLog(e.ToString());
                appendLog("問題取得に失敗しました。");
                return null;
            }
        }

        // =============================================================================================
        // 問題ファイル入力関係
        // =============================================================================================

        // 問題ファイルを開くボタン
        private void button_OpenProblem(object sender, EventArgs e)
        {
            if (File.Exists(prob_name))
            {
                openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(prob_name);
            }
            openFileDialog1.Filter = "All files (*.*)|*.*|ppm files (*.ppm)|*.ppm|txt files (*.txt)|*.txt";
            openFileDialog1.ShowDialog();
        }

        // 問題ファイルを開くダイアログ処理正常終了
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBoxProblemFile.Text = prob_name = openFileDialog1.FileName;
            readingImg();
        }

        // 問題ファイルのテキストボックスでのキーイベント
        private void textBoxProblemFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (File.Exists(textBoxProblemFile.Text))
                {
                    var path = Path.GetFullPath(textBoxProblemFile.Text);
                    if (prob_name != path)
                    {
                        prob_name = path;
                        readingImg();
                    }
                }
                else
                {
                    appendLog("問題ファイルが存在しません。");
                }
            }
        }

        // 問題ファイル読み込み
        void readingImg()
        {
            var info = Ppm.Program.Main(this, prob_name);
            if (info != null)
            {
                problemInfo = (Ppm.Information)info;
                refreshSourceImage();
                array = null;
                array_r = null;
                appendLog("問題の読み込みに成功しました。");
                tryStartParser();
            }
        }

        // =============================================================================================
        // パーサ関係
        // =============================================================================================
        // ファイルを開くボタン
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(parser_name))
            {
                openFileDialog2.InitialDirectory = System.IO.Path.GetDirectoryName(parser_name);
            }
            openFileDialog2.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog2.ShowDialog();
        }

        // ファイルを開くダイアログ処理正常終了
        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            textBoxParserPath.Text = parser_name = openFileDialog2.FileName;
            tryStartParser();
        }

        // パーサパステキストボックスでのキーイベント
        private void textBoxParserPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (File.Exists(textBoxParserPath.Text))
                {
                    var path = Path.GetFullPath(textBoxParserPath.Text);
                    if (parser_name != path)
                    {
                        parser_name = path;
                        tryStartParser();
                    }
                }
                else
                {
                    appendLog("復元プログラムが存在しません。");
                }
            }
        }

        // パーサ再起動ボタン
        private void buttonRestartParser_Click(object sender, EventArgs e)
        {
            tryStartParser();
        }

        // パーサ起動
        private void tryStartParser()
        {
            if(File.Exists(parser_name) && File.Exists(prob_name))
            {
                appendLog("復元プログラムを起動します。");
                Pipe.StartParserProcess(parser_name.Replace(@"\", @"\\"), prob_name.Replace(@"\", @"\\"));
                doParse();

            }
        }

        private void doParse()
        {
            var result = Pipe.RunParse();
            if (result == null)
            {
                appendLog("パースに失敗しました．");
                return;
            }
            else
            {
                appendLog("パースに成功しました。");
            }
            array = result;
            array_r = new int[array.Length];
            for (int i = 0; i < array.Length; ++i)
            {
                array_r[array[i]] = i;
            }
            decide_log = Pipe.LogCommand();
            trackBarResult.Maximum = decide_log.Length;
            trackBarResult.Value = decide_log.Length;
            trackBarResult.Minimum = 0;
            trackBarResult.Enabled = true;

            Bitmap canvas = new Bitmap(problemInfo.img.Width, problemInfo.img.Height); ;
            Graphics g = Graphics.FromImage(canvas);
            int sh = problemInfo.h / problemInfo.dh;
            int sw = problemInfo.w / problemInfo.dw;
            int k = 0;
            for (int i = 0; i < problemInfo.dh; i++)
            {
                for (int j = 0; j < problemInfo.dw; j++)
                {
                    Rectangle srcRect = new Rectangle(j * sw, i * sh, sw, sh);
                    Rectangle desRect = new Rectangle((array[k] % problemInfo.dw) * sw, (array[k] / problemInfo.dw) * sh, sw, sh);
                    g.DrawImage(problemInfo.img, desRect, srcRect, GraphicsUnit.Pixel);

                    k++;
                }
            }
            resultImage = canvas;
            updateResultImage();
            g.Dispose();
        }

        // BADリストクリア
        private void buttonBadListClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (Pipe.ClearCommand())
                {
                    listViewBadCommand.Items.Clear();
                    appendLog("BADリストをクリアしました");
                }
                else
                {
                    appendLog("BADリストクリアに失敗");
                }
            }
            catch (Exception ex)
            {
                appendLog("BADリストクリアに失敗");
                appendLog(ex.ToString());
            }
        }

        bool syncBadCommandList()
        {
            try
            {
                foreach (ListViewItem item in listViewBadCommand.Items)
                {
                    var ds = "RDLU";
                    var a = int.Parse(item.SubItems[1].Text);
                    var b = int.Parse(item.SubItems[2].Text);
                    var rstr = item.SubItems[3].Text;
                    var r = ds.IndexOf(rstr[0]);
                    if (item.Checked)
                    {
                        Pipe.BadCommand(a, b, r);
                    }
                    else
                    {
                        Pipe.AllowCommand(a, b, r);
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                appendLog(e.ToString());
                appendLog("BAD ALLOWの同期に失敗しました。");
                return false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonParse.Enabled = false;
            syncBadCommandList();
            doParse();
            buttonParse.Enabled = true;
        }

        // =============================================================================================
        // ソルバー関係
        // =============================================================================================
        private void buttonSolverASelect_Click(object sender, EventArgs e)
        {
            if (File.Exists(solverNameA))
            {
                openFileDialog3.InitialDirectory = System.IO.Path.GetDirectoryName(solverNameA);
            }
            openFileDialog3.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog3.ShowDialog();
        }

        private void buttonSolverBSelect_Click(object sender, EventArgs e)
        {
            if (File.Exists(solverNameB))
            {
                openFileDialog4.InitialDirectory = System.IO.Path.GetDirectoryName(solverNameB);
            }
            openFileDialog4.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog4.ShowDialog();
        }

        private void buttonSolverCSelect_Click(object sender, EventArgs e)
        {
            if (File.Exists(solverNameC))
            {
                openFileDialog5.InitialDirectory = System.IO.Path.GetDirectoryName(solverNameC);
            }
            openFileDialog5.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog5.ShowDialog();
        }
        private void buttonSolverDSelect_Click(object sender, EventArgs e)
        {
            if (File.Exists(solverNameD))
            {
                openFileDialog6.InitialDirectory = System.IO.Path.GetDirectoryName(solverNameD);
            }
            openFileDialog6.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog6.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            textBoxSolverPathA.Text = solverNameA = openFileDialog3.FileName;
        }

        private void openFileDialog4_FileOk(object sender, CancelEventArgs e)
        {
            textBoxSolverPathB.Text = solverNameB = openFileDialog4.FileName;
        }

        private void openFileDialog5_FileOk(object sender, CancelEventArgs e)
        {
            textBoxSolverPathC.Text = solverNameC = openFileDialog5.FileName;
        }

        private void openFileDialog6_FileOk(object sender, CancelEventArgs e)
        {
            textBoxSolverPathD.Text = solverNameD = openFileDialog6.FileName;
        }

        async Task<string> runSolver(string solverPath)
        {
            appendLog("スライドパズルを解きます。");
            try
            {
                string arg = "";
                arg += problemInfo.dw.ToString();
                arg += " "+problemInfo.dh.ToString();
                arg += " "+problemInfo.allow.ToString();
                arg += " "+problemInfo.selectCost.ToString();
                arg += " "+problemInfo.swapCost.ToString();
                for(int i = 0 ; i < array.Length ; i++)
                    arg += " "+array[i].ToString();
                appendLog(solverPath);
            
                var res = await Pipe.RunProcessAsync(solverPath, arg);
                if (res == null) throw new FormatException();
                appendLog("解けました。");
                var summary = getSummary(res);
                if(summary != null)
                {
                    int id = answerList.Count;
                    answerList.Add(res);
                    string[] column = {
                        id.ToString(),
                        summary.Item1.ToString(),
                        summary.Item2.ToString(),
                        summary.Item3.ToString(),
                        "-",
                    };
                    listViewAnswer.Items.Add(new ListViewItem(column));
                    listViewAnswer.EnsureVisible(listViewAnswer.Items.Count - 1);
                    return res;
                }
                else
                {
                    appendLog("無効な回答でした。");
                    return null;
                }
            }
            catch(Exception ex)
            {
                appendLog(ex.ToString());
                appendLog("解くのに失敗しました。");
            }
            return null;
        }

        private static int hexToDec(char x)
        {
            if (x >= 'A') return x - 'A' + 10;
            return x - '0';
        }

        public Tuple<int, int, int> getSummary(string output)
        {
            try
            {

                System.IO.StringReader rs = new System.IO.StringReader(output);
                string buf = rs.ReadLine();
                int count = int.Parse(buf);
                int move = 0;
                if (count > problemInfo.allow)
                {

                    appendLog("選択回数オーバー");
                    return null;
                }
                List<int> sx = new List<int>(), sy = new List<int>();
                List<List<int>> moves = new List<List<int>>();

                while (rs.Peek() > -1)
                {

                    buf = rs.ReadLine(); // さわりはじめの場所
                    sx.Add(hexToDec(buf[0]));
                    sy.Add(hexToDec(buf[1]));
                    buf = rs.ReadLine(); // 何回続く?(要らない情報)
                    move += int.Parse(buf);
                    buf = rs.ReadLine(); // 実際の操作列
                    moves.Add(new List<int>());
                    for (int i = 0; i < buf.Length; i++)
                    {
                        const string table = "URDL";
                        int idx = table.IndexOf(buf[i]);
                        moves[moves.Count - 1].Add(idx);
                    }
                }

                int total = count * problemInfo.selectCost + move * problemInfo.swapCost;
                return new Tuple<int, int, int>(count, move, total);
            }
            catch
            {
                appendLog("ソルバの出力が異常です。");
                return null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public string filename { get; set; }


        public void refreshSourceImage()
        {
            if (problemInfo.img != null)
            {
                pictureBoxSource.BackgroundImage = problemInfo.img;
            }
        }

        public void updateResultImage()
        {
            if (resultImage != null)
            {
                pictureBoxResult.BackgroundImage = resultImage;
                resultImageRectangle.X = 0;
                resultImageRectangle.Y = 0;
                resultImageRectangle.Width = resultImage.Width;
                resultImageRectangle.Height = resultImage.Height;
                updateResultImageOption();
            }
        }

        public void updateResultImageOption()
        {
            if (resultImage == null)
            {
                return;
            }

            var width = pictureBoxResult.BackgroundImage.Width;
            var height = pictureBoxResult.BackgroundImage.Height;
            var img = new Bitmap(width, height);
            var g = Graphics.FromImage(img);
            var cellWidth = width / problemInfo.dw;
            var cellHeight = height / problemInfo.dh;
            for (var i = trackBarResult.Value; i < decide_log.Length; ++i)
            {
                var p = array[decide_log[i]];
                var posX = (p % problemInfo.dw) * cellWidth;
                var posY = (p / problemInfo.dw) * cellHeight;
                g.FillRectangle(Brushes.Black, posX, posY, cellWidth, cellHeight);
            }

            if (checkBoxGrid.Checked)
            {
                for (int y = 0; y < problemInfo.dh; ++y)
                {
                    g.DrawLine(Pens.White, 0, y * cellHeight, problemInfo.w, y * cellHeight);
                }
                for (int x = 0; x < problemInfo.dw; ++x)
                {
                    g.DrawLine(Pens.White, x * cellWidth, 0, x * cellWidth, problemInfo.h);
                }
            }

            if (checkBoxNumber.Checked)
            {
                Font font = new Font("MS UI Gothic", 10, FontStyle.Bold);
                int k = 0;
                for (int y = 0; y < problemInfo.dh; ++y)
                {
                    for (int x = 0; x < problemInfo.dw; ++x)
                    {
                        var num = array_r[k++];
                        g.DrawString(num.ToString(), font, Brushes.DeepPink, x * cellWidth, y * cellHeight);
                    }
                }
            }

            pictureBoxResult.Image = img;
        }

        private void textBoxProblemFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // ==============================================================================
        // 復元画像のイベント関係
        // ==============================================================================
        private void pictureBoxResult_Paint(object sender, PaintEventArgs e)
        {
            if(resultImage == null) {
                return;
            }
            if(!trackBarResult.Enabled || decide_log == null && array == null) {
                return;
            }
            
            var g = e.Graphics;
            g.DrawLine(Pens.Gold, mouseDownPoint, mouseMovePoint);

        }

        Point toImagePosition(Point p)
        {
            if (pictureBoxResult.Image == null || resultImage == null)
            {
                return new Point(0, 0);
            }

            // 
            // http://stackoverflow.com/questions/10473582/how-to-retrieve-zoom-factor-of-a-winforms-picturebox
            // 

            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = pictureBoxResult.Image.Width; 
            int h_i = pictureBoxResult.Image.Height;
            int w_c = pictureBoxResult.Width;
            int h_c = pictureBoxResult.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X / scaleFactor);
                unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(p.Y / scaleFactor);
            }

            return unscaled_p;
        }

        int imagePositionToNumber(Point p)
        {
            var cellW = resultImage.Width / problemInfo.dw;
            var cellH = resultImage.Height / problemInfo.dh;
            var X = toImagePosition(p).X / cellW;
            var Y = toImagePosition(p).Y / cellH;
            var P = Y * problemInfo.dw + X;
            if (P >= 0 && P < array_r.Length)
            {
                return array_r[P];
            }
            else
            {
                return -1;
            }
        }

        private void pictureBoxResult_MouseDown(object sender, MouseEventArgs e)
        {
            if (resultImageRectangle.Contains(toImagePosition(e.Location)))
            {
                mouseDown = true;
                mouseDownPoint = e.Location;
            }
        }

        private void pictureBoxResult_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (resultImageRectangle.Contains(toImagePosition(e.Location)))
                {
                    mouseMovePoint = e.Location;
                    pictureBoxResult.Refresh(); 
                }
            }
        }

        private void pictureBoxResult_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (resultImageRectangle.Contains(toImagePosition(e.Location)))
                {
                    mouseMovePoint = e.Location;
                }
                else
                {
                    mouseDownPoint.X = mouseDownPoint.Y = 0;
                    mouseMovePoint.X = mouseMovePoint.Y = 0;
                }
                mouseDown = false;
                pictureBoxResult.Refresh();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (resultImage == null)
            {
                return;
            }
            if (e.KeyData == Keys.R)
            {
                button6_Click(this, e);
            }

            if (e.KeyData == Keys.B && !mouseDown)
            {
                if (mouseMovePoint.X == 0 && mouseMovePoint.Y == 0)
                {
                    return;
                }
                try
                {
                    var cellW = resultImage.Width / problemInfo.dw;
                    var cellH = resultImage.Height / problemInfo.dh;

                    var downX = toImagePosition(mouseDownPoint).X / cellW;
                    var downY = toImagePosition(mouseDownPoint).Y / cellH;
                    var downPos = downY * problemInfo.dw + downX;
                    var downID = array_r[downPos];

                    var moveX = toImagePosition(mouseMovePoint).X / cellW;
                    var moveY = toImagePosition(mouseMovePoint).Y / cellH;
                    var movePos = moveY * problemInfo.dw + moveX;
                    var moveID = array_r[movePos];

                    var dx = new int[] { 1, 0, -1, 0 };
                    var dy = new int[] { 0, 1, 0, -1 };
                    var ds = "RDLU";

                    for (int r = 0; r < 4; ++r)
                    {
                        var x = downX + dx[r];
                        var y = downY + dy[r];
                        if (x >= 0 && x < problemInfo.dw && y >= 0 && y < problemInfo.dh)
                        {
                            var p = y * problemInfo.dw + x;
                            if (array_r[p] == moveID)
                            {
                                appendLog("BADコマンド " + downID.ToString() + " " + moveID.ToString() + " " + r.ToString());
                                var before = Pipe.ConfigCommand();
                                Pipe.BadCommand(downID, moveID, r);
                                var after = Pipe.ConfigCommand();
                                if(before.Length < after.Length)
                                {
                                    string[] columns = { "", downID.ToString(), moveID.ToString(), ds[r].ToString() };
                                    var item = new ListViewItem(columns);
                                    item.Checked = true;
                                    listViewBadCommand.Items.Add(item);
                                }
                                else
                                {
                                    appendLog("すでに登録済みのようです。");
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    appendLog("BADコマンドに失敗しました。");
                    appendLog(exp.ToString());
                }
                finally
                {
                    mouseDownPoint.X = mouseDownPoint.Y = 0;
                    mouseMovePoint.X = mouseMovePoint.Y = 0;
                    Refresh();
                }
            }
        }


        private void trackBarResult_ValueChanged(object sender, EventArgs e)
        {
            updateResultImageOption();
        }

        private void checkBoxNumber_CheckedChanged(object sender, EventArgs e)
        {
            updateResultImageOption();
        }

        private void checkBoxGrid_CheckedChanged(object sender, EventArgs e)
        {
            updateResultImageOption();
        }

        private void checkBoxClickRectangle_CheckedChanged(object sender, EventArgs e)
        {
            updateResultImageOption();
        }

        async Task<bool> autoSequence()
        {
            string prob_path = await HTTPGETProblem();
            if (prob_path == null)
            {
                return false;
            }
            textBoxProblemFile.Text = prob_name = prob_path;
            readingImg();
            if (solverNameA != null)
            {
                string ans = await runSolver(solverNameA);
                if (ans != null && listViewAnswer.Items.Count > 0)
                {
                    var item = listViewAnswer.Items[listViewAnswer.Items.Count - 1];
                    submitAnswerByListItem(item);
                }
            }
            return true;
        }

        private async void buttonHTTPGET_Click(object sender, EventArgs e)
        {
            try
            {
                buttonHTTPGET.Enabled = false;

                bool success = false;

                if(checkBoxAutoRetry.Checked)
                {
                    while (!success && checkBoxAutoRetry.Checked)
                    {
                        success = await autoSequence();
                        await Task.Delay(500);
                    }
                }
                else
                {
                        success = await autoSequence();
                }
            }
            catch
            {
            }
            finally
            {
                buttonHTTPGET.Enabled = true;
            }
        }

        private async void submitAnswerByListItem(ListViewItem item)
        {
                try
                {
                    var subitems = item.SubItems;
                    var id = int.Parse(subitems[0].Text);
                    subitems[subitems.Count - 1].Text = "*";
                    var answer = answerList[id];
                    string result = await HTTPPOSTAnswer(answer);
                    if (result != null)
                    {
                        appendLog(result);
                        subitems[subitems.Count - 1].Text = result;
                    }
                }
                catch(Exception ex)
                {
                    appendLog(ex.ToString());
                    appendLog("提出に失敗しました。");
                }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            var items = listViewAnswer.SelectedItems;
            if (items.Count == 1)
            {
                submitAnswerByListItem(items[0]);
            }
            else
            {
                appendLog("回答が選択されていません。");
            }
        }

        private void buttonCopyAnswer_Click(object sender, EventArgs e)
        {
            var items = listViewAnswer.SelectedItems;
            if (items.Count == 1)
            {
                try
                {
                    var subitems = items[0].SubItems;
                    var id = int.Parse(subitems[0].Text);
                    var answer = answerList[id];
                    Clipboard.SetText(answer);
                    appendLog("回答をクリップボードにコピーしました。");
                }
                catch (Exception ex)
                {
                    appendLog(ex.ToString());
                    appendLog("コピーに失敗しました。");
                }
            }
            else
            {
                appendLog("回答が選択されていません。");
            }
        }

        private void textBoxServerDomain_TextChanged(object sender, EventArgs e)
        {
            labelGETURL.Text = getProblemGETURL().ToString();
            labelPOSTURL.Text = getAnswerPOSTURL().ToString();
        }

        private void textBoxPlayerID_TextChanged(object sender, EventArgs e)
        {
            labelGETURL.Text = getProblemGETURL().ToString();
            labelPOSTURL.Text = getAnswerPOSTURL().ToString();
        }

        private void maskedtextBoxProblemID_TextChanged(object sender, EventArgs e)
        {
            labelGETURL.Text = getProblemGETURL().ToString();
            labelPOSTURL.Text = getAnswerPOSTURL().ToString();
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            labelGETURL.Text = getProblemGETURL().ToString();
            labelPOSTURL.Text = getAnswerPOSTURL().ToString();
        }

        private void buttonPing_Click(object sender, EventArgs e)
        {
            if(textBoxServerDomain.Text != "")
            {
                try
                {
                    System.Net.NetworkInformation.Ping pg = new System.Net.NetworkInformation.Ping();
                    System.Net.NetworkInformation.PingReply pr = pg.Send(textBoxServerDomain.Text, 500);
                    if (pr.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        appendLog("Ping成功 IP=" + pr.Address.ToString() + " RTT=" + pr.RoundtripTime.ToString() + "[ms] TTL=" + pr.Options.Ttl.ToString() );
                    }
                    else
                    {
                        appendLog("Ping失敗");
                    }
                }
                catch
                {
                    appendLog("Ping失敗");
                }
            }
        }

        private void listViewAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonCopyAnswer.Enabled = true;
            buttonSubmit.Enabled = true;
        }

        private void trackBarResult_Scroll(object sender, EventArgs e)
        {

        }

        private void textBoxSolverPath_TextChanged(object sender, EventArgs e)
        {

        }
        private async void buttonRunSolverA_Click(object sender, EventArgs e)
        {
            buttonRunSolverA.Enabled = false;
            await runSolver(solverNameA);
            buttonRunSolverA.Enabled = true;
        }

        private async void buttonRunSolverB_Click(object sender, EventArgs e)
        {
            buttonRunSolverB.Enabled = false;
            await runSolver(solverNameB);
            buttonRunSolverB.Enabled = true;
        }

        private async void buttonRunSolverC_Click(object sender, EventArgs e)
        {
            buttonRunSolverC.Enabled = false;
            await runSolver(solverNameC);
            buttonRunSolverC.Enabled = true;
        }

        private async void buttonRunSolverD_Click(object sender, EventArgs e)
        {
            buttonRunSolverD.Enabled = false;
            await runSolver(solverNameD);
            buttonRunSolverD.Enabled = true;
        }



    }
}
