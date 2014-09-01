using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using wordcountV2;
using System.IO;
using System.Reflection;

using MSWord = Microsoft.Office.Interop.Word;

namespace wordcountV2
{
    class wordlist
    {
        public static int file_num = 0;
        public static int per_height = 30;
        public static int now_height = 20;
        //所有控件应该添加进入的panel
        public static Control.ControlCollection control_panel = null; 
        //显示总字数的label
        public static Label total_words_label = null;
        //显示总字符数的label
        public static Label total_character_label = null;
        //显示总中朝字符数的label
        public static Label total_CBS_label = null;
        //控件的toolTips
        public static ToolTip main_ToolTip = null;
        private file_info file = null;
        private Label Words_label = null;
        private Label Character_label = null;
        private Label DBCS_label = null;
        private bool already_add_flag = false; 

        public wordlist(file_info file)
        {
            this.file = file;
        }

        public void add_control()
        {
            if (this.already_add_flag) return;
            Random rand = new Random();
            if (wordlist.control_panel == null)
            {
                MessageBox.Show("这是一个可以避免的错误，请联系胡光~~~~");
                return;
            }
            int index = wordlist.file_num++;
            string button_name = "button_" + index;
            string delete_button_name = "delbutton_" + index;
            string checkbox_name = "checkbox_" + index;
            string wordslabel_name = "wordslabel_" + index;
            string charalabel_name = "charalabel_"+index;
            string CBSlabel_name = "CBSlabel_" + index;
            string opendirbutton_name = "opendir_" + index;
            int control_x = 10; 
            /*
            初始化所有控件，并且把控件添加进事先设置好的panel当中
             */

            //初始化checkbox
            CheckBox chooce_checkbox = new CheckBox();
            int chooce_checkbox_size = 100;
            chooce_checkbox.Name = checkbox_name;
            chooce_checkbox.Text = this.file.File_name;
            chooce_checkbox.CheckedChanged += new EventHandler(wordlist.checkBox_CheckedChanged);
            chooce_checkbox.Location = new Point(control_x,wordlist.now_height);
            chooce_checkbox.Size = new Size(chooce_checkbox_size, wordlist.per_height / 3 * 2);
            wordlist.main_ToolTip.SetToolTip(chooce_checkbox, file.File_name);
            control_x += chooce_checkbox_size + 20;
            //初始化words label，用来显示字数
            Label wordslabel = new Label();
            int wordslabel_size = 50;
            wordslabel.Name = wordslabel_name;
            wordslabel.Text = "0";
            wordslabel.TextAlign = ContentAlignment.MiddleCenter;
            wordslabel.Location = new Point(control_x, wordlist.now_height);
            wordslabel.Size = new Size(wordslabel_size, wordlist.per_height / 3 * 2);
            this.Words_label = wordslabel;
            control_x += wordslabel_size + 20;
            //初始化character label，用来显示字符数（不含空格）
            Label charalabel = new Label();
            int charalabel_size = 50;
            charalabel.Name = charalabel_name;
            charalabel.Text = "0";
            charalabel.TextAlign = ContentAlignment.MiddleCenter;
            charalabel.Location = new Point(control_x, wordlist.now_height);
            charalabel.Size = new Size(charalabel_size, wordlist.per_height / 3 * 2);
            this.Character_label = charalabel;
            control_x += charalabel_size + 20;
            //初始化cbs label，用来显示中朝字符数
            Label cbslabel = new Label();
            int cbslabel_size = 50;
            cbslabel.Name = CBSlabel_name;
            cbslabel.Text = "0";
            cbslabel.TextAlign = ContentAlignment.MiddleCenter;
            cbslabel.Location = new Point(control_x, wordlist.now_height);
            cbslabel.Size = new Size(cbslabel_size, wordlist.per_height / 3 * 2);
            this.DBCS_label = cbslabel;
            control_x += cbslabel_size + 20;
            //初始化button
            Button calc_button = new Button();
            int calc_button_size = 50;
            calc_button.Name = button_name;
            calc_button.Text = "计 算";
            calc_button.Location = new Point(control_x, wordlist.now_height);
            calc_button.Size = new Size(calc_button_size, wordlist.per_height / 3 * 2);
            calc_button.Click += new EventHandler(this.calcbutton_Click);
            control_x += calc_button_size + 20;
            //初始化删除按钮
            Button del_button = new Button();
            int del_button_size = 50;
            del_button.Name = delete_button_name;
            del_button.Text = "删 除";
            del_button.Location = new Point(control_x, wordlist.now_height);
            del_button.Size = new Size(del_button_size, wordlist.per_height / 3 * 2);
            del_button.Click += new System.EventHandler(wordlist.delbutton_Click);
            control_x += del_button_size + 20;
            //初始化打开文件夹按钮
            Button opendir_button = new Button();
            int opendir_button_size = 100;
            opendir_button.Name = opendirbutton_name;
            opendir_button.Text = "打开图片文件夹";
            opendir_button.Location = new Point(control_x, wordlist.now_height);
            opendir_button.Size = new Size(opendir_button_size, wordlist.per_height / 3 * 2);
            opendir_button.Click += new EventHandler(this.opendir_Click);
            control_x += opendir_button_size + 20;
            //把所有控件添加到事先预定好的panel当中
            wordlist.control_panel.Add(chooce_checkbox);
            wordlist.control_panel.Add(wordslabel);
            wordlist.control_panel.Add(charalabel);
            wordlist.control_panel.Add(cbslabel);
            wordlist.control_panel.Add(calc_button);
            wordlist.control_panel.Add(del_button);
            wordlist.control_panel.Add(opendir_button);
            //全局高度增加
            wordlist.now_height += wordlist.per_height;
            this.already_add_flag = true;
        }


