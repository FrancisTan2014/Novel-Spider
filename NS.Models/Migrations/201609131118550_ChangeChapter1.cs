namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeChapter1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "TitleWithNoSpace", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "TitleWithNoSpace");
        }
    }
}
