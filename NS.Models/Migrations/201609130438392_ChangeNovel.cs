namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNovel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Novels", "CoverUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Novels", "CoverUrl");
        }
    }
}
