using middleware_d26.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace middleware_d26.DataDbContext
{
    public class MiddlewareDbContext : DbContext
    {
        public MiddlewareDbContext() : base(Properties.Settings.Default.ConnStr)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Data> DataRecords { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}