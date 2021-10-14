namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_new_field_in_VillasAvailable_Collect_Price_Over_together : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_VillasAvailables", "AdvancePayment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_VillasAvailables", "AdvancePayment");
        }
    }
}
