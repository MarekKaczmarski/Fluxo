namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public record GetAccountsQuery(int PageNumber = 1, int PageSize = 10);