        public static void delbutton_Click(object sender, EventArgs e)
        {
            Button temp_button = (Button)sender;
            string button_name = temp_button.Name;
            string index = button_name.Split('_')[1];
            wordlist.control_panel.RemoveByKey("button_" + index);
            CheckBox temp_checkbox = (CheckBox)wordlist.control_panel.Find("checkbox_" + index,true)[0];
            temp_checkbox.Checked = false;
            wordlist.control_panel.RemoveByKey("checkbox_" + index);
            wordlist.control_panel.RemoveByKey("wordslabel_" + index);
            wordlist.control_panel.RemoveByKey("charalabel_" + index);
            wordlist.control_panel.RemoveByKey("CBSlabel_" + index);
            wordlist.control_panel.RemoveByKey("delbutton_" + index);
            wordlist.control_panel.RemoveByKey("opendir_" + index);
            foreach (Control c in wordlist.control_panel)
            {
                string index2 = c.Name.Split('_')[1];
                int a1, a2;
                int.TryParse(index, out a1);
                int.TryParse(index2, out a2);
                if (a1 < a2)
                {
                    c.Location = new Point(c.Location.X, c.Location.Y - wordlist.per_height);
                }
            }
            wordlist.now_height -= wordlist.per_height;
        }

        private static void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox temp_checkbox = (CheckBox)sender;
            string index = temp_checkbox.Name.Split('_')[1];
            Label words_label = (Label)wordlist.control_panel.Find("wordslabel_" + index, true)[0];
            Label character_label = (Label)wordlist.control_panel.Find("charalabel_" + index, true)[0];
            Label cbs_label = (Label)wordlist.control_panel.Find("CBSlabel_" + index, true)[0];
            int words1, words2, character1, character2, cbs1, cbs2;
            int.TryParse(words_label.Text, out words2);
            int.TryParse(wordlist.total_words_label.Text, out words1);
            int.TryParse(character_label.Text, out character2);
            int.TryParse(wordlist.total_character_label.Text, out character1);
            int.TryParse(cbs_label.Text, out cbs2);
            int.TryParse(wordlist.total_CBS_label.Text, out cbs1);
            if (temp_checkbox.Checked)
            {
                wordlist.total_CBS_label.Text = (cbs1 + cbs2).ToString();
                wordlist.total_character_label.Text = (character1 + character2).ToString();
                wordlist.total_words_label.Text = (words1 + words2).ToString();
            }
            else
            {
                wordlist.total_CBS_label.Text = (cbs1 - cbs2).ToString();
                wordlist.total_character_label.Text = (character1 - character2).ToString();
                wordlist.total_words_label.Text = (words1 - words2).ToString();
            }
        }

