using middleware_d26.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using System;

public class SomiodMessageHandler : DelegatingHandler
{
    //private readonly IMqttClientFactory _mqttClientFactory; // Assuming you have an MQTT client factory

    //public SomiodMessageHandler(IMqttClientFactory mqttClientFactory)
    //{
    //    _mqttClientFactory = mqttClientFactory;
    //}

    async protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Check if the request is a discover request
        if (request.Headers.TryGetValues("somiod-discover", out var discoverValues))
        {
            var discoverType = discoverValues.FirstOrDefault();

            switch (discoverType)
            {
                case "application":
                    // Logic to return all applications (names)
                    break;

                case "container":
                    // Logic to return all containers (names) under the specified parent
                    break;

                case "data":
                    // Logic to return all data records (names) under the specified parent
                    break;

                case "subscription":
                    // Logic to handle subscription creation
                    var subscription = await request.Content.ReadAsAsync<Subscription>(); // Assuming Subscription class
                    //SetupNotification(subscription);
                    break;

                default:
                    // Handle unknown discover type
                    break;
            }

            // Return a response for discover requests
            return new HttpResponseMessage();
        }
        else
        {
            // Process the regular HTTP request
            Debug.WriteLine("Processing regular request");

            // Call the inner handler to continue processing the request
            var response = await base.SendAsync(request, cancellationToken);

            // Process the response message as needed
            Debug.WriteLine("Processing response");

            // Return the modified response
            return response;
        }
    }

    //private void SetupNotification(Subscription subscription)
    //{
    //    // Logic to set up notification mechanism based on the subscription details

    //    // For example, if MQTT
    //    using (var mqttClient = _mqttClientFactory.CreateMqttClient())
    //    {
    //        mqttClient.Connect(Guid.NewGuid().ToString());

    //        // Subscribe to the relevant MQTT channel based on the subscription details
    //        var channelName = $"api/somiod/{subscription.Application}/{subscription.Container}";
    //        mqttClient.Subscribe(new[] { channelName }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    //        // Handle incoming MQTT messages and send notifications
    //        mqttClient.MqttMsgPublishReceived += (sender, e) =>
    //        {
    //            // Logic to handle incoming MQTT messages and send notifications
    //            var notificationContent = $"Event: {Encoding.UTF8.GetString(e.Message)}, Data: {subscription.Data}";
    //            SendNotification(notificationContent, subscription.Endpoint);
    //        };
    //    }

    //    // Dispose of the MQTT client appropriately (not shown in this example)
    //}

    //private void SendNotification(string content, string endpoint)
    //{
    //    // Logic to send notifications to the specified endpoint

    //    // For example, if HTTP
    //    using (var httpClient = new HttpClient())
    //    {
    //        httpClient.PostAsync(endpoint, new StringContent(content)).Wait();
    //    }
    //}
}
