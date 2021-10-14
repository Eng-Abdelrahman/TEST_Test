namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingMyPropertyAndAddingIsCanceled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_AgreementContract", "IsCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_AgreementContract", "IsCancelled");
        }
    }
}
