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
                Endpoint = subscriptionDTO.Endpoint,
                Event = subscriptionDTO.Event,
                Parent = parentContainer.Id
            };
            dbContext.Subscriptions.Add(subscription);
            await dbContext.SaveChangesAsync();
        }


        internal Task<object> GetSubscription(string applicationName, string containerName, string subscriptionName)
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

            return Task.FromResult<object>(subscription);
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
    }
}
