namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removephonevalidationfromclltables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_ClientsCalls", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_FellowupCall", "PhoneNumber1", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_FellowupCall", "PhoneNumber2", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_FellowupCall", "PhoneNumber2", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.tbl_FellowupCall", "PhoneNumber1", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.tbl_ClientsCalls", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
        }
    }
}
