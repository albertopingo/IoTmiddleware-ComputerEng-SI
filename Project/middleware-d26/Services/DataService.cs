using middleware_d26.DataContext;
using middleware_d26.Models;
using middleware_d26.Models.DTOs;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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

        public async Task CreateData(string applicationName, string containerName, DataDTO dataDTO)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c => c.Name == containerName && c.Parent == parentApplication.Id)
                ?? throw new Exception("Parent container not found");

            if (dbContext.DataRecords.Any(d =>
                           d.Parent == parentContainer.Id && d.Name == dataDTO.Name))
            {
                throw new Exception("Data already exists");
            }

            var data = new Data
            {
                Content = dataDTO.Content,
                Name = dataDTO.Name,
                Creation_Dt = DateTime.Now,
                Parent = parentContainer.Id
            };
            
            // Send notification on Creation
            var subscriptions = dbContext.Subscriptions.Where(s => s.Parent == parentContainer.Id && s.Event.ToLower() == "creation");
            var topic = $"{applicationName}/{containerName}";
            //sub count
            Debug.WriteLine($"Subscriptions count: {subscriptions.Count()}");

            foreach (var subscription in subscriptions)
            {

                using (var mqttService = new MqttService(subscription.Endpoint))
                {
                    mqttService.PublishMessage(topic, data, "creation");
                }
            }

            dbContext.DataRecords.Add(data);
            await dbContext.SaveChangesAsync();
        }

        internal Data GetData(string applicationName, string containerName, string dataName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                                                             c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Parent container not found");

            var data = dbContext.DataRecords.FirstOrDefault(d =>
                           d.Parent == parentContainer.Id && d.Name == dataName)
                ?? throw new Exception("Data not found");

            return data;
        }

        public async Task DeleteData(string applicationName, string containerName, string dataName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c => c.Name == containerName && c.Parent == parentApplication.Id)
                ?? throw new Exception("Parent container not found");

            var data = dbContext.DataRecords.FirstOrDefault(d =>
                                      d.Parent == parentContainer.Id && d.Name == dataName)
                ?? throw new Exception("Data not found");

            // Send notification on Deletion
            var subscriptions = dbContext.Subscriptions.Where(s => s.Parent == parentContainer.Id && s.Event.ToLower() == "deletion");
            var topic = $"{applicationName}/{containerName}";
            Debug.WriteLine($"Subscriptions count: {subscriptions.Count()}");

            foreach (var subscription in subscriptions)
            {
                using (var mqttService = new MqttService(subscription.Endpoint))
                {
                    mqttService.PublishMessage(topic, data, "deletion");
                }
            }

            dbContext.DataRecords.Remove(data);
            await dbContext.SaveChangesAsync();
        }
    }
}
