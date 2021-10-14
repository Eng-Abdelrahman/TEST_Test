namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsInCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AvailableLands", "FlatNumber", c => c.String());
            AddColumn("dbo.tbl_AvailableLands", "ApartmentNumber", c => c.String());
            AddColumn("dbo.tbl_AvailableLands", "GroupNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AvailableLands", "GroupNumber");
            DropColumn("dbo.tbl_AvailableLands", "ApartmentNumber");
            DropColumn("dbo.tbl_AvailableLands", "FlatNumber");
        }
    }
}
