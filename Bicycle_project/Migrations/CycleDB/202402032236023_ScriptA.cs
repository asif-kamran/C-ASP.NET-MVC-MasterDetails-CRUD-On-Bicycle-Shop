namespace Bicycle_project.Migrations.CycleDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Cycles",
                c => new
                    {
                        CycleId = c.Int(nullable: false, identity: true),
                        CycleName = c.String(nullable: false, maxLength: 50),
                        EntryDate = c.DateTime(nullable: false, storeType: "date"),
                        Status = c.Boolean(nullable: false),
                        Picture = c.String(),
                        CompanyId = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CycleId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.CycleModels", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.CycleModels",
                c => new
                    {
                        CycleModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CycleModelId);
            
            CreateTable(
                "dbo.StockInfoes",
                c => new
                    {
                        StockInfoId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Qty = c.Int(nullable: false),
                        CycleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockInfoId)
                .ForeignKey("dbo.Cycles", t => t.CycleId, cascadeDelete: true)
                .Index(t => t.CycleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockInfoes", "CycleId", "dbo.Cycles");
            DropForeignKey("dbo.Cycles", "ModelId", "dbo.CycleModels");
            DropForeignKey("dbo.Cycles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.StockInfoes", new[] { "CycleId" });
            DropIndex("dbo.Cycles", new[] { "ModelId" });
            DropIndex("dbo.Cycles", new[] { "CompanyId" });
            DropTable("dbo.StockInfoes");
            DropTable("dbo.CycleModels");
            DropTable("dbo.Cycles");
            DropTable("dbo.Companies");
        }
    }
}
