namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVillaNumberAndGroupNumberInVillas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_VillasAvailables", "VillaNumber", c => c.String());
            AddColumn("dbo.tbl_VillasAvailables", "GroupNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_VillasAvailables", "GroupNumber");
            DropColumn("dbo.tbl_VillasAvailables", "VillaNumber");
        }
    }
}
