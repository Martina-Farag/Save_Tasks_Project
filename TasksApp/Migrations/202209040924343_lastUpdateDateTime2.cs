namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastUpdateDateTime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "creationDateTime", c => c.DateTime());
            AlterColumn("dbo.Tasks", "lastUpdateDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "lastUpdateDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tasks", "creationDateTime", c => c.DateTime(nullable: false));
        }
    }
}
