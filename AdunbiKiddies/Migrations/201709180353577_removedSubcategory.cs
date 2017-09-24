namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedSubcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SubCategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SubCategoryId");
        }
    }
}
