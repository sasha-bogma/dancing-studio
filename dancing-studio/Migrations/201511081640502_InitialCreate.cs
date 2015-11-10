namespace dancing_studio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                        DateTime = c.DateTime(),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.GroupId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Presents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        LessonId = c.Int(nullable: false),
                        Condition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        PhoneNumber = c.String(maxLength: 18),
                        Birthday = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        Status = c.String(maxLength: 60),
                        Name = c.String(nullable: false, maxLength: 60),
                        Phone = c.String(maxLength: 18),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        PhoneNumber = c.String(maxLength: 18),
                        Birthday = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.GrouoStudents",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.StudentId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GrouoStudents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.GrouoStudents", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Salaries", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Lessons", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Groups", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Presents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Payments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Parents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Presents", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.Lessons", "GroupId", "dbo.Groups");
            DropIndex("dbo.GrouoStudents", new[] { "StudentId" });
            DropIndex("dbo.GrouoStudents", new[] { "GroupId" });
            DropIndex("dbo.Salaries", new[] { "TeacherId" });
            DropIndex("dbo.Payments", new[] { "StudentId" });
            DropIndex("dbo.Parents", new[] { "StudentId" });
            DropIndex("dbo.Presents", new[] { "LessonId" });
            DropIndex("dbo.Presents", new[] { "StudentId" });
            DropIndex("dbo.Lessons", new[] { "TeacherId" });
            DropIndex("dbo.Lessons", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "TeacherId" });
            DropTable("dbo.GrouoStudents");
            DropTable("dbo.Salaries");
            DropTable("dbo.Teachers");
            DropTable("dbo.Payments");
            DropTable("dbo.Parents");
            DropTable("dbo.Students");
            DropTable("dbo.Presents");
            DropTable("dbo.Lessons");
            DropTable("dbo.Groups");
        }
    }
}
