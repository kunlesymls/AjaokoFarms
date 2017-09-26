namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merchantPayement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MerchantPayments",
                c => new
                    {
                        MerchantPaymentId = c.Int(nullable: false, identity: true),
                        MerchantId = c.String(maxLength: 128),
                        AmountPayed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDateTime = c.DateTime(nullable: false),
                        IsPayed = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.MerchantPaymentId)
                .ForeignKey("dbo.Merchants", t => t.MerchantId)
                .Index(t => t.MerchantId);
            
            AddColumn("dbo.Merchants", "Haspayed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Merchants", "ExpiryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MerchantPayments", "MerchantId", "dbo.Merchants");
            DropIndex("dbo.MerchantPayments", new[] { "MerchantId" });
            DropColumn("dbo.Merchants", "ExpiryDate");
            DropColumn("dbo.Merchants", "Haspayed");
            DropTable("dbo.MerchantPayments");
        }
    }
}
