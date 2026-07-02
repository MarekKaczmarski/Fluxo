using Fluxo.Domain.Entities;

namespace Fluxo.Infrastructure.Data.Seeding;

public static class AccountSeeder
{
    public static IEnumerable<Account> GetSeedData()
    {
        var account = new Account(
            SeedDataConstants.DefaultAccountId,
            "Main Wallet",
            5000.00m,
            "PLN"
        );

        foreach (var transaction in TransactionSeeder.GetSeedData())
        {
            account.ApplyTransaction(transaction.Amount, transaction.Type);
        }

        return [account];
    }
}
