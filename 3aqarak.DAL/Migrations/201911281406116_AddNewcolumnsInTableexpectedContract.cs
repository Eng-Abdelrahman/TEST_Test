namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewcolumnsInTableexpectedContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_ExpectedContracts", "BuyerAddress", c => c.String());
            AddColumn("dbo.tbl_ExpectedContracts", "BuyerNationalId", c => c.String());
            AddColumn("dbo.tbl_ExpectedContracts", "SellerAddress", c => c.String());
            AddColumn("dbo.tbl_ExpectedContracts", "SellerNationalId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_ExpectedContracts", "SellerNationalId");
            DropColumn("dbo.tbl_ExpectedContracts", "SellerAddress");
            DropColumn("dbo.tbl_ExpectedContracts", "BuyerNationalId");
            DropColumn("dbo.tbl_ExpectedContracts", "BuyerAddress");
        }
    }
}
