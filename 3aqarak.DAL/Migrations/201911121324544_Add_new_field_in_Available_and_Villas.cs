namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_new_field_in_Available_and_Villas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AvailableUnits", "Remaining", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.tbl_AvailableUnits", "Over", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.tbl_AvailableUnits", "YearOfInstallment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tbl_AvailableUnits", "BasisOfInstallment", c => c.Byte());
            AddColumn("dbo.tbl_VillasAvailables", "Remaining", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.tbl_VillasAvailables", "Over", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.tbl_VillasAvailables", "YearOfInstallment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tbl_VillasAvailables", "BasisOfInstallment", c => c.Byte());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_VillasAvailables", "BasisOfInstallment");
            DropColumn("dbo.tbl_VillasAvailables", "YearOfInstallment");
            DropColumn("dbo.tbl_VillasAvailables", "Over");
            DropColumn("dbo.tbl_VillasAvailables", "Remaining");
            DropColumn("dbo.tbl_AvailableUnits", "BasisOfInstallment");
            DropColumn("dbo.tbl_AvailableUnits", "YearOfInstallment");
            DropColumn("dbo.tbl_AvailableUnits", "Over");
            DropColumn("dbo.tbl_AvailableUnits", "Remaining");
        }
    }
}
