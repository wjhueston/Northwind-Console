namespace NorthwindConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCategoryRules : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Description", c => c.String());
        }
    }
}
