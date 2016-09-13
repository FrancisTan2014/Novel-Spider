namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeChapter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Novels", "ChapterListUrl", c => c.String());
            AddColumn("dbo.Chapters", "WordCount", c => c.Int(nullable: false));
            DropColumn("dbo.Novels", "NovelTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Novels", "NovelTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Chapters", "WordCount");
            DropColumn("dbo.Novels", "ChapterListUrl");
        }
    }
}
