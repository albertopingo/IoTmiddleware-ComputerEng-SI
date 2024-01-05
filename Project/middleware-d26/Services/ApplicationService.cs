using System;
using System.Collections.Generic;
using System.Linq;
using middleware_d26.Models;
using middleware_d26.DataContext;
using System.Threading.Tasks;

namespace middleware_d26.Services
{
    public class ApplicationService
    {
        private readonly MiddlewareDbContext dbContext;

        public ApplicationService(MiddlewareDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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

        public async Task UpdateApplication(string applicationName, string newApplicationName)
        {

            if (dbContext.Applications.Any(a =>
                a.Name == newApplicationName))
            {
                throw new Exception("Application already exists");
            }

            // check if container exists
            var application = dbContext.Applications.FirstOrDefault(a =>
                            a.Name == applicationName)
                ?? throw new Exception("Application not found");

            application.Name = newApplicationName;
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteApplication(string applicationName)
        {
            var application = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName)
                ?? throw new Exception("application not found");

            var containers = dbContext.Containers.Where(c =>
                c.Parent == application.Id).ToList();
            if (containers.Any())
            {
                foreach(var container in containers)
                {
                    dbContext.Containers.Remove(container);
                    await dbContext.SaveChangesAsync();
                }
            }

            dbContext.Applications.Remove(application);
            await dbContext.SaveChangesAsync();
        }
        public Task GetApplication(string applicationName)
        {
            
            var application = dbContext.Applications.FirstOrDefault(a => a.Name == applicationName);

            if (application == null)
            {
                throw new Exception("Application not found");
            }

            return application;
        }
    }
}
