namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(int PageNumber = 1, int PageSize = 20);
