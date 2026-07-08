using Fluxo.Application.Accounts.Queries.GetAccounts;

namespace Fluxo.Application.Accounts.Queries.GetAccountById;

public interface IGetAccountByIdQueryHandler
{
    Task<AccountDto> HandleAsync(GetAccountByIdQuery query, CancellationToken ct);
}
