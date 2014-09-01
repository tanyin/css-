using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
namespace NewBaiduClick
{
    public class AnalysisScreen
    {
        struct ipNode
        {
            public string ip;
            public string port;
            public ipNode(string ip, string port)
            {
                this.ip = ip;
                this.port = port;
            }
        };
        int Counter = 0;
        Bitmap img;
        Random rand = new Random();
        const int white_color_min = 250;
        const int blank_space_count_min = 10;
        List<Point> blankList = new List<Point>();
        List<ipNode> ipList = new List<ipNode>();
        List<string> filterWordList = new List<string>();
        List<string> keyWordList = new List<string>();
        [DllImport("User32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        [DllImport("User32")]
        public extern static bool GetCursorPos(out Point p);
        [DllImport("USER32.DLL")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);  //导入模拟键盘的
        public enum MouseEventFlags       //鼠标按键的ASCLL码
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            Wheel = 0x0800,
            Absolute = 0x8000,
        }

        MouseEvent mouseEvent = new MouseEvent();
        public void Init()
        {
            InitIpList();
            InitFilterWord();
            InitSearchWord();
        }
        public void Index()
        { 
            Init(); 
            Main();
        }
        public void Main()
        {
            if (Counter >= ipList.Count)
            {
                return;
            }
            CloseCron();
            blankList.Clear();
            Thread.Sleep(1000);
            OpenCron();
            Thread.Sleep(3000);
            ClearCacheAndCookie();
            Thread.Sleep(2000);
            ChangeIpPort(this.ipList[Counter].ip, this.ipList[Counter].port);
            Thread.Sleep(1000);
            IntPutBaidu();
            Thread.Sleep(15000);
            InputSearchWord("药品说明书的英文翻译");
            //InputSearchWord("水果");
            Thread.Sleep(10000);
            Caputure();
            Thread.Sleep(2000);
            this.Counter++;
            Main();
        }
        public void Caputure()
        {
           
            //开始时窗体最小化，截屏后再还原
            Screen s = Screen.PrimaryScreen;
            Rectangle r = s.Bounds;
            int iWidth = r.Width;
            int iHeight = r.Height;
            //创建一个和屏幕一样大的bitmap
            img = new Bitmap(iWidth, iHeight);
            //从一个继承自image类的对象中创建Graphics对象
            Graphics g = Graphics.FromImage(img);
            //抓取全屏幕
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));
            //img.Save("D:\\img\\1.jpg");
            AnalysisPiex(new Rectangle(10, 145, 700, 700));
        }

