using middleware_d26.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using middleware_d26.DataContext;

namespace middleware_d26.DataContext
{
    public class MiddlewareDbInitializer : DropCreateDatabaseIfModelChanges<MiddlewareDbContext>
    {
        private readonly Random random = new Random();

        internal void SeedData(MiddlewareDbContext context)
        {
            SeedApplications(context);
            SeedContainers(context);
            SeedDataRecords(context);
            SeedSubscriptions(context);
        }

        private void SeedApplications(MiddlewareDbContext context)
        {
            var applications = new List<Application>
                {
                    new Application { Name = "App1", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                    new Application { Name = "App2", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                    new Application { Name = "App3", Creation_Dt = GetRandomDateWithinLastNDays(3) }
                };

            context.Applications.AddRange(applications);
            context.SaveChanges();
        }

        private void SeedContainers(MiddlewareDbContext context)
        {
            var containers1 = new List<Container>
                {
                    new Container { Name = "Container1", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 },
                    new Container { Name = "Container2", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 },
                    new Container { Name = "Container3", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 }
                };

            var containers2 = new List<Container>
                {
                    new Container { Name = "Container4", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 2 },
                    new Container { Name = "Container5", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 2 }
                };

            var containers3 = new List<Container>
                {
                    new Container { Name = "Container6", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 3 },
                    new Container { Name = "Container7", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 3 }
                };

            context.Containers.AddRange(containers1);
            context.Containers.AddRange(containers2);
            context.Containers.AddRange(containers3);
            context.SaveChanges();
        }

        private void SeedDataRecords(MiddlewareDbContext context)
        {
            var dataRecords = new List<Data>
            {
                new Data { Name = "Data1", Content = "Content1", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 },
                new Data { Name = "Data2", Content = "Content2", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 2 },
                new Data { Name = "Data3", Content = "Content3", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 3 },
                new Data { Name = "Data4", Content = "Content4", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 4 },
                new Data { Name = "Data5", Content = "Content5", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 5 },
                new Data { Name = "Data6", Content = "Content6", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 6 },
            };

            context.DataRecords.AddRange(dataRecords);
            context.SaveChanges();
        }

        private void SeedSubscriptions(MiddlewareDbContext context)
        {
            var subscriptions = new List<Subscription>
            {
                new Subscription { Name = "Subscription1", Event = "Creation", Endpoint = "Endpoint1", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 },
                new Subscription { Name = "Subscription2", Event = "Deletion", Endpoint = "Endpoint2", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 1 },
                new Subscription { Name = "Subscription3", Event = "Creation", Endpoint = "Endpoint3", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 2 },
                new Subscription { Name = "Subscription4", Event = "Deletion", Endpoint = "Endpoint4", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 2 },
                new Subscription { Name = "Subscription5", Event = "Creation", Endpoint = "Endpoint5", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 3 },
                new Subscription { Name = "Subscription6", Event = "Deletion", Endpoint = "Endpoint6", Creation_Dt = GetRandomDateWithinLastNDays(3), Parent = 3 },
            };

            context.Subscriptions.AddRange(subscriptions);
            context.SaveChanges();
        }
        private DateTime GetRandomDateWithinLastNDays(int days)
        {
            return DateTime.Now.AddDays(-random.Next(1, days + 1));
        }
    }
}
