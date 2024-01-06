using middleware_d26.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Xml.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace middleware_d26.Services
{
    public class MqttService : IDisposable
    {
        private readonly MqttClient mqttClient;

        public MqttService(string brokerIpAddress)
        {
            Debug.WriteLine($"Connecting to MQTT broker at {brokerIpAddress}...");
            mqttClient = new MqttClient(brokerIpAddress);
            Connect();
        }

        private void Connect()
        {
            try
            {
                mqttClient.Connect(Guid.NewGuid().ToString());
                if (!mqttClient.IsConnected)
                {
                    Debug.WriteLine("Error connecting to the MQTT broker...");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error connecting to the MQTT broker: {ex.Message}");
            }
        }

        public void PublishMessage(string topic, Data data, string eventType)
        {
            Debug.WriteLine($"Publishing message to topic {topic}: {data}");
            var xmlDoc = new XDocument(
                new XElement("Message",
                    new XElement("EventType", eventType),
                    new XElement("DataResource",
                        new XElement("Content", data.Content),
                        new XElement("Name", data.Name),
                        new XElement("Creation_Dt", data.Creation_Dt),
                        new XElement("Parent", data.Parent)
                    )
                )
            );

            var messagePayload = xmlDoc.ToString(SaveOptions.DisableFormatting);
            try
            {                
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish(topic, Encoding.UTF8.GetBytes(messagePayload), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                    xmlDoc = null;
                }
                else
                {
                    // Reconnect and try again
                    Connect();
                    if (mqttClient.IsConnected)
                    {
                        mqttClient.Publish(topic, Encoding.UTF8.GetBytes(messagePayload), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                        xmlDoc = null;
                    }
                    else
                    {
                        Debug.WriteLine("Failed to publish message. MQTT client not connected.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error publishing message via MQTT: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
                Debug.WriteLine("Disconnected from MQTT broker.");
            }
        }

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
            Debug.WriteLine("Disposed MQTT service.");
        }
    }
}
