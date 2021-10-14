namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPostpondAndNoteInTblAgreement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AgreementContract", "Notes", c => c.String());
            AddColumn("dbo.tbl_AgreementContract", "IsPostponed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AgreementContract", "IsPostponed");
            DropColumn("dbo.tbl_AgreementContract", "Notes");
        }
    }
}
