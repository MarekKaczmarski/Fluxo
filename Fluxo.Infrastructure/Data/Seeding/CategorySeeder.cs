using Fluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Infrastructure.Data.Seeding
{
    public static class CategorySeeder
    {
        public static IEnumerable<Category> GetSeedData() => new List<Category>
        {
            new Category(SeedDataConstants.TransferCategoryId, "Transfer", "wallet"),
            new Category(SeedDataConstants.FoodCategoryId, "Food", "utensils"),
            new Category(SeedDataConstants.ElectronicsCategoryId, "Electronics", "smartphone"),
            new Category(SeedDataConstants.PharmacyCategoryId, "Pharmacy", "pill"),
            new Category(SeedDataConstants.TransportCategoryId, "Transport", "bus")
        };
    }
}
