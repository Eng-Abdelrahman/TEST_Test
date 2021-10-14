namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeCallumnFloreNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_VillasAvailables", "FloorNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_VillasAvailables", "FloorNumber", c => c.Int(nullable: false));
        }
    }
}
