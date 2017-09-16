namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankDetails",
                c => new
                    {
                        BankDetailId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        AccountName = c.String(),
                        AccountNumber = c.Int(nullable: false),
                        BankName = c.String(),
                        AccountType = c.String(),
                        IsPrimary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BankDetailId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Gender = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        TownOfBirth = c.String(),
                        StateOfOrigin = c.String(),
                        Nationality = c.String(),
                        Passport = c.Binary(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.ShippingDetails",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.Sales", "CustomerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Sales", "CustomerId");
            AddForeignKey("dbo.Sales", "CustomerId", "dbo.Customers", "CustomerId");
            DropColumn("dbo.Sales", "FirstName");
            DropColumn("dbo.Sales", "LastName");
            DropColumn("dbo.Sales", "SalesRepName");
            DropColumn("dbo.Sales", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "Phone", c => c.String(nullable: false, maxLength: 24));
            AddColumn("dbo.Sales", "SalesRepName", c => c.String());
            AddColumn("dbo.Sales", "LastName", c => c.String(nullable: false, maxLength: 160));
            AddColumn("dbo.Sales", "FirstName", c => c.String(nullable: false, maxLength: 160));
            DropForeignKey("dbo.ShippingDetails", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropIndex("dbo.ShippingDetails", new[] { "CustomerId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            DropColumn("dbo.Sales", "CustomerId");
            DropTable("dbo.ShippingDetails");
            DropTable("dbo.Customers");
            DropTable("dbo.BankDetails");
        }
    }
}
