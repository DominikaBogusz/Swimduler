namespace Swimduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErrorHandling : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Clients", new[] { "ApplicationUserId" });
            DropIndex("dbo.Trainers", new[] { "ApplicationUserId" });
            AddColumn("dbo.Clients", "UserId", c => c.String());
            AddColumn("dbo.Trainers", "AdminUserId", c => c.String(nullable: false));
            //DropColumn("dbo.Clients", "ApplicationUserId");
            //DropColumn("dbo.Trainers", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Clients", "ApplicationUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Trainers", "AdminUserId");
            DropColumn("dbo.Clients", "UserId");
            CreateIndex("dbo.Trainers", "ApplicationUserId");
            CreateIndex("dbo.Clients", "ApplicationUserId");
            AddForeignKey("dbo.Trainers", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
