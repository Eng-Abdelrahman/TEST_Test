namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding3fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_units", "FlatNumber", c => c.String());
            AddColumn("dbo.tbl_units", "ApartmentNumber", c => c.String());
            AddColumn("dbo.tbl_units", "GroupNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_units", "GroupNumber");
            DropColumn("dbo.tbl_units", "ApartmentNumber");
            DropColumn("dbo.tbl_units", "FlatNumber");
        }
    }
}
