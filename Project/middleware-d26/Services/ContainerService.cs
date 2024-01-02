using System;
using System.Collections.Generic;
using System.Linq;
using middleware_d26.Models;
using middleware_d26.DataContext;

namespace middleware_d26.Services
{
    public class ContainerService
    {
        private readonly MiddlewareDbContext dbContext;

        public ContainerService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<Container> GetContainers(string applicationName)
        {
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                // Handle the case where the parent application is not found
                // For example, you can return an empty list or throw a more specific exception.
                return Enumerable.Empty<Container>();
            }

            var containers = dbContext.Containers.Where(c => c.Parent == parentApplication.Id).ToList();
            return containers;
        }


        public Container GetContainer(string applicationName, string containerName)
        {
            // Replace with your logic to retrieve a specific container based on applicationName and containerName
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                throw new Exception("Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            return container;
        }

        public void CreateContainer(string applicationName, Container container)
        {
            // Replace with your logic to create a new container
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                throw new Exception("Parent application not found");
            }

            container.Creation_Dt = DateTime.Now;
            container.Parent = parentApplication.Id;

            dbContext.Containers.Add(container);
            dbContext.SaveChanges();
        }

        public void UpdateContainer(string applicationName, string containerName, Container updatedContainer)
        {
            // Replace with your logic to update an existing container
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                throw new Exception("Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            if (container == null)
            {
                throw new Exception("Container not found");
            }

            container.Name = updatedContainer.Name;

            dbContext.SaveChanges();
        }

        public void DeleteContainer(string applicationName, string containerName)
        {
            // Replace with your logic to delete an existing container
            var parentApplication = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (parentApplication == null)
            {
                throw new Exception("Parent application not found");
            }

            var container = dbContext.Containers.FirstOrDefault(c =>
                c.Parent == parentApplication.Id && c.Name == containerName);

            if (container == null)
            {
                throw new Exception("Container not found");
            }

            dbContext.Containers.Remove(container);
            dbContext.SaveChanges();
        }
    }
}
