using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

using MSWord = Microsoft.Office.Interop.Word;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace DOC2XML
{
    public partial class Form1 : Form
    {

        private double time_length = 0;
        DirectoryInfo The_Folder_Input = null;
        DirectoryInfo The_Folder_Output = null;
        MSWord.ApplicationClass wordApp = null; //Word应用程序变量
        MSWord.Document wordDoc = null; //Word文档变量

        MSExcel.ApplicationClass excelApp = null; //Excel应用程序变量
        MSExcel.Workbook excelWorkbook = null; //ExcelWorkBook变量
        MSExcel.Worksheet excelWorksheet = null; //ExcelWorksheet变量，级别小于WorkBook变量


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
                if (file_path == this.textBox2.Text)
                {
                    MessageBox.Show("监控文件夹和输出文件夹请选择不同目录！");
                    return;
                }
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
                if (file_path == this.textBox1.Text)
                {
                    MessageBox.Show("监控文件夹和输出文件夹请选择不同目录！");
                    return;
                }
                this.textBox2.Text = file_path;
                this.The_Folder_Output = new DirectoryInfo(file_path);
            }
        }

        //负责监控的函数
        private void timer1_tick(object sender, EventArgs e)
        {
            time_length += (this.timer1.Interval / 1000.0);
            this.label5.Text = time_length.ToString();

            if (this.The_Folder_Input == null || this.The_Folder_Output == null) return;

            this.timer1.Enabled = false;

            foreach (FileInfo nextFile in The_Folder_Input.GetFiles())
            {
                #region DOC文档的相关处理
                if (nextFile.Extension == ".doc" || nextFile.Extension == ".DOC"
                    || nextFile.Extension == ".docx" || nextFile.Extension == ".DOCX")
                {
                    this.DOCConvertToXml(nextFile.FullName,nextFile.Name);

                    DeleteFile(this.textBox2.Text + "\\" + nextFile.Name);

                    this.ExtractImageFromDOC(nextFile.FullName, nextFile.Name);//从WORD文档中提取图片

                    File.Move(nextFile.FullName, this.textBox2.Text + "\\" + nextFile.Name);
                    File.Delete(nextFile.FullName);
                }
                #endregion

                #region EXCEL表格的处理
                if (nextFile.Extension == ".xlsx" || nextFile.Extension == ".XLSX" ||
                    nextFile.Extension == ".xls" || nextFile.Extension == ".XLS")
                {
                    this.XLSXConvertToDOC(nextFile.FullName, nextFile.Name);

                    DeleteFile(this.textBox2.Text + "\\" + nextFile.Name);

                    File.Move(nextFile.FullName, this.textBox2.Text + "\\" + nextFile.Name);
                    File.Delete(nextFile.FullName);
                }
                #endregion

                #region jpg图片转换成DOC
                if (nextFile.Extension == ".jpg" || nextFile.Extension == ".JPG")
                {
                    this.JPGConvertToDOC(nextFile.FullName,nextFile.Name);

                    DeleteFile(this.textBox2.Text + "\\" + nextFile.Name);

                    File.Move(nextFile.FullName, this.textBox2.Text + "\\" + nextFile.Name);
                    File.Delete(nextFile.FullName);                    
                }
                #endregion

                #region PDF 转换成 TXT

                if (nextFile.Extension == ".pdf" || nextFile.Extension == ".PDF")
                {
                    this.PDFConvertToTXT(nextFile.FullName, nextFile.Name);

                    DeleteFile(this.textBox2.Text + "\\" + nextFile.Name);

                    File.Move(nextFile.FullName, this.textBox2.Text + "\\" + nextFile.Name);
                    File.Delete(nextFile.FullName); 
                }
                #endregion

            }

            this.timer1.Enabled = true;
        }

        private void ExtractImageFromDOC(object path, string name)
        {
            wordApp = new MSWord.ApplicationClass();

            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替

            if (!File.Exists((string)path)) return;
            name = name.Substring(0, name.LastIndexOf('.'));
            wordDoc = wordApp.Documents.Open(ref path, ref Nothing, ref Nothing, ref Nothing,
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            wordDoc.Content.Font.Spacing = 0;
            wordDoc.Content.Font.Position = 0;
            wordDoc.Content.Font.Scaling = 100;
            wordDoc.Content.Font.Kerning = 1;

            //WdSaveFormat为Word 2007文档的保存格式
            object format = MSWord.WdSaveFormat.wdFormatHTML;
            //将wordDoc文档对象的内容保存为XML文档
            
            object path2 = (object)(this.textBox2.Text + "\\" + name);
            string path3 = path2 + ".files";
            //如果已存在，则删除

            if (Directory.Exists(path3))
            {
                Directory.Delete(path3, true);
            }

            DeleteFile(path2.ToString() + ".htm");

            path2 = path2 + ".htm";
            wordDoc.SaveAs(ref path2, ref format, ref Nothing, ref Nothing, ref Nothing,
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);

            wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);

            //删除一些无关紧要的垃圾文件
            DeleteFile(path3 + "\\" + "colorschememapping.xml");
            DeleteFile(path3 + "\\" + "filelist.xml");
            DeleteFile(path3 + "\\" + "header.htm");
            DeleteFile(path3 + "\\" + "themedata.thmx");
            DeleteFile(path2.ToString());

        }

        private void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        private void PDFConvertToTXT(object path, string name)
        {
            if (!File.Exists((string)path)) return;
            name = name.Substring(0, name.LastIndexOf('.'));
            string pathText = this.textBox2.Text + "\\" + name + ".txt";
            string startAppName = "cmd.exe";
            string startpath = string.Format(
                "java -jar \"{0}\\pdfbox-app-1.6.0.jar\" ExtractText -encoding UTF-8 \"{1}\" \"{2}\"",
                Application.StartupPath,(string)path,pathText);

            Process p = new Process();
            p.StartInfo.FileName = startAppName;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.StandardInput.WriteLine(startpath);
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();
            p.Close();
            
        }

        private void JPGConvertToDOC(object path, string name)
        {
            if (!File.Exists((string)path)) return;
            name = name.Substring(0, name.LastIndexOf('.'));
            MODI.Document doc = new MODI.Document(); 
            doc.Create((string)path);
            MODI.Image image;
            MODI.Layout layout; 
            string ret = null;
            /*
             1 : 英语
             2 : 中文
             3 : 日语
             4 : 法语
             5 : 德语
             6 : other 按照英语的识别方式来识别
             * 
             * 在系统不能完全支持日语和其他语言的前提下，统计字数的方法为 ： 
             * 识别日语用中文识别
             * 其余语言用英语
             */
            try
            {
                switch (name[0])
                {
                    case '1': doc.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true); break;
                    case '2': doc.OCR(MODI.MiLANGUAGES.miLANG_CHINESE_SIMPLIFIED, true, true); break;
                    case '3': doc.OCR(MODI.MiLANGUAGES.miLANG_CHINESE_SIMPLIFIED, true, true); break;
                    case '4': doc.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true); break;
                    case '5': doc.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true); break;
                    default : doc.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true); break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            for (int i = 0; i < doc.Images.Count;i++) 
            { 
                image = (MODI.Image)doc.Images[i]; 
                layout = image.Layout;
                ret += layout.Text; 
            }
            string pathText = this.textBox2.Text + "\\" + name + "(" + ret.Length + ")" + ".txt";

            FileStream fp = new FileStream(pathText, FileMode.OpenOrCreate);
            byte[] bytebuffer = System.Text.Encoding.Default.GetBytes(ret);
            fp.Write(bytebuffer, 0,bytebuffer.Length);
            fp.Close();

        }



        private void XLSXConvertToDOC(object path, string name)
        {
            //前期准备工作！

            this.excelApp = new MSExcel.ApplicationClass();
            this.wordApp = new MSWord.ApplicationClass();

            name = name.Substring(0, name.LastIndexOf('.'));

            if (!File.Exists((string)path)) return;

            object wordpath = (object)(this.textBox2.Text + "\\" + name + ".docx");

            this.excelWorkbook = excelApp.Workbooks.Open(path.ToString(), Nothing, Nothing,  Nothing,
                  Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                  Nothing, Nothing, Nothing, Nothing, Nothing);
            


            //将每个sheet里面的内容都提出来  并且写到Word里面，最终对Word文件进行字数统计
            string Value = "";
            
            for (int SheetCount = 1; SheetCount <= this.excelWorkbook.Worksheets.Count; SheetCount++)
            {
                this.excelWorksheet = (MSExcel.Worksheet)this.excelWorkbook.Worksheets[SheetCount];
                this.excelWorksheet.Activate();
                int WorkRows = this.excelWorksheet.UsedRange.Rows.Count;
                int WorkColumns = this.excelWorksheet.UsedRange.Columns.Count;
                Value = "";
                if (WorkColumns <= 2 && WorkRows <= 2)
                {
                    for (int i = 1; i <= WorkRows; i++)
                        for (int j = 1; j <= WorkColumns; j++)
                        {
                            Value = Value + ((MSExcel.Range)this.excelWorksheet.Cells[i, j]).Text.ToString();
                            if (Value != "") break;
                        }
                }
                else
                {
                    Value = "right";
                }
                if (Value != "")
                {
                    //新建一个新的WORD文档
                    this.wordDoc = this.wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                    //将excel里面的东西复制出来
                    ((MSExcel.Worksheet)this.excelWorkbook.Worksheets[SheetCount]).UsedRange.Select();
                    ((MSExcel.Worksheet)this.excelWorkbook.Worksheets[SheetCount]).UsedRange.Copy(Nothing);
                    //将复制的东西粘贴到word里面
                    this.wordDoc.Content.Paste();
                    //将word文档保存一下
                    object wordformat = MSWord.WdSaveFormat.wdFormatDocumentDefault;
                    wordpath = (object)(this.textBox2.Text + "\\" + name + "" + SheetCount + "(" + this.excelWorksheet.Name + ").docx");
                    
                    if (File.Exists(wordpath.ToString()))
                    {
                        File.Delete(wordpath.ToString());
                    }
                    
                    this.wordDoc.SaveAs(ref wordpath, ref wordformat, ref Nothing, ref Nothing, ref Nothing,
                        ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                        ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                    //关闭word文档
                    this.wordDoc.Close(ref Nothing,ref Nothing,ref Nothing);
                }
            }

            //善后工作
            this.excelWorkbook.Save();
            this.excelWorkbook.Close(Nothing, Nothing, Nothing);
            this.wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            this.excelApp.Quit(); 
        }


        private void DOCConvertToXml(object path,string name)
        {
            try
            {
                wordApp = new MSWord.ApplicationClass();

                //由于使用的是COM库，因此有许多变量需要用Missing.Value代替

                if (!File.Exists((string)path)) return;

                wordDoc = wordApp.Documents.Open(ref path, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                MSWord.Dialog wdlg = wordApp.Dialogs[Microsoft.Office.Interop.Word.WdWordDialog.wdDialogToolsWordCount];
                wdlg.Execute();
                System.Type dlgType = typeof(MSWord.Dialog);
                if (this.checkBox1.Checked)
                {
                    object timeout = 0;
                    wdlg.Display(ref timeout);
                }


                object oo = dlgType.InvokeMember("Words",
                    System.Reflection.BindingFlags.GetProperty |
                    System.Reflection.BindingFlags.Public |
                   System.Reflection.BindingFlags.Instance,
                   null,
                   wdlg,
                   null);

                wordDoc.Content.Font.Spacing = 0;
                wordDoc.Content.Font.Position = 0;
                wordDoc.Content.Font.Scaling = 100;
                wordDoc.Content.Font.Kerning = 1;

                //WdSaveFormat为Word 2007文档的保存格式
                object format = MSWord.WdSaveFormat.wdFormatXML;
                //将wordDoc文档对象的内容保存为XML文档
                name = name.Substring(0, name.LastIndexOf('.')) + "(" + oo.ToString().Replace(",", "") + ")";
                object path2 = (object)(this.textBox2.Text + "\\" + name + ".xml");
                //如果已存在，则删除
                DeleteFile(path2.ToString());

                wordDoc.SaveAs(ref path2, ref format, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                string textForWordCount = this.textBox2.Text + "\\" + name + "_WC.txt";

                DeleteFile(textForWordCount);

                FileStream textFileStream = new FileStream(textForWordCount, FileMode.OpenOrCreate);
                byte[] byteFortext = System.Text.Encoding.Default.GetBytes(wordDoc.Content.Text);

                textFileStream.Write(byteFortext, 0, byteFortext.Length);
                textFileStream.Close();

                wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
            }
            catch (Exception e)
            {
                wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            }
        }

    }
}
