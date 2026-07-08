using Fluxo.Application.Accounts.Queries.GetAccounts;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler(IFluxoDbContext context) : IGetAccountByIdQueryHandler
{
    public async Task<AccountDto> HandleAsync(GetAccountByIdQuery query, CancellationToken ct)
    {
        return await context
                .Accounts.AsNoTracking()
                .Where(a => a.Id == query.Id)
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Balance = a.Balance.Amount,
                    Currency = a.Balance.Currency.Code,
                })
                .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException($"Account with ID {query.Id} was not found.");
    }
}
