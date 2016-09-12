namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChapter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        Content = c.String(),
                        UpdateTime = c.DateTime(nullable: false),
                        NovelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Novels", t => t.NovelId, cascadeDelete: true)
                .Index(t => t.NovelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapters", "NovelId", "dbo.Novels");
            DropIndex("dbo.Chapters", new[] { "NovelId" });
            DropTable("dbo.Chapters");
        }
    }
}
