using middleware_d26.Models;
using System.Data.Entity;

namespace middleware_d26.DataContext
{
    public class MiddlewareDbContext : DbContext
    {

        static MiddlewareDbContext()
        {
            Database.SetInitializer(new MiddlewareDbInitializer());
        }

        public MiddlewareDbContext() : base(Properties.Settings.Default.ConnStr)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Data> DataRecords { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}