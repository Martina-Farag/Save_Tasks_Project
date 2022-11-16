namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inshlizeDb2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "taskStartDateTime", c => c.DateTime());
            AlterColumn("dbo.Tasks", "taskEndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "taskEndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tasks", "taskStartDateTime", c => c.DateTime(nullable: false));
        }
    }
}
