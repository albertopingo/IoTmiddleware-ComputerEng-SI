using middleware_d26.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace middleware_d26.Services
{
    public class DiscoverService
    {
        private readonly MiddlewareDbContext dbContext;

        public DiscoverService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    

    public IEnumerable<string> GetApplications()
    {
        // Logic to return all applications (names)
        return dbContext.Applications.Select(a => a.Name).ToList();
    }

        public IEnumerable<string> GetContainers(string applicationName)
        {
            try
            {
                // Logic to return all containers (names) under the specified parent
                var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

                if (parentApplication == null)
                {
                    throw new Exception($"Parent application not found for name: {applicationName}");
                }

                return dbContext.Containers
                    .Where(c => c.Parent == parentApplication.Id)
                    .Select(c => c.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception in GetContainers: {ex.Message}");
                throw; // Re-throw the exception to maintain the original exception details
            }
        }


        public IEnumerable<int> GetDataRecords(string parentName)
        {
            // Logic to return all data records (names) under the specified parent
            // (You can customize this based on your data model)
            return dbContext.DataRecords
                .Where(d => dbContext.Containers.Any(c => c.Id == d.Parent && c.Name == parentName))
                .Select(d => d.Id)
                .ToList();
        }

    }
}