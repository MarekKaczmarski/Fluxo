namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public interface IGetAccountsQueryHandler
{
    Task<List<AccountDto>> HandleAsync(GetAccountsQuery query, CancellationToken ct = default);
}
