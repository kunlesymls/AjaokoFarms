namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductDiscount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductDiscount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
