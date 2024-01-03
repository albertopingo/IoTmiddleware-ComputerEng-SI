using middleware_d26.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using System;
using middleware_d26.Services;
using System.Net;
using System.Net.Http.Formatting;

public class SomiodMessageHandler : DelegatingHandler
{
    //private readonly IMqttClientFactory _mqttClientFactory; // Assuming you have an MQTT client factory

    //public SomiodMessageHandler(IMqttClientFactory mqttClientFactory)
    //{
    //    _mqttClientFactory = mqttClientFactory;
    //}
    private readonly DiscoverService discoverService;
    public SomiodMessageHandler(DiscoverService discoverService)
    {
        this.discoverService = discoverService ?? throw new ArgumentNullException(nameof(discoverService));
    }

    // ...

    async protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.TryGetValues("somiod-discover", out var discoverValues))
        {
            var discoverType = discoverValues.FirstOrDefault();

            switch (discoverType)
            {
                case "application":
                    var applications = discoverService.GetApplications();
                    return CreateResponse(HttpStatusCode.OK, applications);

                case "container":
                    // Extract application name from route data
                    var routeData = request.GetRouteData();
                    var applicationName = routeData.Values["id"] as string;

                    if (string.IsNullOrEmpty(applicationName))
                    {
                        // Handle the case where applicationName is not present in the route data
                        return CreateResponse(HttpStatusCode.BadRequest, "Application name not specified in the route.");
                    }

                    var containers = discoverService.GetContainers(applicationName);
                    return CreateResponse(HttpStatusCode.OK, containers);

                case "data":
                    // Extract parent name from route data
                    var routeDataData = request.GetRouteData();
                    var parentNameData = routeDataData.Values["id"] as string;

                    if (string.IsNullOrEmpty(parentNameData))
                    {
                        // Handle the case where parentName is not present in the route data
                        return CreateResponse(HttpStatusCode.BadRequest, "Parent name not specified in the route.");
                    }

                    var dataRecords = discoverService.GetDataRecords(parentNameData);
                    return CreateResponse(HttpStatusCode.OK, dataRecords);

                case "subscription":
                    //var subscription = await request.Content.ReadAsAsync<Subscription>();
                    // Logic to handle subscription creation
                    // SetupNotification(subscription);
                    return CreateResponse(HttpStatusCode.OK, "Subscription created successfully");

                default:
                    return CreateResponse(HttpStatusCode.BadRequest, "Unknown discover type");
            }
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

    private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new ObjectContent(content.GetType(), content, new XmlMediaTypeFormatter())
        };
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
