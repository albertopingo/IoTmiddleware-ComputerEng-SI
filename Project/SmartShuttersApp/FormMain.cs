using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


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
                    //needs to be <EntityRequest xmlns="Middleware-d26">
                    
                    string xmlContent = $@"<EntityRequest xmlns=""Middleware-d26""><res_type>application</res_type><name>SmartShutters</name></EntityRequest>";

                    //string xmlContent = "<EntityRequest><res_type>application</res_type><name>SmartShutters</name></EntityRequest>";
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
            string entityRequestBody = $@"<EntityRequest xmlns=""Middleware-d26""><res_type>container</res_type><name>{containerName}</name></EntityRequest>";


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
            string name = textBoxSubscriptionName.Text;
            string endpoint = textBoxEndPoint.Text;
            string selectedEvent = comboBoxEvent.SelectedItem.ToString();

            if(string.IsNullOrWhiteSpace(containerName) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(endpoint) && string.IsNullOrWhiteSpace(selectedEvent))
            {
                MessageBox.Show("Erro. Todos os campos tem que estar preenchidos");
            }

            // Create the EntityRequest body with the dynamic values
            string entityRequestBody = $@"<EntityRequest xmlns=""Middleware-d26""><res_type>subscription</res_type><subscription><name>{name}</name><event>{selectedEvent}</event><endpoint>{endpoint}</endpoint></subscription></EntityRequest>";
            
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

        [Obsolete]
        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            MqttClient mClient = new MqttClient(IPAddress.Parse(textBoxEndPoint.Text));
            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                Console.WriteLine("Error connecting to message broker...");
                return;
            }
            //Specify events we are interest on
            //New Msg Arrived
            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            mClient.MqttMsgSubscribed += client_MqttMsgSubscribed;
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            MessageBox.Show("Shutter = " + Encoding.UTF8.GetString(e.Message));
        }
        void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
        MessageBox.Show("SUBSCRIBED WITH SUCCESS");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
