using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Mime;

namespace SmartShuttersApp
{
    public partial class FormMain : Form
    {
        string baseURL = @"http://localhost:65252/api/somiod"; //TODO: needs to be updated!
        public FormMain()
        {
            InitializeComponent();
            CreateApp();
        }
        private void CreateApp()
        {
            string name = "SmartShutters";
            string apiUrlWithQuery = $"{baseURL}/{name}";

            // Make the HTTP GET request
            using (HttpClient client = new HttpClient())
            {
                // Specify the Accept header for XML
                client.DefaultRequestHeaders.Add("Accept", "application/xml");

                HttpResponseMessage response = client.GetAsync(apiUrlWithQuery).Result;

                // Check if the request was successful
                if (!response.IsSuccessStatusCode)
                {
                    string xmlContent = "<EntityRequest><res_type>application</res_type><name>SmartShutters</name></EntityRequest>";
                    StringContent content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                    // Make the HTTP POST request
                    using (HttpClient client1 = new HttpClient())
                    {
                        HttpResponseMessage response1 = client1.PostAsync(baseURL, content).Result;

                        // Check if the request was successful
                        if (response1.IsSuccessStatusCode)
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
        private void buttonCreateContainer_Click(object sender, EventArgs e)
        {
            // Get the name from the TextBox
            string containerName = textBoxContainerName.Text;

            // Create the EntityRequest body with the dynamic container name
            string entityRequestBody = $"<EntityRequest><res_type>container</res_type><name>{containerName}</name></EntityRequest>";

            // Make the HTTP POST request
            using (HttpClient client = new HttpClient())
            {

                // Create the request content
                StringContent content = new StringContent(entityRequestBody, Encoding.UTF8, "application/xml");

                // Specify the API endpoint
                string apiUrl = $"{baseURL}/SmartShutters";

                // Make the HTTP POST request
                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Container '{containerName}' created successfully!");
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }


        }

        private void buttonCreateSubscription_Click(object sender, EventArgs e)
        {
            string containerName = textBoxContainerName.Text;
            string endpoint = textBoxEndPoint.Text;
            string selectedEvent = comboBoxEvent.SelectedItem.ToString();

            // Create the EntityRequest body with the dynamic values
            string entityRequestBody = $@"
                <EntityRequest>
                    <res_type>subscription</res_type>
                    <subscription>
                        <name>NewSubscription</name>
                        <event>{selectedEvent}</event>
                        <endpoint>{endpoint}</endpoint>
                    </subscription>
                </EntityRequest>";

            // Make the HTTP POST request
            using (HttpClient client = new HttpClient())
            {
             
                // Create the request content
                StringContent content = new StringContent(entityRequestBody, Encoding.UTF8, "application/xml");

                // Specify the API endpoint
                string apiUrl = $"{baseURL}/App1/{containerName}";

                // Make the HTTP POST request
                HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Subscription for event '{selectedEvent}' created successfully!");
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        private void textBoxContainerName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
