namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Info", c => c.String(maxLength: 500));
            AddColumn("dbo.Salaries", "lobe", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salaries", "lobe");
            DropColumn("dbo.Students", "Info");
        }
    }
}
