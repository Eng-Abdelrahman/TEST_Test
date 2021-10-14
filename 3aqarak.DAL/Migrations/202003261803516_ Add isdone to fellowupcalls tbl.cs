namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addisdonetofellowupcallstbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_FellowupCall", "IsDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_FellowupCall", "IsDone");
        }
    }
}
