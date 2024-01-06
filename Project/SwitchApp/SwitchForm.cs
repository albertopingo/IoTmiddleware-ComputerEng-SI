using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SwitchApp
{
    public partial class SwitchForm : Form
    {
        string baseUrl = "http://localhost:65252/api/somiod";
        string appName = "SmartShutters";

        public SwitchForm()
        {
            CreateApplication();
            InitializeComponent();

            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseUrl}/{appName}";
                client.DefaultRequestHeaders.Add("Accept", "application/xml");
                client.DefaultRequestHeaders.Add("somiod-discover", "container");
                HttpResponseMessage response = client.GetAsync(url).Result;

                XmlDocument xmlDocument = new XmlDocument();
                string xml = response.Content.ReadAsStringAsync().Result;
                xmlDocument.LoadXml(xml);

                XmlNodeList stringNodes = xmlDocument.SelectNodes("/*/*");
                if (stringNodes.Count > 0) {
                    foreach (XmlNode stringNode in stringNodes)
                    {
                        comboBoxContainers.Items.Add(stringNode.InnerText);
                    }
                    comboBoxContainers.SelectedIndex = 0;
                }
            }
        }

        private void CreateApplication()
        {
            string appName = "Switch";
            string apiUrlWithQuery = $"{baseUrl}/{appName}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/xml");

                HttpResponseMessage response = client.GetAsync(apiUrlWithQuery).Result;
                // if not success (meaning application doesn't exist yet)
                if (!response.IsSuccessStatusCode)
                {
                    string xmlContent = $"<EntityRequest><res_type>application</res_type><name>{appName}</name></EntityRequest>";
                    StringContent content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                    using (HttpClient client1 = new HttpClient())
                    {
                        HttpResponseMessage response1 = client1.PostAsync(baseUrl, content).Result;

                        if (response1.IsSuccessStatusCode)
                        {
                            //MessageBox.Show("Request successful!");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                }
            }
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            Send("on");
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            Send("off");
        }

        private void Send(string data)
        {
            string xml = "" +
                "<EntityRequest>" +
                "<res_type>data</res_type>" +
                "<data>" +
                $"<content>{data}</content>" +
                $"<name>{textBoxDataName.Text}</name>" +
                "</data>" +
                "</EntityRequest>";
            StringContent content = new StringContent(xml, Encoding.UTF8, "application/xml");

            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseUrl}/{appName}/{comboBoxContainers.Text}";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Request successful!");
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
