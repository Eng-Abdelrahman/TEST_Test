namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneNumber2ToFellowupCall : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_FellowupCall", "PhoneNumber1", c => c.String(nullable: false, maxLength: 11));
            AddColumn("dbo.tbl_FellowupCall", "PhoneNumber2", c => c.String(nullable: false, maxLength: 11));
            DropColumn("dbo.tbl_FellowupCall", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_FellowupCall", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
            DropColumn("dbo.tbl_FellowupCall", "PhoneNumber2");
            DropColumn("dbo.tbl_FellowupCall", "PhoneNumber1");
        }
    }
}
