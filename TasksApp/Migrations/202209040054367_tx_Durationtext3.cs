namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tx_Durationtext3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "taskDurationTicks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "taskDurationTicks", c => c.Long(nullable: false));
        }
    }
}
