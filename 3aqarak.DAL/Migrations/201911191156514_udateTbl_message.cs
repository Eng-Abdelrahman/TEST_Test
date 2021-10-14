namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udateTbl_message : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Messages", "DateTimeStart", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.tbl_Messages", "DateTimeEnd", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.tbl_Messages", "IsDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Messages", "IsDone");
            DropColumn("dbo.tbl_Messages", "DateTimeEnd");
            DropColumn("dbo.tbl_Messages", "DateTimeStart");
        }
    }
}
