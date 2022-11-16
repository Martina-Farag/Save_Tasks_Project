namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastUpdateDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "creationDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "lastUpdateDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "lastUpdateDateTime");
            DropColumn("dbo.Tasks", "creationDateTime");
        }
    }
}
