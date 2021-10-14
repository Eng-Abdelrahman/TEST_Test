namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsAgreementColumnToExpectedContractTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_ExpectedContracts", "IsContractAgreement", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_ExpectedContracts", "IsContractAgreement");
        }
    }
}
