using middleware_d26.DataContext;
using middleware_d26.Models;
using System;
using System.Linq;
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

        public Container GetContainer(string applicationName, string containerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            return container;
        }

        public async Task UpdateContainer(string applicationName, string containerName, string newContainerName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Parent application not found");

            if (dbContext.Containers.Any(c =>
                c.Parent == parentApplication.Id && c.Name == newContainerName))
            {
                throw new Exception("Container already exists");
            }

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
    }
}