        public Point AnalysisPiex(Rectangle r)
        {
            //img = (Bitmap)Bitmap.FromFile("D:\\img\\1.jpg", true);
            String str = "";
            bool blank_space = true;
            int blank_space_count = 0;
            int blank_space_start = 0;
            for (int y = r.Y; y < r.Height; y++)
            {
                blank_space = true;
                for (int x = r.X; x < r.Width; x++)
                {
                    Color c = img.GetPixel(x, y);
                    int cc = (int)((c.R + c.G + c.B) / 3);
                    if (cc < white_color_min)
                    {
                        blank_space = false;
                        break;
                    }
                }
                if (blank_space == true)
                {
                    if (blank_space_start == 0)
                    {
                        blank_space_start = y;
                    }
                    blank_space_count++;
                    if (y == img.Height - 1)
                    {
                        blankList.Add(new Point(blank_space_start, blank_space_count));
                    }
                }
                else
                {
                    if (blank_space_count > blank_space_count_min)
                    {
                        blankList.Add(new Point(blank_space_start, blank_space_count));
                    }
                    blank_space_count = 0;
                    blank_space_start = 0;
                }
            }
            for (int i = 1; i < blankList.Count; i++)
            {
                Point start = new Point(550 + rand.Next(50, 100), blankList[i].X + blankList[i].Y - 35 + rand.Next(5, 10));
                Point end = new Point(rand.Next(15, 30), blankList[i - 1].X + 25 + rand.Next(5, 10));
                string html = mouseEvent.CopyHtml(start, end);
                //str += blankList[i].X + ":" + blankList[i].Y + "\n";
                 if (html != "" && CheckClick(html))
                {
                    ClickKeyWord(new Point(end.X + rand.Next(20,80), end.Y + rand.Next(0,5)));
                    Thread.Sleep(rand.Next(8000,10000));
                    CloseNewWindow();
                    WriteTracker(DateTime.Now.ToString() + ":" + this.ipList[Counter].ip + ":" +this.Counter + "-----------------\n" + html + "\n");
                }
            }
            //MessageBox.Show(str);
            return new Point(0, 0);
        }
        public bool ChangeIpPort(string ip, string port)
        {
            mouseEvent.ClickPoint(new Point(1870, 40), false); //设置
            Thread.Sleep(2000);
            mouseEvent.ClickPoint(new Point(1800, 200), false); //选项
            Thread.Sleep(2000); 
            Point IpPointStart = new Point(1000,330);
            Point IpPointEnd = new Point(1000-200, 330);

            Point PortPointStart = new Point(1500, 330);
            Point PortPointEnd = new Point(1500 - 200, 330);
            mouseEvent.InputStrToPoint(IpPointStart, IpPointEnd, ip, false, false);
            mouseEvent.InputStrToPoint(PortPointStart, PortPointEnd, port, false, false);
            mouseEvent.ClickPoint(new Point(1350, 600), false); //保存
            return true;
        }
        public void OpenCron()
        {
            mouseEvent.ClickPoint(new Point(40, 40), false);
            Thread.Sleep(100);
            mouseEvent.ClickPoint(new Point(40, 40), false);
        }
        public void IntPutBaidu()
        {
            //mouseEvent.CopyStrToPoint(new Point(150, 40), new Point(200, 40), "www.baidu.com", false);
            mouseEvent.InputStrToPoint(new Point(150, 40), new Point(200, 40), "www.baidu.com ", false, false);
            Thread.Sleep(100);
            Point point =  mouseEvent.getPosition(new Point(rand.Next(400, 1200), rand.Next(20, 700)));
            mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), point.X, point.Y, 0, IntPtr.Zero);
            Thread.Sleep(100);
            keybd_event(13, 0, 0, 0);//Enter
            keybd_event(13, 0, 2, 0);//Enter
        }
        
       
        public void InputSearchWord(string str)
        {
            mouseEvent.InputStrToPoint(new Point(750 + rand.Next(0, 50), 250 + rand.Next(0, 10)),
                                       new Point(750 + rand.Next(0, 100), 250 + rand.Next(0, 10)),
                                       pinyin(str), true, true);
            Thread.Sleep(mouseEvent.SleepTime(100));
            keybd_event(13, 0, 0, 0);//Enter
            Thread.Sleep(mouseEvent.SleepTime(100));
            keybd_event(13, 0, 2, 0);//Enter
        }
        public bool CheckClick(string str)
        {
            if (str.IndexOf("推广") > 0 && str.IndexOf("365fanyi") >= 0)
            {
                //Random rand = new Random();
                //if (rand.Next(4) > 2)
                {
                    return true;
                }
                //else
                {
                //    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void ClickKeyWord(Point point)
        {
            Thread.Sleep(rand.Next(0, 2000));
            mouseEvent.ClickPoint(point, true);
        }
        public void CloseNewWindow()
        {
            mouseEvent.ClickPoint(new Point(570, 10),true);
        }
       public void ClearCacheAndCookie()
        {
            
            keybd_event(123, 0, 0, 0);//F12
            keybd_event(123, 0, 2, 0);//F12
            Thread.Sleep(1000);

            Point start = mouseEvent.getPosition(new Point(200, 700));
            Point cache = mouseEvent.getPosition(new Point(250, 820));
            Point cookie = mouseEvent.getPosition(new Point(250, 840));

            mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), start.X, start.Y, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.RightDown), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.RightUp), 0, 0, 0, IntPtr.Zero);

           Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), cache.X ,cache.Y, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);

           Thread.Sleep(500);
            keybd_event(13, 0, 0, 0);//Enter
            keybd_event(13, 0, 2, 0);//Enter
            Thread.Sleep(500);
            
            mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), start.X, start.Y , 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.RightDown), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.RightUp), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), cookie.X,cookie.Y , 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(500);
            keybd_event(13, 0, 0, 0);//Enter
            keybd_event(13, 0, 2, 0);//Enter
            
            Thread.Sleep(500);
            keybd_event(123, 0, 0, 0);//F12
            keybd_event(123, 0, 2, 0);//F12
        }
        public void CloseCron()
        {
            mouseEvent.ClickPoint(new Point(1900, 10),true);
            //切换输入法
            /*if (InputLanguage.CurrentInputLanguage.LayoutName == "中文(简体) - 搜狗拼音输入法")
            {
                keybd_event(16, 0, 0, 0);
                Thread.Sleep(50);
                keybd_event(17, 0, 0, 0);
                Thread.Sleep(700);
                keybd_event(16, 0, 2, 0);
                Thread.Sleep(50);
                keybd_event(17, 0, 2, 0);
                Thread.Sleep(50);
            }*/
        }
        public void WriteTracker(string str)
        {
            //实例化一个文件流--->与写入文件相关联
            FileStream fs = new FileStream("./trackerlog.txt", FileMode.Append);
            //实例化一个StreamWriter-->与fs相关联
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        public void InitIpList()
        {
            FileStream fin = File.Open("./iplist.txt", FileMode.Open);
            byte[] buffer = new byte[500000];
            fin.Read(buffer, 0, 499999);
            ASCIIEncoding encoding = new ASCIIEncoding();
            string iptext = encoding.GetString(buffer);
            string[] iplist1 = iptext.Split('\n');
            foreach (string str in iplist1)
            {
                string[] temp = str.Split(':');
                this.ipList.Add(new ipNode(temp[0], temp[1]));
            }
            fin.Close();
        }
        public void InitFilterWord()
        {
            this.filterWordList.Add("www.365fanyi.com");
            this.filterWordList.Add("365fanyi.com");
            this.filterWordList.Add("365fanyi");

        }
        public void InitSearchWord()
        {
            this.keyWordList.Add("翻译公司");
            this.keyWordList.Add("北京翻译公司");
            this.keyWordList.Add("英语翻译服务");
            this.keyWordList.Add("专业翻译");
            this.keyWordList.Add("英语翻译公司");
        }
        private bool isFilterWord(string str)
        {
            str = str.ToLower();
            foreach (string s in this.filterWordList)
            {
                if (str.Contains(s))
                    return true;
            }
            return false;
        }
        private string pinyin(string chinese)
        {
            string result = "";
            switch (chinese)
            {
                case "水果":
                    result = "shuiguo ";
                    break;
                case "药品说明书的英文翻译":
                    result = "yaop shuom shu2de yingw fanyi ";
                    break;
                default:
                    result = "weizhi ";
                    break;
            }
            return result;
        }
    }
}
