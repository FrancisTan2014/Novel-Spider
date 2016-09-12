namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Novels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        UpdateTime = c.DateTime(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        AuthorId = c.Int(nullable: false),
                        NovelTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.NovelTypes", t => t.NovelTypeId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.NovelTypeId);
            
            AlterColumn("dbo.NovelTypes", "AddTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Novels", "NovelTypeId", "dbo.NovelTypes");
            DropForeignKey("dbo.Novels", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Novels", new[] { "NovelTypeId" });
            DropIndex("dbo.Novels", new[] { "AuthorId" });
            AlterColumn("dbo.NovelTypes", "AddTime", c => c.DateTime(nullable: false));
            DropTable("dbo.Novels");
            DropTable("dbo.Authors");
        }
    }
}
