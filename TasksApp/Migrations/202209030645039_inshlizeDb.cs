namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inshlizeDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        taskId = c.Int(nullable: false, identity: true),
                        taskText = c.String(),
                        taskStartDateTime = c.DateTime(nullable: false),
                        taskEndDateTime = c.DateTime(nullable: false),
                        taskDuration = c.String(),
                    })
                .PrimaryKey(t => t.taskId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tasks");
        }
    }
}
