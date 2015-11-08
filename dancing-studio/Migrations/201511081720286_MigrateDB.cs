namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lessons", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lessons", "DateTime", c => c.DateTime());
        }
    }
}
