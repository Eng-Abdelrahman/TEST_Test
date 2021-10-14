namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_AvailableLands", "FlatNumber");
            DropColumn("dbo.tbl_AvailableLands", "ApartmentNumber");
            DropColumn("dbo.tbl_AvailableLands", "GroupNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_AvailableLands", "GroupNumber", c => c.String());
            AddColumn("dbo.tbl_AvailableLands", "ApartmentNumber", c => c.String());
            AddColumn("dbo.tbl_AvailableLands", "FlatNumber", c => c.String());
        }
    }
}
