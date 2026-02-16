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
            new() { Id = SeedDataConstants.TransferCategoryId, Name = "Transfer", Icon = "wallet" },
            new() { Id = SeedDataConstants.FoodCategoryId, Name = "Food", Icon = "utensils" },
            new() { Id = SeedDataConstants.ElectronicsCategoryId, Name = "Electronics", Icon = "smartphone" },
            new() { Id = SeedDataConstants.PharmacyCategoryId, Name = "Pharmacy", Icon = "pill" },
            new() { Id = SeedDataConstants.TransportCategoryId, Name = "Transport", Icon = "bus" }
        };
    }
}
