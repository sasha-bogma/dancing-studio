namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Salaries", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Salaries", "Date", c => c.DateTime());
            AlterColumn("dbo.Payments", "Date", c => c.DateTime());
        }
    }
}
