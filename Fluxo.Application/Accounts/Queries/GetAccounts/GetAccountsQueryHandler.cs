using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler(IFluxoDbContext context) : IGetAccountsQueryHandler
{
    public Task<List<AccountDto>> HandleAsync(
        GetAccountsQuery query,
        CancellationToken ct = default
    )
    {
        return context
            .Accounts.AsNoTracking()
            .OrderBy(a => a.Name)
            .Skip((Math.Max(query.PageNumber, 1) - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Name = a.Name,
                Balance = a.Balance.Amount,
                Currency = a.Balance.Currency.Code,
            })
            .ToListAsync(ct);
    }
}
