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
    class MouseEvent
    {
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
        Random rand = new Random();
        public void ClickPoint(Point end, bool mannue)
        {
            if (mannue)
            {
                SmoothMove(end);
            }
            else
            {
                Point position = getPosition(end);
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), position.X, position.Y, 0, IntPtr.Zero);
            }
            Thread.Sleep(200);
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(100);
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);
        }
        public void SmoothMove(Point end)
        {
            Point start;
            GetCursorPos(out start);
            //start = getPosition(start);
            ManueMouseMove manueMouseMove = new ManueMouseMove(start, end);
            while (manueMouseMove.next())
            {
                Point nowPosition = getPosition(manueMouseMove.nowPosition);
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), nowPosition.X, nowPosition.Y, 0, IntPtr.Zero);
                Thread.Sleep(10);
            }
        }
       /* public bool CopyStrToPoint(Point start, Point end, string str, bool mannue)
        { 
            if (!mannue)
            {
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), (int)(start.X * 65535 / 1920), (int)(start.Y * 65535 / 1080), 0, IntPtr.Zero);
            }
            else
            {
                SmoothMove(start);
            }
            Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(500);
            if (!mannue)
            {
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), (int)((end.X) * 65535 / 1920), (int)(end.Y * 65535 / 1080), 0, IntPtr.Zero);
            }
            else
            {
                SmoothMove(end);
            }
            Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);
            try
            {
                Thread.Sleep(500);
                Clipboard.Clear();
                Thread.Sleep(500);
                Clipboard.SetText(str);
                Thread.Sleep(1000);
                IDataObject iData = Clipboard.GetDataObject();//获取剪贴板数据。
                Thread.Sleep(1000);
                //检测数据是否是可以使用的格式,即文本格式
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    Thread.Sleep(500);
                    //是可以使用的格式,就在textbox2中显示剪切板中的内容
                    keybd_event(17, 0, 0, 0); //Control 
                    Thread.Sleep(500);
                    keybd_event(86, 0, 0, 0);//v
                    Thread.Sleep(500);
                    keybd_event(86, 0, 2, 0);//v
                    Thread.Sleep(500);
                    keybd_event(17, 0, 2, 0); //Control
                    return true;
                }
                else
                {
                    MessageBox.Show("clipboard no data");
                    Thread.Sleep(1000);
                    keybd_event(13, 0, 2, 0); //Enter
                    keybd_event(13, 0, 2, 0); //Enter
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/

        public string CopyHtml(Point start, Point end)
        {
            try
            {
                Thread.Sleep(500); //等待写入粘贴板
                Clipboard.Clear();//清空粘贴板方法。
                Thread.Sleep(500); //等待写入粘贴板
            }
            catch (Exception ex)
            {
            }

            SmoothMove(start);
         
            Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(500);
            SmoothMove(end);
       
            Thread.Sleep(500);
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(500);
            keybd_event(17, 0, 0, 0); //Control 
            Thread.Sleep(500);
            keybd_event(67, 0, 0, 0);//c
            Thread.Sleep(500);
            keybd_event(67, 0, 2, 0);//c
            Thread.Sleep(500);
            keybd_event(17, 0, 2, 0); //Control
            Thread.Sleep(1000); //等待写入粘贴板
            try
            {
                IDataObject iData = Clipboard.GetDataObject();//获取剪贴板数据。
                Thread.Sleep(1000); //等待写入粘贴板
                //检测数据是否是可以使用的格式,即文本格式
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    Thread.Sleep(500); //等待写入粘贴板
                    //是可以使用的格式,就在textbox2中显示剪切板中的内容
                    string str = (String)iData.GetData(DataFormats.Text);
                    return str;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("error");
                return "";
            }
            return "";

        }
        public void InputStrToPoint(Point start, Point end, string str, bool mannue, bool chinese)
        {
            
            if (!mannue)
            {
                start = getPosition(start); 
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), start.X, start.Y, 0, IntPtr.Zero);
            }
            else
            {
                SmoothMove(start);
            }
            Thread.Sleep(SleepTime(100));
            mouse_event((int)(MouseEventFlags.LeftDown), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(SleepTime(100));
            if (!mannue)
            {
                end = getPosition(end);
                mouse_event((int)(MouseEventFlags.Move | MouseEventFlags.Absolute), end.X, end.Y, 0, IntPtr.Zero);
            }
            else
            {
                SmoothMove(end);
            }
            Thread.Sleep(SleepTime(100));
            mouse_event((int)(MouseEventFlags.LeftUp), 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(SleepTime(100));
            if (chinese)
            {
                InputChinese(str);
                return;
            }
            str = str.ToUpper();
            char[] letters = str.ToCharArray();
            byte code ;
            foreach(char letter in letters)
            {
                if (letter == '.')
                {
                    code = 110; 
                }  
                else
                {
                    code = (byte)letter;
                }
                keybd_event(code, 0, 0, 0); 
                Thread.Sleep(SleepTime(100));
                keybd_event(code, 0, 2, 0);
            }  
        }
        public int SleepTime(int time)
        {

            return (int)(time * (1 + (rand.Next(5, 20) / 100)));
        }

        public void InputChinese(string str)
        {
            //切换输入法
            /*if (InputLanguage.CurrentInputLanguage.LayoutName != "中文(简体) - 搜狗拼音输入法")
            {
                keybd_event(16, 0, 0, 0);
                Thread.Sleep(SleepTime(50));
                keybd_event(17, 0, 0, 0);
                Thread.Sleep(SleepTime(700));
                keybd_event(16, 0, 2, 0);
                Thread.Sleep(SleepTime(50));
                keybd_event(17, 0, 2, 0);
                Thread.Sleep(SleepTime(50));
            }*/
            Thread.Sleep(SleepTime(1000));
            str = str.ToUpper();
            char[] letters = str.ToCharArray();
            byte code;
            foreach (char letter in letters)
            {
                code = (byte)letter;
                keybd_event(code, 0, 0, 0);
                Thread.Sleep(SleepTime(500));
                keybd_event(code, 0, 2, 0);
            }
            Thread.Sleep(SleepTime(1000));
        }
        public Point getPosition(Point position)
        {
            return new Point((int)(position.X * 65535 / 1920), (int)(position.Y * 65535 / 1080));
        }
    }
}
