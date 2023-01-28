using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Tasks28012023
{
    public partial class Tasks : Form
    {
        public Tasks()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //StreamWriter sw = new StreamWriter("Text.txt");

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        sw.WriteLine($"{i}");

            //    }
            //    sw.Close();
            //});

            for (int i = 0; i <= 10; i++)
            {
                label1.Text = await Counter(i);
            }
        }

        public Task<string> Counter(int num)
        {
            return Task.Run(() => { Thread.Sleep(1000); return num.ToString(); });
        }

     
       

       
        

        private async void button2_Click_1(object sender, EventArgs e)
        {

            string text = "", temp = "";
            int min = 0, hour = 1;
            bool start = true;
            DateTime dateTime = DateTime.Now;
            hour += dateTime.Hour;
            min = dateTime.Minute;
            while (hour != dateTime.Hour && min == dateTime.Minute)
            {
                temp = await YNetData(start) + "\n";
                if (temp != text)
                {
                    text = dateTime.Hour + ":" + dateTime.Minute + " - " + temp;
                    richTextBox1.Text += text;
                }
                start = false;
            }
        }
        public Task<string> YNetData(bool start)
        {
            return Task.Run(async () =>
            {
                const int firstTitle = 3;
                int i = 0;
                string text = "";
                using (var client = new HttpClient())
                {
                    // We'll use the GetAsync method to send 
                    // a GET request to the specified URL
                    var response = await client.GetAsync("http://www.ynet.co.il/Integration/StoryRss2.xml");

                    // If the response is successful, we'll
                    // interpret the response as XML
                    if (response.IsSuccessStatusCode)
                    {
                        var xml = await response.Content.ReadAsStringAsync();

                        // We can then use the LINQ to XML API to query the XML
                        var doc = XDocument.Parse(xml);

                        // Let's query the XML to get all of the <title> elements
                        var titles = from el in doc.Descendants("title")
                                     select el.Value;

                        // And finally, we'll print out the titles
                        foreach (var title in titles)
                        {
                            i++;
                            if (i == firstTitle)
                            {
                                text = title;
                            }
                        }
                    }
                }
                if (start != true)
                    // this sleep is for 2 minutes
                    Thread.Sleep(2000 * 60);
                return text;
            });
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            label2.Text = await YNetSize();
        }
        public Task<string> YNetSize()
        {
            return Task.Run(async () =>
            {
                int numOfBytes = 0;
                using (var client = new HttpClient())
                {
                    // We'll use the GetAsync method to send 
                    // a GET request to the specified URL
                    var response = await client.GetAsync("https://www.ynet.co.il/home/0,7340,L-8,00.html");

                    // If the response is successful, we'll
                    // interpret the response as XML
                    if (response.IsSuccessStatusCode)
                    {
                        var xml = await response.Content.ReadAsStringAsync();
                        numOfBytes = System.Text.ASCIIEncoding.Unicode.GetByteCount(xml);
                    }

                }
                return numOfBytes.ToString();
            });

        }
    }
}
