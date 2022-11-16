namespace TasksApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inshlizeDb3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "taskProjectSymbol", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "taskProjectSymbol");
        }
    }
}
