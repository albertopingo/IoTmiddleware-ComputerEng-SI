using middleware_d26.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace middleware_d26.Services
{
    public class DiscoverService
    {
        private readonly MiddlewareDbContext dbContext;

        public DiscoverService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public IEnumerable<string> DiscoverApplications()
        {
            return dbContext.Applications.Select(a => a.Name).ToList();
        }

        public IEnumerable<string> DiscoverContainers(string applicationName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception($"Parent application not found for name: {applicationName}");

            return dbContext.Containers
                .Where(c => c.Parent == parentApplication.Id)
                .Select(c => c.Name)
                .ToList();
        }

        public IEnumerable<string> DiscoverDataRecords(string applicationName, string containerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception($"Parent application not found for name: {applicationName}");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception($"Parent container not found for name: {containerName}");

            return dbContext.DataRecords
                .Where(d => d.Parent == parentContainer.Id)
                .Select(d => d.Name)
                .ToList();
        }

        public IEnumerable<string> DiscoverSubscriptions(string applicationName, string containerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception($"Parent application not found for name: {applicationName}");

            var parentContainer = dbContext.Containers.FirstOrDefault(c =>
                           c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception($"Parent container not found for name: {containerName}");

            return dbContext.Subscriptions
                .Where(s => s.Parent == parentContainer.Id)
                .Select(s => s.Name)
                .ToList();
        }
    }
}