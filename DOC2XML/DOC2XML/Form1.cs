using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using MSWord = Microsoft.Office.Interop.Word;

namespace DOC2XML
{
    public partial class Form1 : Form
    {

        private double time_length = 0;
        DirectoryInfo The_Folder_Input = null;
        DirectoryInfo The_Folder_Output = null;
        MSWord.ApplicationClass wordApp = null;                    //Word应用程序变量
        MSWord.Document wordDoc = null;                   //Word文档变量
        Object Nothing = Missing.Value;

        public Form1()
        {
            InitializeComponent();
            Reboot(); 
        }

        private void Reboot()
        {
            this.label2.Text = "未监控";
            this.timer1.Interval = 1000;
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.timer1.Enabled = false;
            this.timer1.Tick += new EventHandler(timer1_tick);
            this.label5.Text = time_length.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择要监控的文件夹！");
                return;
            }

            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请选择要输出的文件夹！");
                return;
            }
            
            if (this.textBox1.Text != "" && this.textBox2.Text != "")
            {
                this.label2.Text = "监控中......";
                this.timer1.Enabled = true;


            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.label2.Text = "未监控";
            this.timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string file_path;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                file_path = this.folderBrowserDialog1.SelectedPath;
                this.textBox1.Text = file_path;
                this.The_Folder_Input = new DirectoryInfo(file_path);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string file_path;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                file_path = this.folderBrowserDialog1.SelectedPath;
                this.textBox2.Text = file_path;
                this.The_Folder_Output = new DirectoryInfo(file_path);
            }
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            time_length += (this.timer1.Interval / 1000.0);
            this.label5.Text = time_length.ToString();

            if (this.The_Folder_Input == null || this.The_Folder_Output == null) return;

            this.timer1.Enabled = false;

            foreach (FileInfo nextFile in The_Folder_Input.GetFiles())
            {
                if (nextFile.Extension == ".doc" || nextFile.Extension == ".DOC"
                    || nextFile.Extension == ".docx" || nextFile.Extension == ".DOCX")
                {
                    this.ConvertToXml(nextFile.FullName,nextFile.Name);

                    if (File.Exists(this.textBox2.Text + "\\" + nextFile.Name))
                    {
                        File.Delete(this.textBox2.Text + "\\" + nextFile.Name);
                    }

                    File.Move(nextFile.FullName, this.textBox2.Text + "\\" + nextFile.Name);
                    File.Delete(nextFile.FullName);
                }
            }

            this.timer1.Enabled = true;

        }

        private void ConvertToXml(object path,string name)
        {
            wordApp = new MSWord.ApplicationClass();

            //如果已存在，则删除
            if (File.Exists(this.textBox2.Text + "\\" + name + ".xml"))
            {
                File.Delete(this.textBox2.Text + "\\" + name + ".xml");
            }
            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替

            if (!File.Exists((string)path)) return;

            wordDoc = wordApp.Documents.Open(ref path, ref Nothing, ref Nothing, ref Nothing, 
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, 
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            wordDoc.Content.Font.Spacing = 0;
            wordDoc.Content.Font.Position = 0;
            wordDoc.Content.Font.Scaling = 100;
            wordDoc.Content.Font.Kerning = 1;
            //WdSaveFormat为Word 2007文档的保存格式
            object format = MSWord.WdSaveFormat.wdFormatXML;
            //将wordDoc文档对象的内容保存为XML文档

            object path2 = (object)(this.textBox2.Text + "\\" + name + ".xml");

            wordDoc.SaveAs(ref path2, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            
            wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);

            wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
        }

    }
}
