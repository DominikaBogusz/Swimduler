namespace Swimduler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonBaseClassAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Street", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Clients", "ApartmentNumber", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Clients", "PostCode", c => c.String(nullable: false));
            AddColumn("dbo.Trainers", "Street", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Trainers", "ApartmentNumber", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Trainers", "PostCode", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clients", "SecondName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Trainers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Trainers", "SecondName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Clients", "Address");
            DropColumn("dbo.Trainers", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Clients", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Trainers", "SecondName", c => c.String(nullable: false));
            AlterColumn("dbo.Trainers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "SecondName", c => c.String(nullable: false));
            AlterColumn("dbo.Clients", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.Trainers", "PostCode");
            DropColumn("dbo.Trainers", "ApartmentNumber");
            DropColumn("dbo.Trainers", "Street");
            DropColumn("dbo.Clients", "PostCode");
            DropColumn("dbo.Clients", "ApartmentNumber");
            DropColumn("dbo.Clients", "Street");
        }
    }
}
