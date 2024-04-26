namespace Bicycle_project.Migrations.CycleDB
{
    using Bicycle_project.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bicycle_project.Models.CycleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\CycleDB";
        }

        protected override void Seed(Bicycle_project.Models.CycleDbContext db)
        {
            db.Companys.AddRange(new Company[]
            {
                new Company{CompanyName="Duranto"},
                new Company{CompanyName="Phoneix"},
                new Company{CompanyName="Hero"}
            });
            db.CycleModels.AddRange(new CycleModel[]
            {
                new CycleModel{ModelName="Altruiste"},
                new CycleModel{ModelName="Sprint"},
                new CycleModel{ModelName="Spyder"}
            });
            db.SaveChanges();
            Cycle c = new Cycle
            {
                CycleName = "Hero MT04",
                ModelId = 2,
                CompanyId = 3,
                EntryDate = new DateTime(2024, 1, 04),
                Status = true,
                Picture = "img-1.jpg"
            };
            c.Stocks.Add(new StockInfo { Category = Category.MountainBikes, Qty = 5, Amount = 14000 });
            c.Stocks.Add(new StockInfo { Category = Category.AdventureBikes, Qty = 10, Amount = 15000 });
            db.Cycles.Add(c);
            db.SaveChanges();
        }
    }
}
