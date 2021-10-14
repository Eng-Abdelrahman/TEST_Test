namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createMigrationForAddingNewFieldsNamedTblAgreementContract : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_AgreementContract",
                c => new
                    {
                        PK_AgreementContract_Id = c.Int(nullable: false, identity: true),
                        Paid = c.Int(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        SellerAddress = c.String(),
                        BuyerAddress = c.String(),
                        MyProperty = c.Int(nullable: false),
                        SellerId = c.String(),
                        BuyerId = c.String(),
                        DateOfsigningContract = c.DateTime(nullable: false),
                        DemandUnits_Id = c.Int(nullable: false),
                        AvailableUnits_Id = c.Int(nullable: false),
                        ContractUrl = c.String(),
                        FK_AvailableCat = c.Int(nullable: false),
                        FK_DemandCat = c.Int(nullable: false),
                        FK_AgreementcContract_Client_sallerId = c.Int(nullable: false),
                        FK_AgreementcContract_Client_BuyerId = c.Int(nullable: false),
                        FK_AgreementcContract_Users_CreatedBy = c.Int(nullable: false),
                        FK_AgreementcContract_Users_ModidfiedBy = c.Int(nullable: false),
                        tbl_Clients1_PK_Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PK_AgreementContract_Id)
                .ForeignKey("dbo.tbl_Clients", t => t.FK_AgreementcContract_Client_BuyerId)
                .ForeignKey("dbo.tbl_Clients", t => t.tbl_Clients1_PK_Client_Id)
                .ForeignKey("dbo.tbl_Users", t => t.FK_AgreementcContract_Users_CreatedBy)
                .ForeignKey("dbo.tbl_Users", t => t.FK_AgreementcContract_Users_ModidfiedBy)
                .ForeignKey("dbo.tbl_Categories", t => t.FK_AvailableCat)
                .ForeignKey("dbo.tbl_Categories", t => t.FK_DemandCat)
                .Index(t => t.FK_AvailableCat)
                .Index(t => t.FK_DemandCat)
                .Index(t => t.FK_AgreementcContract_Client_BuyerId)
                .Index(t => t.FK_AgreementcContract_Users_CreatedBy)
                .Index(t => t.FK_AgreementcContract_Users_ModidfiedBy)
                .Index(t => t.tbl_Clients1_PK_Client_Id);
            
        }
        
        public override void Down()
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
    }
}
