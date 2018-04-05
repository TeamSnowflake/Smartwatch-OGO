using System;
using System.Xml;
using System.Net;
using System.Windows.Forms;

namespace SmartwatchMain
{
    public partial class Form1 : Form
    {
        //Declaring fields
        int hh = DateTime.Now.Hour; //setting default system time
        int mm = DateTime.Now.Minute; //Idem
        int ss = DateTime.Now.Second; //Idem
        int timemode = 0; //Mode 0 means default system time and mode 1 means a custom time
        int timepress = 0; //0 means that only the hours will be edited and 1 means that the minutes will be edited
        double temp = 0.00;
        string location = "Amsterdam"; //Setting the default location
        int unitsetting = 0; //Default unitsetting (0) is used for the metric system, unitsetting (1) is used for the imperial system
        string unittype = "° Celsius";

        //Initializing objects
        Timer mainclock = new Timer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Timer interval for every 1 second (1000ms)
            mainclock.Interval = 1000;

            mainclock.Tick += new EventHandler(this.t_Tick);

            mainclock.Start();

            weatherUpdate();
        }

        private async void weatherUpdate()
        {
            textBox1.Text = location;

            string weburl = "http://api.openweathermap.org/data/2.5/weather?q=" + location + "&APPID=4e515071564d7c30d6a0107caade3f8a&mode=xml"; //Emmen needs to be replaced with location

            var xml = await new WebClient().DownloadStringTaskAsync(new Uri(weburl));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string szTemp = doc.DocumentElement.SelectSingleNode("temperature").Attributes["value"].Value;
            string humi = doc.DocumentElement.SelectSingleNode("humidity").Attributes["value"].Value;

            //Control structure for the temp conversion
            if (unitsetting == 0)
            {
                unittype = "° Celcius";
                temp = double.Parse(szTemp) - 273.16;
            }
            else if (unitsetting == 1) //Imperial system
            {
                unittype = "° Fahrenheit";
                temp = double.Parse(szTemp) * 9/5 - 459.67;
            }

            //Update the labels
            label2.Text = "Temperature: " + temp.ToString("N2") + unittype;
            label3.Text = "Humidity: " + humi + "%";
            label6.Text = "Current location: " + location;
        }

        private void ckb_Click(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            foreach (Control c in Controls)
            {
                CheckBox checkBox = c as CheckBox;
                if (checkBox != null)
                {
                    if (!checkBox.Equals(activeCheckBox))
                    { checkBox.Checked = !activeCheckBox.Checked; }
                    else
                    { checkBox.Checked = true; }
                }
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (timemode == 1) //if the time had changed to mode 1 (which means it is a custom time) it will still be updated every minute
            {
                ss = DateTime.Now.Second;
                if (ss > 58)
                {
                    mm = mm + 1;
                    if (mm > 58)
                    {
                        hh = hh + 1;
                        if (hh > 22)
                        {
                            hh = 0;
                        }
                    }
                }
            }
            else //if the time mode is on mode 0 then the clock will update every second from the system timer
            {
                hh = DateTime.Now.Hour; 
                mm = DateTime.Now.Minute; 
            }

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
            //Control structure to check in which time "edit" mode we are, 1 = incrementhours, 2 = increment minutes and with 0 we need to switch tabs
                if (timepress == 1)
                {
                    //Set the clock 00 instead of 24 after it strikes 23
                    if (hh > 22)
                    {
                        hh = 0; 
                    }
                    else
                    {
                        hh = hh + 1; //Increment the hours
                    }
                }
                else if (timepress == 2)
                {
                    //Set the clock to 00 instead of 60 after it strikes 59
                    if (mm > 58)
                    {
                        mm = 0; 
                    }
                    else
                    {
                        mm = mm + 1; //Increment the minutes
                    }
                }
                else if (timepress == 0)
                {
                    //Tabswitcher
                    tabControl1.SelectedTab = tabPage2;
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           //Setting the time mode to a custom time
            timemode = 1;

            //Set timepress when button 2 is clicked in the first tab (DisplayTime)
            timepress = timepress + 1;
            //Control structure to check when when the butten has been pressed 3 times. then it needs to be resetted
            if (timepress >= 3)
            {
                timepress = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Tabswitcher
            tabControl1.SelectedTab = tabPage3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Tabswitcher
            tabControl1.SelectedTab = tabPage2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Tabswitcher
            tabControl1.SelectedTab = tabPage1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Needs to save the location and unit type and at the end return to the displayweather tab.

            location = textBox1.Text;

            weatherUpdate();

            //Controlstructure to check for the unittype
            if (radioButton1.Checked == true)
            {
                //System is imperial
                unitsetting = 1;
            }
            else if (radioButton2.Checked == true)
            {
                //System is metric
                unitsetting = 0;
            }

            tabControl1.SelectedTab = tabPage2;
        }
    }
}