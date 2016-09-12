namespace NS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNovelType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NovelTypes", "AddTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.NovelTypes", "IsDelete", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NovelTypes", "TypeName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NovelTypes", "TypeName", c => c.String());
            DropColumn("dbo.NovelTypes", "IsDelete");
            DropColumn("dbo.NovelTypes", "AddTime");
        }
    }
}
