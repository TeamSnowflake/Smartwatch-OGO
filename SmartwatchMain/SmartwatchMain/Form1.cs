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
        private int mode = 0;

        //Initializing objects
        Timer t = new Timer();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //timer interval
            t.Interval = 1000; // 1000ms == 1s

            t.Tick += new EventHandler(this.t_Tick);

            t.Start();
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

            //update text label
            label1.Text = time;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}