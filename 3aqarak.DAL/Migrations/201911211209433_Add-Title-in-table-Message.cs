namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitleintableMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Messages", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Messages", "Title");
        }
    }
}
