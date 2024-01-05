namespace middleware_d26.Migrations
{
    using middleware_d26.DataContext;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<middleware_d26.DataContext.MiddlewareDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(middleware_d26.DataContext.MiddlewareDbContext context)
        {
            new MiddlewareDbInitializer().SeedData(context);
        }
    }
}
