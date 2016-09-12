namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNovelTypeRelate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Novels", "NovelTypeId", "dbo.NovelTypes");
            DropIndex("dbo.Novels", new[] { "NovelTypeId" });
            CreateTable(
                "dbo.NovelTypeRelates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NovelTypeId = c.Int(nullable: false),
                        NovelId = c.Int(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Novels", t => t.NovelId, cascadeDelete: true)
                .ForeignKey("dbo.NovelTypes", t => t.NovelTypeId, cascadeDelete: true)
                .Index(t => t.NovelTypeId)
                .Index(t => t.NovelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NovelTypeRelates", "NovelTypeId", "dbo.NovelTypes");
            DropForeignKey("dbo.NovelTypeRelates", "NovelId", "dbo.Novels");
            DropIndex("dbo.NovelTypeRelates", new[] { "NovelId" });
            DropIndex("dbo.NovelTypeRelates", new[] { "NovelTypeId" });
            DropTable("dbo.NovelTypeRelates");
            CreateIndex("dbo.Novels", "NovelTypeId");
            AddForeignKey("dbo.Novels", "NovelTypeId", "dbo.NovelTypes", "Id", cascadeDelete: true);
        }
    }
}
