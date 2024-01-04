using System;
using System.Collections.Generic;
using System.Linq;
using middleware_d26.Models;
using middleware_d26.DataContext;
using System.Threading.Tasks;

namespace middleware_d26.Services
{
    public class DataService
    {
        private readonly MiddlewareDbContext dbContext;

        public DataService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateData(string applicationName, string containerName, string content)
        {

            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c => c.Name == containerName && c.Parent == parentApplication.Id)
                ?? throw new Exception("Parent container not found");

            var data = new Data
            {
                Content = content,
                Creation_Dt = DateTime.Now,
                Parent = parentContainer.Id
            };

            dbContext.DataRecords.Add(data);
            await dbContext.SaveChangesAsync();

            // Fetch the container's subscription endpoint
            var subscription = dbContext.Subscriptions.FirstOrDefault(s => s.Parent == parentContainer.Id) 
                ?? throw new Exception("Subscription not found");

            var topic = $"{applicationName}/{containerName}";

            using (var mqttService = new MqttService(subscription.Endpoint))
            {
                mqttService.PublishMessage(topic, content);
            }
        }

        public async Task DeleteData(string applicationName, string containerName, int dataId)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c => c.Name == containerName && c.Parent == parentApplication.Id)
                ?? throw new Exception("Parent container not found");

            var data = dbContext.DataRecords.FirstOrDefault(d => d.Parent == parentContainer.Id && d.Id == dataId)
                ?? throw new Exception("Data not found");

            dbContext.DataRecords.Remove(data);
            await dbContext.SaveChangesAsync();
        }
    }
}
