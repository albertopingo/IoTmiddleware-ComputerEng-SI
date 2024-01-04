using System;
using System.Collections.Generic;
using System.Linq;
using middleware_d26.Models;
using middleware_d26.DataContext;
using System.Threading.Tasks;

namespace middleware_d26.Services
{
    public class ContainerService
    {
        private readonly MiddlewareDbContext dbContext;

        public ContainerService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateContainer(string applicationName, string containerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            if (dbContext.Containers.Any(c =>
                c.Parent == parentApplication.Id && c.Name == containerName))
            {
                throw new Exception("Container already exists");
            }

            var container = new Container
            {
                Name = containerName,
                Creation_Dt = DateTime.Now,
                Parent = parentApplication.Id
            };

            dbContext.Containers.Add(container);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateContainer(string applicationName, string containerName, string newContainerName)
        {
            // check if parent application exists
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            // check if new name already exists
            if (dbContext.Containers.Any(c =>
                c.Parent == parentApplication.Id && c.Name == containerName))
            {
                throw new Exception("Container already exists");
            }

            // check if container exists
            var container = dbContext.Containers.FirstOrDefault(c =>
                           c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Container not found");

            container.Name = newContainerName;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteContainer(string applicationName, string containerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName)
                ?? throw new Exception("Container not found");
            
            dbContext.Containers.Remove(container);
            await dbContext.SaveChangesAsync();
        }

        //public Task<IEnumerable<Container>> GetContainers(string applicationName)
        //{
        //    var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

        //    if (parentApplication == null)
        //    {
        //        // Handle the case where the parent application is not found
        //        // For example, you can return an empty list or throw a more specific exception.
        //        return Enumerable.Empty<Container>();
        //    }

        //    var containers = dbContext.Containers.Where(c => c.Parent == parentApplication.Id).ToList();
        //    return containers;
        //}


        //public Task<Container> GetContainer(string applicationName, string containerName)
        //{
        //    // Replace with your logic to retrieve a specific container based on applicationName and containerName
        //    var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

        //    if (parentApplication == null)
        //    {
        //        throw new Exception("Parent application not found");
        //    }

        //    var container = dbContext.Containers.FirstOrDefault(c =>
        //        c.Parent == parentApplication.Id && c.Name == containerName);

        //    return container;
        //}
    }
}
