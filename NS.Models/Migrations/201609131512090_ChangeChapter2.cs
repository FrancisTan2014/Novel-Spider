namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeChapter2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "Sort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "Sort");
        }
    }
}
