namespace middleware_d26.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Creation_Dt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Containers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Creation_Dt = c.DateTime(nullable: false),
                        Parent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Content = c.String(nullable: false, maxLength: 255),
                        Creation_Dt = c.DateTime(nullable: false),
                        Parent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Creation_Dt = c.DateTime(nullable: false),
                        Parent = c.Int(nullable: false),
                        Event = c.String(nullable: false, maxLength: 255),
                        Endpoint = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Data");
            DropTable("dbo.Containers");
            DropTable("dbo.Applications");
        }
    }
}
