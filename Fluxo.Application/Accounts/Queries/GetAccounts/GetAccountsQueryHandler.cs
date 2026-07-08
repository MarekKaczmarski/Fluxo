using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler(IFluxoDbContext context) : IGetAccountsQueryHandler
{
    public Task<List<AccountDto>> HandleAsync(GetAccountsQuery query, CancellationToken ct)
    {
        return context
            .Accounts.AsNoTracking()
            .OrderBy(a => a.Name)
            .Take(50)
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
