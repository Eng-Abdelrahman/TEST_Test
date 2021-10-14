namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefieldinvalls : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_VillasAvailables", "Address");
            DropColumn("dbo.tbl_VillasDemands", "MinFloorNumber");
            DropColumn("dbo.tbl_VillasDemands", "MaxFloorNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_VillasDemands", "MaxFloorNumber", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_VillasDemands", "MinFloorNumber", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_VillasAvailables", "Address", c => c.String(nullable: false));
        }
    }
}