        private void opendir_Click(object sender, EventArgs e)
        {
            if (this.file.PicDirExist())
            {
                string path = this.file.Pic_path;
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }


        public void calcbutton_Click(object sender, EventArgs e)
        {
            Button temp_button = (Button)sender;
            string index = temp_button.Name.Split('_')[1];
            CheckBox temp_checkbox = (CheckBox)wordlist.control_panel.Find("checkbox_" + index, true)[0];
            bool temp_flag = temp_checkbox.Checked;
            temp_checkbox.Checked = false;
            this.DOCWordCount();
            temp_checkbox.Checked = temp_flag;
        }

        private void DOCWordCount()
        {
            MSWord.ApplicationClass wordApp = null; //Word应用程序变量
            MSWord.Document wordDoc = null; //Word文档变量
            object path = this.file.File_path + "\\" + this.file.File_name;
            Object Nothing = Missing.Value;
            try
            {
                if (!this.file.file_exist()) return;
                wordApp = new MSWord.ApplicationClass();

                //由于使用的是COM库，因此有许多变量需要用Missing.Value代替

                wordDoc = wordApp.Documents.Open(ref path, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                MSWord.Dialog wdlg = wordApp.Dialogs[Microsoft.Office.Interop.Word.WdWordDialog.wdDialogToolsWordCount];
                wdlg.Execute();
                System.Type dlgType = typeof(MSWord.Dialog);

                //字数
                object oo = dlgType.InvokeMember("Words",
                    System.Reflection.BindingFlags.GetProperty |
                    System.Reflection.BindingFlags.Public |
                   System.Reflection.BindingFlags.Instance,
                   null,
                   wdlg,
                   null);
                int words = Convert.ToInt32(oo.ToString().Replace(",", ""));

                // 字符数（不计空格）
                oo = dlgType.InvokeMember("Characters",
                System.Reflection.BindingFlags.GetProperty |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance,
                null,
                wdlg,
                null);
                int characters = Convert.ToInt32(oo.ToString().Replace(",", ""));

                int charactersIncludingSpaces = Convert.ToInt32(oo.ToString().Replace(",", ""));

                // 中朝字符数
                oo = dlgType.InvokeMember("DBCs",
                System.Reflection.BindingFlags.GetProperty |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance,
                null,
                wdlg,
                null);

                int dbcs = Convert.ToInt32(oo.ToString().Replace(",", ""));
                this.Words_label.Text = words.ToString();
                this.Character_label.Text = characters.ToString();
                this.DBCS_label.Text = dbcs.ToString();
                /*
                 提取WORD中的图片到一个文件夹中
                 */
                this.ExtractImageFromDOC(wordDoc);
                wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                //删除一些无关紧要的垃圾文件
                object path2 = (object)(this.file.File_fullname);
                string path3 = path2 + ".files";
                path2 = path2 + ".htm";
                DeleteFile(path3 + "\\" + "colorschememapping.xml");
                DeleteFile(path3 + "\\" + "filelist.xml");
                DeleteFile(path3 + "\\" + "header.htm");
                DeleteFile(path3 + "\\" + "themedata.thmx");
                DeleteFile(path2.ToString());
            }
            catch (Exception e)
            {
            }
            finally
            {
                wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            }
        }

        //从WORD中提取图片
        private void ExtractImageFromDOC(MSWord.Document wordDoc)
        {
            Object Nothing = Missing.Value;
            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            if (wordDoc == null) return;

            //WdSaveFormat为Word 2007文档的保存格式
            object format = MSWord.WdSaveFormat.wdFormatHTML;
            //将wordDoc文档对象的内容保存为XML文档

            object path2 = (object)(this.file.File_fullname);
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
        }

        private void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
