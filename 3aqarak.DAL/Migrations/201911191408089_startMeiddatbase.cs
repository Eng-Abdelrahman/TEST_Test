namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class startMeiddatbase : DbMigration
    {
        public override void Up()
        {           

     
            DropForeignKey("dbo.tbl_AgreementContract", "FK_DemandCat", "dbo.tbl_Categories");
            DropForeignKey("dbo.tbl_AgreementContract", "FK_AvailableCat", "dbo.tbl_Categories");
            DropForeignKey("dbo.tbl_AgreementContract", "FK_AgreementcContract_Users_ModidfiedBy", "dbo.tbl_Users");
            DropForeignKey("dbo.tbl_AgreementContract", "FK_AgreementcContract_Users_CreatedBy", "dbo.tbl_Users");
            DropForeignKey("dbo.tbl_AgreementContract", "tbl_Clients1_PK_Client_Id", "dbo.tbl_Clients");
            DropForeignKey("dbo.tbl_AgreementContract", "FK_AgreementcContract_Client_BuyerId", "dbo.tbl_Clients");
            DropIndex("dbo.tbl_AgreementContract", new[] { "tbl_Clients1_PK_Client_Id" });
            DropIndex("dbo.tbl_AgreementContract", new[] { "FK_AgreementcContract_Users_ModidfiedBy" });
            DropIndex("dbo.tbl_AgreementContract", new[] { "FK_AgreementcContract_Users_CreatedBy" });
            DropIndex("dbo.tbl_AgreementContract", new[] { "FK_AgreementcContract_Client_BuyerId" });
            DropIndex("dbo.tbl_AgreementContract", new[] { "FK_DemandCat" });
            DropIndex("dbo.tbl_AgreementContract", new[] { "FK_AvailableCat" });
            DropTable("dbo.tbl_AgreementContract");
        }
        
        public override void Down()
        {
         
            AddColumn("dbo.tbl_AgreementContract", "MyProperty", c => c.Int(nullable: false));
            DropColumn("dbo.tbl_SaleAgreementHeaders", "IsNormalContruct");
            DropColumn("dbo.tbl_AgreementContract", "IsPostponed");
            DropColumn("dbo.tbl_AgreementContract", "PostponeDateTime");
            DropColumn("dbo.tbl_AgreementContract", "Notes");
            DropColumn("dbo.tbl_AgreementContract", "IsCancelled");
        }
    }
}
