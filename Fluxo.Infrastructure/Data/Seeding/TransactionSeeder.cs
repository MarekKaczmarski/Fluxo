using Fluxo.Domain.Entities;
using Fluxo.Domain.Enums;
using Fluxo.Domain.ValueObjects;

namespace Fluxo.Infrastructure.Data.Seeding
{
    public static class TransactionSeeder
    {
        public static IEnumerable<Transaction> GetSeedData() =>
            new List<Transaction>
            {
                new Transaction(
                    SeedDataConstants.Transaction1Id,
                    Money.Positive(150.50m, Currency.FromCode("PLN")),
                    "Grocery",
                    new DateTime(2024, 05, 20, 10, 0, 0, DateTimeKind.Utc),
                    SeedDataConstants.FoodCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Expense
                ),
                new Transaction(
                    SeedDataConstants.Transaction2Id,
                    Money.Positive(3000.00m, Currency.FromCode("PLN")),
                    "Transfer",
                    new DateTime(2024, 05, 21, 12, 0, 0, DateTimeKind.Utc),
                    SeedDataConstants.TransferCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Income
                ),
                new Transaction(
                    SeedDataConstants.Transaction3Id,
                    Money.Positive(45.00m, Currency.FromCode("PLN")),
                    "Pharmacy",
                    new DateTime(2024, 05, 22, 09, 30, 0, DateTimeKind.Utc),
                    SeedDataConstants.PharmacyCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Expense
                ),
            };
    }
}
