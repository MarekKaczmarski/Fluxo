using Fluxo.Domain.Entities;
using Fluxo.Domain.Enums;
using Fluxo.Infrastructure.Data.Seeding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Infrastructure.Data.Seeding
{
    public static class TransactionSeeder
    {
        /* public static IEnumerable<Transaction> GetSeedData() => new List<Transaction>
        {
            new()
            {
                Id = SeedDataConstants.Transaction1Id,
                Description = "Grocery",
                Amount = -150.50m,
                Date = new DateTime(2024, 05, 20, 10, 0, 0, DateTimeKind.Utc),
                CategoryId = SeedDataConstants.FoodCategoryId,
                AccountId = SeedDataConstants.DefaultAccountId
            },
            new()
            {
                Id = SeedDataConstants.Transaction2Id,
                Description = "Transfer",
                Amount = 3000.00m,
                Date = new DateTime(2024, 05, 21, 12, 0, 0, DateTimeKind.Utc),
                CategoryId = SeedDataConstants.TransferCategoryId,
                AccountId = SeedDataConstants.DefaultAccountId
            },
            new()
            {
                Id = SeedDataConstants.Transaction3Id,
                Description = "Pharmacy",
                Amount = -45.00m,
                Date = new DateTime(2024, 05, 22, 09, 30, 0, DateTimeKind.Utc),
                CategoryId = SeedDataConstants.PharmacyCategoryId,
                AccountId = SeedDataConstants.DefaultAccountId
            }
        }; */

        public static IEnumerable<Transaction> GetSeedData() => new List<Transaction>
        {
                new Transaction(
                    SeedDataConstants.Transaction1Id,
                    150.50m,
                    "Grocery",
                    new DateTime(2024, 05, 20, 10, 0, 0, DateTimeKind.Utc),
                    SeedDataConstants.FoodCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Expense),

                new Transaction(
                    SeedDataConstants.Transaction2Id,
                    3000.00m,
                    "Transfer",
                    new DateTime(2024, 05, 21, 12, 0, 0, DateTimeKind.Utc),
                    SeedDataConstants.TransferCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Income),

                new Transaction(
                    SeedDataConstants.Transaction3Id,
                    45.00m,
                    "Pharmacy",
                    new DateTime(2024, 05, 22, 09, 30, 0, DateTimeKind.Utc),
                    SeedDataConstants.PharmacyCategoryId,
                    SeedDataConstants.DefaultAccountId,
                    TransactionType.Expense)
        };
    } 
}
