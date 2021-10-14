namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTbl_FellowupCalls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_FellowupCall",
                c => new
                    {
                        PK_FellowupCalls_Id = c.Int(nullable: false, identity: true),
                        FK_FellowupCalls_Users_EmpolyeeId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Descreption = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, storeType: "date"),
                        FK_FellowupCalls_Users_CreatedBy = c.Int(nullable: false),
                        FK_FellowupCalls_Users_ModidfiedBy = c.Int(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false, storeType: "date"),
                        IsDeleted = c.Boolean(nullable: false),
                        ClientName = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        Subject = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(),
                        FK_ClientCalls_Clients_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_FellowupCalls_Id)
                .ForeignKey("dbo.tbl_Users", t => t.FK_FellowupCalls_Users_EmpolyeeId)
                .ForeignKey("dbo.tbl_Users", t => t.FK_FellowupCalls_Users_CreatedBy)
                .ForeignKey("dbo.tbl_Users", t => t.FK_FellowupCalls_Users_ModidfiedBy)
                .ForeignKey("dbo.tbl_Clients", t => t.FK_ClientCalls_Clients_Id)
                .Index(t => t.FK_FellowupCalls_Users_EmpolyeeId)
                .Index(t => t.FK_FellowupCalls_Users_CreatedBy)
                .Index(t => t.FK_FellowupCalls_Users_ModidfiedBy)
                .Index(t => t.FK_ClientCalls_Clients_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_FellowupCall", "FK_ClientCalls_Clients_Id", "dbo.tbl_Clients");
            DropForeignKey("dbo.tbl_FellowupCall", "FK_FellowupCalls_Users_ModidfiedBy", "dbo.tbl_Users");
            DropForeignKey("dbo.tbl_FellowupCall", "FK_FellowupCalls_Users_CreatedBy", "dbo.tbl_Users");
            DropForeignKey("dbo.tbl_FellowupCall", "FK_FellowupCalls_Users_EmpolyeeId", "dbo.tbl_Users");
            DropIndex("dbo.tbl_FellowupCall", new[] { "FK_ClientCalls_Clients_Id" });
            DropIndex("dbo.tbl_FellowupCall", new[] { "FK_FellowupCalls_Users_ModidfiedBy" });
            DropIndex("dbo.tbl_FellowupCall", new[] { "FK_FellowupCalls_Users_CreatedBy" });
            DropIndex("dbo.tbl_FellowupCall", new[] { "FK_FellowupCalls_Users_EmpolyeeId" });
            DropTable("dbo.tbl_FellowupCall");
        }
    }
}
