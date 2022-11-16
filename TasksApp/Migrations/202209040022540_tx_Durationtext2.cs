namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tx_Durationtext2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "taskDurationTicks", c => c.Long(nullable: false));
            AddColumn("dbo.Tasks", "taskDurationText", c => c.String());
            DropColumn("dbo.Tasks", "taskDuration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "taskDuration", c => c.String());
            DropColumn("dbo.Tasks", "taskDurationText");
            DropColumn("dbo.Tasks", "taskDurationTicks");
        }
    }
}
