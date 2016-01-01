using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

// src: http://code-life.net/?p=717
namespace Ppm
{
    public struct Information
    {
        public int dw, dh, allow, selectCost, swapCost, w, h, brightness;
        public Bitmap img;
    };

    class Program
    {
        private static string readLine(ref FileStream fs)
        {
            Byte b;
            string l = "";
            while ((b = (Byte)fs.ReadByte()) != 10) // 10 = (lf)
            {
                if ((char)b == '\r')
                {
                    continue;
                }
                else
                {
                    l += (char)b;
                }
            }
            return l;
        }

        public static Information? Main(ProconVisualizer.MainForm parent, string name)
        {
            Information ret = new Information();
            int readSize;
            byte[] P = new byte[2];
            byte[] lf = new byte[1];
            string line="";
            FileStream fs = null;
            try
            {
                fs = new FileStream(name, FileMode.Open, FileAccess.Read);
                readSize = fs.Read(P, 0, P.Length);
                string bufString = Encoding.UTF8.GetString(P);
                if (bufString != "P6")
                {
                    parent.appendLog("PPMファイルがバイナリ形式ではありません");
                    fs.Dispose();
                    return null;
                }
                line = readLine(ref fs); // 改行

                line = readLine(ref fs); // 分割サイズ
                string[] dsize = line.Split(new char[] { ' ' });
                ret.dw = int.Parse(dsize[1]);
                ret.dh = int.Parse(dsize[2]);
                Console.WriteLine(line);

                line = readLine(ref fs); // allow
                string[] allow = line.Split(new char[] { ' ' });
                ret.allow = int.Parse(allow[1]);
                Console.WriteLine(line);

                line = readLine(ref fs); // cost
                string[] costs = line.Split(new char[] { ' ' });
                ret.selectCost = int.Parse(costs[1]);
                ret.swapCost = int.Parse(costs[2]);
                Console.WriteLine(line);

                //横・縦サイズ取得(ゴミを読み飛ばす)
                while ((line = readLine(ref fs))[0] == '#')
                {
                    Console.WriteLine(">" + line);
                }
                Console.WriteLine(line);

                string[] size = line.Split(new char[] { ' ' });
                int x = int.Parse(size[0]);
                int y = int.Parse(size[1]);
                ret.w = x;
                ret.h = y;
                line = readLine(ref fs);
                Console.WriteLine(line);
                if (line != "255")
                {
                    parent.appendLog("[警告]問題が24bitカラーではありません");
                    ret.brightness = 255;
                }
                else
                {
                    ret.brightness = 255;
                }

                // fs.Seek(line.Length+fPos, SeekOrigin.Begin);

                //改行コード読み飛ばし
                //readSize = fs.Read(lf, 0, lf.Length);
                //bufString = Encoding.UTF8.GetString(lf);


                byte[,] datar = new byte[x, y];
                byte[,] datag = new byte[x, y];
                byte[,] datab = new byte[x, y];
                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        datar[j, i] = (Byte)fs.ReadByte();
                        datag[j, i] = (Byte)fs.ReadByte();
                        datab[j, i] = (Byte)fs.ReadByte();
                    }
                }

                Bitmap bitmap = new Bitmap(x, y, PixelFormat.Format24bppRgb);
                int[,] r = new int[x, y];
                int[,] g = new int[x, y];
                int[,] b = new int[x, y];


                for (int yy = 0; yy < y; yy++)
                {
                    for (int xx = 0; xx < x; xx++)
                    {
                        r[xx, yy] = ((int)datar[xx, yy]);
                        g[xx, yy] = ((int)datag[xx, yy]);
                        b[xx, yy] = ((int)datab[xx, yy]);

                        bitmap.SetPixel(xx, yy,
                        Color.FromArgb(
                        r[xx, yy],
                        g[xx, yy],
                        b[xx, yy]
                        ));

                    }
                }
                ret.img = bitmap;
                return ret;
            }
            catch (FileNotFoundException e)
            {
                parent.appendLog("PPMファイルが存在しません。");
                parent.appendLog(e.ToString());
            }
            catch (Exception e)
            {
                parent.appendLog("読み込みに失敗しました。");
                parent.appendLog(e.ToString());
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
            return null;
        }
    }
}