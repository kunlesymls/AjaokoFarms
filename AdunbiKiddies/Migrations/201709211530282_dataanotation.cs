namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataanotation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "SubCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "SubCategoryId", c => c.Int(nullable: false));
        }
    }
}
