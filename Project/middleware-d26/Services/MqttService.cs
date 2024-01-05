using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace middleware_d26.Services
{
    public class MqttService : IDisposable
    {
        private readonly MqttClient mqttClient;

        public MqttService(string brokerIpAddress)
        {
            mqttClient = new MqttClient(brokerIpAddress);
            Connect();
        }

        private void Connect()
        {
            mqttClient.Connect(Guid.NewGuid().ToString());
            if (!mqttClient.IsConnected)
            {
                Console.WriteLine("Error connecting to the MQTT broker...");
            }
        }

        public void PublishMessage(string topic, string message)
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
            else
            {
                // Reconnect and try again
                Connect();
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                }
                else
                {
                    Console.WriteLine("Failed to publish message. MQTT client not connected.");
                }
            }
        }

        public void Disconnect()
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
            }
        }

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}
