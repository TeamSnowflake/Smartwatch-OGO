using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Net;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace WebServiceApp
{
    /// <summary>
    /// Summary description for ServiceFile
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceFile : System.Web.Services.WebService
    {
        SqlConnection myConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        public string getURL()
        {
            string location = "Amsterdam";
            string unitType = getUnitType();
            string APICode = "6cdbc1630349494c7f9866084b85c694";
            string cmdString = "USE WeatherApp; SELECT * FROM settings WHERE Setting_ID = 1";
            //full link: http://api.openweathermap.org/data/2.5/weather?q=Emmen&APPID=6cdbc1630349494c7f9866084b85c694&mode=xml&units=metric&lang=en

            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdString, myConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    location = reader["City"].ToString();
                }
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            string url = String.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}&mode=xml&units={2}&lang=en", location, APICode, unitType);
            return url;
        }

        [WebMethod]
        public string GetLocation()
        {
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(getURL());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("current");
                string toReturn = node["city"].GetAttribute("name");
                return toReturn;
            }
        }

        [WebMethod]
        public string GetTemp()
        {
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(getURL());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("current");
                string toReturn = node["temperature"].GetAttribute("value");
                return toReturn;
            }
        }

        [WebMethod]
        public string GetWind()
        {
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(getURL());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("current/wind");
                string direction = node["direction"].GetAttribute("code");
                string name = node["speed"].GetAttribute("name");
                string toReturn = direction + ", " + name;
                return toReturn;
            }
        }

        public string getUnitType()
        {
            string unitType = "Metric";
            string cmdString = "USE WeatherApp; SELECT * FROM settings WHERE Setting_ID = 1";
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand(cmdString, myConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    unitType = reader["Unit_Type"].ToString();
                }
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return unitType;
        }

        public string GetWeatherIcon()
        {
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(getURL());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("current");
                string iconString = node["weather"].GetAttribute("icon");
                string toReturn = String.Format("http://openweathermap.org/img/w/{0}.png", iconString);
                return toReturn;
            }
        }

        public string GetWeatherValue()
        {
            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(getURL());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNode node = doc.SelectSingleNode("current");
                string toReturn = node["weather"].GetAttribute("value");
                return toReturn;
            }
        }

        [WebMethod]
        public List<string> GetArray()
        {
            string location = GetLocation();
            string temp = GetTemp();
            string wind = GetWind();
            string unitType = getUnitType();
            string iconLink = GetWeatherIcon();
            string weatherValue = GetWeatherValue();
            //string[] array = new string[] { location, temp, wind };
            //return array;

            List<string> list = new List<string>();
            list.Add(location);
            list.Add(temp);
            list.Add(wind);
            list.Add(unitType);
            list.Add(weatherValue);
            list.Add(iconLink);

            string[] array = new string[4] { location, temp, wind, unitType };

            return list;
        }
    }
}
