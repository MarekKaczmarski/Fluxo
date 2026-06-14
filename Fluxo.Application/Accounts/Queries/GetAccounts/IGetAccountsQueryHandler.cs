namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public interface IGetAccountsQueryHandler
{
    Task<IEnumerable<AccountDto>> HandleAsync(GetAccountsQuery query, CancellationToken ct = default);
}
