using middleware_d26.DataContext;
using middleware_d26.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace middleware_d26.Services
{
    public class ApplicationService
    {
        private readonly MiddlewareDbContext dbContext;
        private readonly ContainerService containerService;

        public ApplicationService(MiddlewareDbContext dbContext, ContainerService containerService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.containerService = containerService ?? throw new ArgumentNullException(nameof(containerService));
        }

        public async Task CreateApplication(string applicationName)
        {
            if (dbContext.Applications.Any(a =>
                        a.Name == applicationName))
            {
                throw new Exception("Application name already exists");
            }
            var application = new Application
            {
                Name = applicationName,
                Creation_Dt = DateTime.Now
            };

            dbContext.Applications.Add(application);
            await dbContext.SaveChangesAsync();
        }

        public Application GetApplication(string applicationName)
        {
            var application = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Application not found");

            return application;
        }

        public async Task UpdateApplication(string applicationName, string newApplicationName)
        {

            if (dbContext.Applications.Any(a =>
                a.Name == newApplicationName))
            {
                throw new Exception("Application already exists");
            }

            var application = dbContext.Applications.FirstOrDefault(a =>
                            a.Name == applicationName)
                ?? throw new Exception("Application not found");

            application.Name = newApplicationName;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteApplication(string applicationName)
        {
            var application = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("Application not found");

            var containers = dbContext.Containers.Where(c => c.Parent == application.Id).ToList();

            if (containers.Any())
            {
                foreach (var container in containers)
                {
                    await containerService.DeleteContainer(applicationName, container.Name);
                }
            }

            dbContext.Applications.Remove(application);
            await dbContext.SaveChangesAsync();
        }
    }
}
