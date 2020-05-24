namespace NorthwindConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMaxDesLen : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Description", c => c.String(maxLength: 500));
        }
    }
}
