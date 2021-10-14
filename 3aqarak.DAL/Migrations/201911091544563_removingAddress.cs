namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingAddress : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_units", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_units", "Address", c => c.String(nullable: false));
        }
    }
}
