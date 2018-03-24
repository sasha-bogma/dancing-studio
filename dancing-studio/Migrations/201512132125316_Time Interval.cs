namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeInterval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plans", "LessTimeInterval", c => c.Int(nullable: false));
            DropColumn("dbo.Plans", "LessTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plans", "LessTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Plans", "LessTimeInterval");
        }
    }
}
