namespace _3aqarak.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeBirthDateNullableinUsrTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Users", "DateOfBirth", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_Users", "DateOfBirth", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
