using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ProconVisualizer
{
    class Pipe
    {
        private static Process parserProcess;
        private static StringBuilder parserOutput;
        private static bool waitingParserProcess;
        private static bool acceptedCurrentParserCommand;

        public static void StartParserProcess(string command, string filename)
        {
            Kill();
            parserOutput = new StringBuilder();
            parserProcess = new Process();
            parserProcess.StartInfo.FileName = command; // 実行するファイル
            parserProcess.StartInfo.Arguments = filename;
            parserProcess.StartInfo.Arguments += " -int"; // インタラクティブモード
            parserProcess.StartInfo.CreateNoWindow = true; // コンソールを開かない
            parserProcess.StartInfo.UseShellExecute = false; // シェル機能を使用しない
            parserProcess.StartInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            parserProcess.StartInfo.RedirectStandardInput = true; // 標準出力をリダイレクト
            parserProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            parserProcess.Start(); // アプリの実行開始
            parserProcess.BeginOutputReadLine();
        }

        public static void Kill()
        {
            if (parserProcess != null)
            {
                if (!parserProcess.HasExited)
                {
                    parserProcess.Kill();
                }
                parserProcess = null;
            }
        }

        public static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                Debug.WriteLine(outLine.Data);
                if (outLine.Data == "OK")
                {
                    acceptedCurrentParserCommand = true;
                }
                else if(outLine.Data == "NG")
                {
                    acceptedCurrentParserCommand = false;
                }
                else if(outLine.Data == "END")
                {

                    waitingParserProcess = false;
                }
                else
                {
                    parserOutput.AppendLine(outLine.Data);
                }
            }
        }

        public static int[] RunParse()
        {
            if(parserProcess == null)
            {
                return null;
            }

            waitingParserProcess = true;
            acceptedCurrentParserCommand = false;
            parserOutput.Clear();

            try
            {
                parserProcess.StandardInput.WriteLine("run");

                while (waitingParserProcess)
                {
                    System.Threading.Thread.Sleep(100); // 雑だけど妥協
                }
                string output = parserOutput.ToString();
                output = output.Replace("\r\n", "\n"); // 改行コードの修正
                Debug.Write(output); // ［出力］ウィンドウに出力
                string[] tmp = output.Split(new char[] { ' ', '\n' });
                if (tmp.Length < 5) return null;
                var list = new List<int>();
                for (int i = 5; i < tmp.Length; i++)
                {
                    if (tmp[i] == "") continue;
                    list.Add(int.Parse(tmp[i]));
                }
                return list.ToArray();
            }
            catch
            {
                return null;
            }
        }

		public static int[] LogCommand()

		{
			if (parserProcess == null)
			{
				return null;
			}

			waitingParserProcess = true;
			acceptedCurrentParserCommand = false;
			parserOutput.Clear();

			try
			{
				parserProcess.StandardInput.WriteLine("log");

				while (waitingParserProcess)
				{
					System.Threading.Thread.Sleep(100); // 雑だけど妥協
				}
				string output = parserOutput.ToString();
				output = output.Replace("\r\n", "\n"); // 改行コードの修正
				Debug.Write(output); // ［出力］ウィンドウに出力
				string[] tmp = output.Split(new char[] { ' ', '\n' });
				var retList = new List<int>();
				for (int i = 0; i < tmp.Length; i++)
				{
					if (tmp[i] == "") continue;
					retList.Add(int.Parse(tmp[i]));
				}
				return retList.ToArray();
			}
			catch
			{
				return null;
			}
		}

        public static bool BadCommand(int i, int j, int r)
        {
            if (parserProcess == null)
            {
                return false;
            }


            waitingParserProcess = true;
            acceptedCurrentParserCommand = false;
            parserOutput.Clear();

            try
            {
                string command = String.Format("bad {0} {1} {2}", i, j, r);
                parserProcess.StandardInput.WriteLine(command);
                while (waitingParserProcess)
                {
                    System.Threading.Thread.Sleep(10);
                }
                return acceptedCurrentParserCommand;

            }
            catch
            {
                return false;
            }
        }

        public static bool AllowCommand(int i, int j, int r)
        {
            if (parserProcess == null)
            {
                return false;
            }

            waitingParserProcess = true;

            acceptedCurrentParserCommand = false;
            parserOutput.Clear();

            try
            {
                string command = String.Format("allow {0} {1} {2}", i, j, r);
                parserProcess.StandardInput.WriteLine(command);
                while (waitingParserProcess)
                {
                    System.Threading.Thread.Sleep(10);
                }
                return acceptedCurrentParserCommand;
            }
            catch
            {

                return false;
            }
        }

        public static string ConfigCommand()
        {
            if (parserProcess == null)
            {
                return null;
            }

            waitingParserProcess = true;
            acceptedCurrentParserCommand = false;
            parserOutput.Clear();

            try
            {
                string command = "config";

                parserProcess.StandardInput.WriteLine(command);
                while (waitingParserProcess)
                {
                    System.Threading.Thread.Sleep(100);
                }
                return parserOutput.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static bool ClearCommand()
        {
            if (parserProcess == null)
            {
                return false;
            }

            waitingParserProcess = true;
            acceptedCurrentParserCommand = false;
            parserOutput.Clear();


            try
               
            {
                string command = "clear";
                parserProcess.StandardInput.WriteLine(command);

                while (waitingParserProcess)
                {
                    System.Threading.Thread.Sleep(10);
                }
                return acceptedCurrentParserCommand;
            }
            catch
            {
                return acceptedCurrentParserCommand;
            }
        }

        public static Task<string> Solve(string command,string inputText)
        {
            try
            {
                //http://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
                // there is no non-generic TaskCompletionSource

                var tcs = new TaskCompletionSource<string>();

                Process p = new Process();

                p.StartInfo.FileName = command; // 実行するファイル
                p.StartInfo.CreateNoWindow = true; // コンソールを開かない
                p.StartInfo.UseShellExecute = false; // シェル機能を使用しない
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
                p.EnableRaisingEvents = true;
                p.Exited += (sender, args) =>
                {
                    string output = p.StandardOutput.ReadToEnd(); // 標準出力の読み取り
                    tcs.SetResult(output);
                    p.Dispose();

                };
                p.Start(); // アプリの実行開始
                StreamWriter myStreamWriter = p.StandardInput;
                myStreamWriter.WriteLine(inputText);

                return tcs.Task;
            }
            catch
            {

                return null;
            }

        }

        //

        // http://www.levibotelho.com/development/async-processes-with-taskcompletionsource/
        //
        public static Task<string> RunProcessAsync(string processPath, string option)
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();
                var process = new Process
                {

                    EnableRaisingEvents = true,
                    StartInfo = new ProcessStartInfo(processPath)
                    {
                        CreateNoWindow = true, // コンソールを開かない
                        UseShellExecute = false, // シェル機能を使用しない
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true, // 標準出力をリダイレクト
                    }
                };
                StringBuilder res = new StringBuilder();
                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        res.AppendLine(e.Data);
                    }
                };
                process.Exited += (object sender, EventArgs e) =>
                {
                    tcs.SetResult(res.ToString());
                    process.Dispose();
                };
                process.Start();
                StreamWriter myStreamWriter = process.StandardInput;
                myStreamWriter.WriteLine(option);
                process.BeginOutputReadLine();
                return tcs.Task;
            }
            catch
            {
                return null;
            }
        }
    }
}