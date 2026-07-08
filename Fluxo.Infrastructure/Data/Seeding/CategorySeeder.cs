using Fluxo.Domain.Entities;

namespace Fluxo.Infrastructure.Data.Seeding;

public static class CategorySeeder
{
    public static IEnumerable<Category> GetSeedData()
    {
        return
        [
            new Category(SeedDataConstants.TransferCategoryId, "Transfer", "wallet"),
            new Category(SeedDataConstants.FoodCategoryId, "Food", "utensils"),
            new Category(SeedDataConstants.ElectronicsCategoryId, "Electronics", "smartphone"),
            new Category(SeedDataConstants.PharmacyCategoryId, "Pharmacy", "pill"),
            new Category(SeedDataConstants.TransportCategoryId, "Transport", "bus"),
        ];
    }
}
