using middleware_d26.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

public class SomiodMessageHandler : DelegatingHandler
{
    private readonly IMqttClientFactory _mqttClientFactory; // Assuming you have an MQTT client factory

    public SomiodMessageHandler(IMqttClientFactory mqttClientFactory)
    {
        _mqttClientFactory = mqttClientFactory;
    }

    async protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Process the request message as needed
        Debug.WriteLine("Processing request");

        // Call the inner handler to continue processing the request
        var response = await base.SendAsync(request, cancellationToken);

        // Process the response message as needed
        Debug.WriteLine("Processing response");

        // Check if the request is a notification request
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

                default:
                    // Handle unknown discover type
                    break;
            }
        }

        // Check if the request is a subscription request
        if (request.Headers.TryGetValues("somiod-discover", out var subscribeValues) &&
            subscribeValues.FirstOrDefault() == "subscription")
        {
            // Logic to handle subscription creation
            var subscription = await request.Content.ReadAsAsync<Subscription>(); // Assuming Subscription class

            // Save the subscription details and set up notification mechanism
            SetupNotification(subscription);
        }

        // Return the modified response
        return response;
    }

    //private void SetupNotification(Subscription subscription)
    //{
    //    // Logic to set up notification mechanism based on the subscription details

    //    // For example, if MQTT
    //    var mqttClient = _mqttClientFactory.CreateMqttClient();
    //    mqttClient.ConnectAsync().Wait();

    //    // Subscribe to the relevant MQTT channel based on the subscription details
    //    var channelName = $"api/somiod/{subscription.Application}/{subscription.Container}";
    //    mqttClient.SubscribeAsync(channelName).Wait();

    //    // Handle incoming MQTT messages and send notifications
    //    mqttClient.UseApplicationMessageReceivedHandler(e =>
    //    {
    //        // Logic to handle incoming MQTT messages and send notifications
    //        var notificationContent = $"Event: {e.ApplicationMessage.Payload}, Data: {subscription.Data}";
    //        SendNotification(notificationContent, subscription.Endpoint);
    //    });

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
