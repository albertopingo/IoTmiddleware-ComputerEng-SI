using middleware_d26.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace middleware_d26.DataContext
{
    public class MiddlewareDbInitializer : DropCreateDatabaseIfModelChanges<MiddlewareDbContext>
    {
        private readonly Random random = new Random();
        internal void SeedData(MiddlewareDbContext context)
        {
            // Your seeding logic here
            var applications = new List<Application>
            {
                new Application { Name = "App1", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Application { Name = "App2", Creation_Dt = GetRandomDateWithinLastNDays(3) }
            };

            context.Applications.AddRange(applications);
            context.SaveChanges();

            // Seed initial data for Container
            var containers = new List<Container>
            {
                new Container { Name = "Container1", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Container { Name = "Container2", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Container { Name = "Container3", Creation_Dt = GetRandomDateWithinLastNDays(3) }
            };

            context.Containers.AddRange(containers);
            context.SaveChanges();

            // Seed initial data for Data
            var dataRecords = new List<Data>
            {
                new Data { Content = "Content1", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Data { Content = "Content2", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Data { Content = "Content3", Creation_Dt = GetRandomDateWithinLastNDays(3) }
            };

            context.DataRecords.AddRange(dataRecords);
            context.SaveChanges();

            // Seed initial data for Subscription
            var subscriptions = new List<Subscription>
            {
                new Subscription { Name = "Subscription1", Event = "Event1", Endpoint = "Endpoint1", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Subscription { Name = "Subscription2", Event = "Event2", Endpoint = "Endpoint2", Creation_Dt = GetRandomDateWithinLastNDays(3) },
                new Subscription { Name = "Subscription3", Event = "Event3", Endpoint = "Endpoint3", Creation_Dt = GetRandomDateWithinLastNDays(3) }
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
