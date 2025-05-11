namespace WebSiteMCSport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class aaaa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ProductReview",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProductId = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Message = c.String(nullable: false),
                    Rating = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.tb_ProductReview", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_ProductReview", new[] { "ProductId" });
            DropTable("dbo.tb_ProductReview");
        }
    }
}
