using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewBaiduClick
{
    public partial class Form1 : Form
    {
        AnalysisScreen screen = new AnalysisScreen();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            screen.Index();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
