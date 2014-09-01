using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace wordcountV2
{
    public partial class Form1 : Form
    {

        private int index = 0;
        public Form1()
        {
            InitializeComponent();
            wordlist.control_panel = this.panel1.Controls;
            this.textBox1.Enabled = false;
            this.label4.Text = "0";
            this.label5.Text = "0";
            this.label6.Text = "0";
            wordlist.total_words_label = this.label4;
            wordlist.total_character_label = this.label5;
            wordlist.total_CBS_label = this.label6;
            wordlist.main_ToolTip = this.toolTip1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            file_info file = new file_info();
            file.File_name = "hello" + index;
            file.File_path = "C:\\lamp";
            wordlist temp_wordlist = new wordlist(file);
            temp_wordlist.add_control();
            this.index++;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*
             扫描选定的文件夹下的所有WORD文件，并且添加，列出相应的条目
             */
            if (this.textBox1.Text.Trim() != "")
            {
                SearchAllFile(new DirectoryInfo(this.textBox1.Text));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void SearchAllFile(DirectoryInfo The_Folder)
        {
            foreach (FileInfo f in The_Folder.GetFiles())
            {
                if (f.Extension.ToLower() == ".doc" || f.Extension.ToLower() == ".docx")
                {
                    file_info temp_file = new file_info();
                    temp_file.File_name = f.Name;
                    temp_file.File_path = The_Folder.FullName;
                    wordlist temp_wordlist = new wordlist(temp_file);
                    temp_wordlist.add_control();
                }
            }
            foreach (DirectoryInfo d in The_Folder.GetDirectories())
            {
                this.SearchAllFile(d);
            }
        }
    }
}
