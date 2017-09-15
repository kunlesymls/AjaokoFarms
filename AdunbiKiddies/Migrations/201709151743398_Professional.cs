namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Professional : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessionalWorkers",
                c => new
                    {
                        ProfessionalWorkerId = c.String(nullable: false, maxLength: 128),
                        HighestQualification = c.String(),
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
                .PrimaryKey(t => t.ProfessionalWorkerId);
            
            CreateTable(
                "dbo.ProfessionalAddresses",
                c => new
                    {
                        ProfessionalAddressId = c.String(nullable: false, maxLength: 128),
                        ProfessionalWorkerId = c.String(maxLength: 128),
                        AddressType = c.String(),
                        BuildingNo = c.String(),
                        StreetName = c.String(),
                        TownName = c.String(),
                        StateName = c.String(),
                    })
                .PrimaryKey(t => t.ProfessionalAddressId)
                .ForeignKey("dbo.ProfessionalWorkers", t => t.ProfessionalWorkerId)
                .Index(t => t.ProfessionalWorkerId);
            
            CreateTable(
                "dbo.ProfessionalDocuments",
                c => new
                    {
                        ProfessionalDocumentId = c.Int(nullable: false, identity: true),
                        ProfessionalWorkerId = c.String(maxLength: 128),
                        FileName = c.String(),
                        FileLocation = c.String(),
                    })
                .PrimaryKey(t => t.ProfessionalDocumentId)
                .ForeignKey("dbo.ProfessionalWorkers", t => t.ProfessionalWorkerId)
                .Index(t => t.ProfessionalWorkerId);
            
            CreateTable(
                "dbo.ProfessionalPayments",
                c => new
                    {
                        ProfessionalPaymentId = c.Int(nullable: false, identity: true),
                        ProfessionalWorkerId = c.String(maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProfessionalPaymentId)
                .ForeignKey("dbo.ProfessionalWorkers", t => t.ProfessionalWorkerId)
                .Index(t => t.ProfessionalWorkerId);
            
            AddColumn("dbo.Merchants", "Gender", c => c.String());
            AddColumn("dbo.Merchants", "TownOfBirth", c => c.String());
            AddColumn("dbo.Merchants", "StateOfOrigin", c => c.String());
            AddColumn("dbo.Merchants", "Nationality", c => c.String());
            AddColumn("dbo.Merchants", "Passport", c => c.Binary());
            AlterColumn("dbo.Merchants", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Merchants", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Merchants", "MiddleName", c => c.String(maxLength: 50));
            DropColumn("dbo.Merchants", "Idcard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Merchants", "Idcard", c => c.Binary());
            DropForeignKey("dbo.ProfessionalPayments", "ProfessionalWorkerId", "dbo.ProfessionalWorkers");
            DropForeignKey("dbo.ProfessionalDocuments", "ProfessionalWorkerId", "dbo.ProfessionalWorkers");
            DropForeignKey("dbo.ProfessionalAddresses", "ProfessionalWorkerId", "dbo.ProfessionalWorkers");
            DropIndex("dbo.ProfessionalPayments", new[] { "ProfessionalWorkerId" });
            DropIndex("dbo.ProfessionalDocuments", new[] { "ProfessionalWorkerId" });
            DropIndex("dbo.ProfessionalAddresses", new[] { "ProfessionalWorkerId" });
            AlterColumn("dbo.Merchants", "MiddleName", c => c.String());
            AlterColumn("dbo.Merchants", "LastName", c => c.String());
            AlterColumn("dbo.Merchants", "FirstName", c => c.String());
            DropColumn("dbo.Merchants", "Passport");
            DropColumn("dbo.Merchants", "Nationality");
            DropColumn("dbo.Merchants", "StateOfOrigin");
            DropColumn("dbo.Merchants", "TownOfBirth");
            DropColumn("dbo.Merchants", "Gender");
            DropTable("dbo.ProfessionalPayments");
            DropTable("dbo.ProfessionalDocuments");
            DropTable("dbo.ProfessionalAddresses");
            DropTable("dbo.ProfessionalWorkers");
        }
    }
}
