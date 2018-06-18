namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedpercentfieldfromsalary : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Salaries", "lobe");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salaries", "lobe", c => c.Double(nullable: false));
        }
    }
}
