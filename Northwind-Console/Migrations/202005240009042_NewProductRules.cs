namespace NorthwindConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewProductRules : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "SupplierId" });
            AlterColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            AlterColumn("dbo.Products", "SupplierId", c => c.Int());
            AlterColumn("dbo.Products", "CategoryId", c => c.Int());
            CreateIndex("dbo.Products", "SupplierId");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "SupplierId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "CategoryId");
        }
    }
}
