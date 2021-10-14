namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsContractColumnToContractsTables : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.tbl_RentAgreementHeaders", "IsFromAgreementContract", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_SaleAgreementHeaders", "IsFromAgreementContract", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
           
            
            DropColumn("dbo.tbl_SaleAgreementHeaders", "IsFromAgreementContract");
            DropColumn("dbo.tbl_RentAgreementHeaders", "IsFromAgreementContract");
           
        }
    }
}
