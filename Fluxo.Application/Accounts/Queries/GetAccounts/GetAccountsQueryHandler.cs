using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler(IFluxoDbContext context) : IGetAccountsQueryHandler
{
    public async Task<IEnumerable<AccountDto>> HandleAsync(GetAccountsQuery query, CancellationToken ct = default)
    {
        return await context.Accounts
            .AsNoTracking()
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Name = a.Name,
                Balance = a.Balance.Amount,
                Currency = a.Balance.Currency.Code
            })
            .ToListAsync(ct);
    }
}
