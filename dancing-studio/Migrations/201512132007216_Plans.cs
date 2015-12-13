namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Plans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessDay = c.Int(nullable: false),
                        LessTime = c.DateTime(nullable: false),
                        HallNum = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            DropIndex("dbo.Plans", new[] { "GroupId" });
            DropTable("dbo.Plans");
        }
    }
}
