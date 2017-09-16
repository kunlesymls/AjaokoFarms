namespace AdunbiKiddies.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateProfessionalPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProfessionalPayments", "PaymentDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfessionalPayments", "PaymentDateTime");
        }
    }
}
