namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNovelType1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NovelTypes", "TypeName", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NovelTypes", "TypeName", c => c.String(nullable: false));
        }
    }
}
