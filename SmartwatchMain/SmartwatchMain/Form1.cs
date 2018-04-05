using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartwatchMain
{
    public partial class Form1 : Form
    {
        //Declaring fields
        //private int mode = 0;

        //Initializing objects
        Timer mainclock = new Timer();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //timer interval
            mainclock.Interval = 1000; // 1000ms == 1s

            mainclock.Tick += new EventHandler(this.t_Tick);

            mainclock.Start();
        }

        //Timer event handler
        private void t_Tick(object sender, EventArgs e)
        {
            //get current time on initialization aka default system time
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;

            //time
            string time = "";

            //nul toevoegen wanneer getallen kleiner zijn dan 0
            if (hh < 10)
            {
                time += "0" + hh;
            }
            else
            {
                time += hh;
            }
            time += ":";

            if(mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }

            //update the text label
            label1.Text = time;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Needs the time update shit
        }

        private void switchTab1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
    }
}