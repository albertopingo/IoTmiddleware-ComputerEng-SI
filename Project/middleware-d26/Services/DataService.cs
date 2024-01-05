using middleware_d26.DataContext;
using middleware_d26.Models;
using middleware_d26.Models.DTOs;
using System;
using System.Linq;
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

            var data = new Data
            {
                Content = dataDTO.Content,
                Name = dataDTO.Name,
                Creation_Dt = DateTime.Now,
                Parent = parentContainer.Id
            };

            dbContext.DataRecords.Add(data);
            await dbContext.SaveChangesAsync();

            var subscription = dbContext.Subscriptions.FirstOrDefault(s => s.Parent == parentContainer.Id)
                ?? throw new Exception("Subscription not found");

            var topic = $"{applicationName}/{containerName}";

            using (var mqttService = new MqttService(subscription.Endpoint))
            {
                mqttService.PublishMessage(topic, dataDTO.Content);
            }
        }

        internal Task<object> GetData(string applicationName, string containerName, string dataName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                                                             c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Parent container not found");

            var data = dbContext.DataRecords.FirstOrDefault(d =>
                           d.Parent == parentContainer.Id && d.Content == dataName)
                ?? throw new Exception("Data not found");

            return Task.FromResult<object>(data);
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

            dbContext.DataRecords.Remove(data);
            await dbContext.SaveChangesAsync();
        }
    }
}
