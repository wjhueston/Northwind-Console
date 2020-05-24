namespace NorthwindConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMaxDesLen1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Description", c => c.String());
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "ProductName", c => c.String());
            AlterColumn("dbo.Categories", "Description", c => c.String(maxLength: 250));
        }
    }
}
