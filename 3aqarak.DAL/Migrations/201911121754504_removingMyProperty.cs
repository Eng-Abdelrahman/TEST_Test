namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingMyProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_AgreementContract", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_AgreementContract", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
