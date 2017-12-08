namespace Swimduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CycleEndAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "CycleEnd", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lessons", "CycleEnd");
        }
    }
}
