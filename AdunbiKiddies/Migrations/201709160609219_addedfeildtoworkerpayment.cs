namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfeildtoworkerpayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfessionalPayments", "IsPayed", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProfessionalPayments", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfessionalPayments", "Status");
            DropColumn("dbo.ProfessionalPayments", "IsPayed");
        }
    }
}
