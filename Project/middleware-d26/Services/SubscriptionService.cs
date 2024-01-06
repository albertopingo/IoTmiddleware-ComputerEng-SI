using middleware_d26.DataContext;
using middleware_d26.Models;
using middleware_d26.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace middleware_d26.Services
{
    public class SubscriptionService
    {
        private readonly MiddlewareDbContext dbContext;

        public SubscriptionService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateSubscription(string applicationName, string containerName, SubscriptionDTO subscriptionDTO)
        {
            //validate subscriptionDTO xml
            if (subscriptionDTO.Name == null || subscriptionDTO.Endpoint == null || subscriptionDTO.Event == null)
            {
                throw new Exception("SubscriptionDTO is not valid");
            }

            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                                      c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Parent container not found");

            if (dbContext.Subscriptions.Any(s =>
                s.Parent == parentContainer.Id && s.Name == subscriptionDTO.Name))
            {
                throw new Exception("Subscription already exists");
            }

            var subscription = new Subscription
            {
                Name = subscriptionDTO.Name,
                Creation_Dt = DateTime.Now,
                Endpoint = subscriptionDTO.Endpoint,
                Event = subscriptionDTO.Event,
                Parent = parentContainer.Id
            };
            dbContext.Subscriptions.Add(subscription);
            await dbContext.SaveChangesAsync();
        }


        internal Subscription GetSubscription(string applicationName, string containerName, string subscriptionName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                                                 c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Parent container not found");

            // get subscription with name
            var subscription = dbContext.Subscriptions.FirstOrDefault(s =>
                           s.Parent == parentContainer.Id && s.Name == subscriptionName)
                ?? throw new Exception("Subscription not found");

            return subscription;
        }


        public async Task DeleteSubscription(string applicationName, string containerName, string subscriptionName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                                                 c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Parent container not found");

            var subscription = dbContext.Subscriptions.FirstOrDefault(s =>
                           s.Parent == parentContainer.Id && s.Name == subscriptionName)
                ?? throw new Exception("Subscription not found");

            dbContext.Subscriptions.Remove(subscription);
            await dbContext.SaveChangesAsync();
        }

        /* public void NotifySubscriptions(string containerName, string eventData, EventType eventType)
         {
             try
             {
                 // Logic to check subscriptions for the corresponding container
                 var subscriptions = dbContext.Subscriptions
                     .Where(s => s.Parent == containerName && (s.Event == "both" || (eventType == EventType.Creation && s.Event == "creation") || (eventType == EventType.Deletion && s.Event == "deletion")))
                     .ToList();

                 // Logic to send notifications
                 foreach (var subscription in subscriptions)
                 {
                     SendNotification(eventData, eventType, subscription);
                 }
             }
             catch (Exception ex)
             {
                 // Log the exception
                 Console.WriteLine($"Exception in NotifySubscriptions: {ex.Message}");
                 throw; // Re-throw the exception to maintain the original exception details
             }
         }

         private void SendNotification(string eventData, EventType eventType, Subscription subscription)
         {
             // Logic to send notifications to the specified endpoint

             if (subscription.Endpoint.StartsWith("http"))
             {
                 // For HTTP
                 using (var httpClient = new HttpClient())
                 {
                     var notificationContent = CreateNotificationContent(eventData, eventType);
                     httpClient.PostAsync(subscription.Endpoint, new StringContent(notificationContent, Encoding.UTF8, "application/json")).Wait();
                 }
             }
             else if (subscription.Endpoint.StartsWith("mqtt"))
             {
                 // For MQTT
                 using (var mqttClient = new MqttClient(subscription.Endpoint))
                 {
                     mqttClient.Connect(Guid.NewGuid().ToString());

                     var channelName = $"api/somiod/{subscription.Application}/{subscription.Container}";
                     var notificationContent = CreateNotificationContent(eventData, eventType);
                     mqttClient.Publish(channelName, Encoding.UTF8.GetBytes(notificationContent), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                 }
             }
         }

         private string CreateNotificationContent(string eventData, EventType eventType)
         {
             // Create the content of the notification including data resource and event type
             return $"{{ \"event\": \"{eventType}\", \"data\": {eventData} }}";
         }
     */
    }
}