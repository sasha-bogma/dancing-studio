namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class req : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teachers", "Birthday", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teachers", "Birthday", c => c.DateTime());
            AlterColumn("dbo.Students", "Birthday", c => c.DateTime());
        }
    }
}
