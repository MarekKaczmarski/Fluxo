using Fluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Infrastructure.Data.Seeding;
public static class AccountSeeder
{
    public static IEnumerable<Account> GetSeedData() => new List<Account>
    {
        new Account(SeedDataConstants.DefaultAccountId, "Main Wallet", 5000.00m, "PLN")
    };
}
