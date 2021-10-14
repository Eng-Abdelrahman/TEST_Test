namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Writependingmodelchanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_SaleAgreementHeaders", "IsNormalContruct");
        }
        
        public override void Down()
        {           
            
            DropColumn("dbo.tbl_SaleAgreementHeaders", "IsNormalContruct");         
         
        }
    }
}
