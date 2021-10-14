namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostpondInTblAgreement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AgreementContract", "PostponeDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AgreementContract", "PostponeDateTime");
        }
    }
}
